using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{

    //Singleton
    public static GameManager Instance { get; set; }
    [HideInInspector]
    public static GameState CurrentGameState = GameState.Menu;
  
    [Header("Birds Game Objects")]
    [SerializeField] private List<GameObject> _birds;

    [Header("Enemy Building")]
    [SerializeField] private GameObject _enemyBuilding;

    [Header("Birds Initial Position")]
    [SerializeField] private Transform[] _birdsPosition;

    [Header("Score Manager")]
    public ScoreManager scoreManager;

    [Header("UI Canvas GameObjects")]
    [SerializeField] private GameObject _gameplayMenu;
    [SerializeField] private GameObject _gameOverMenu;

    [Header("Time to call GAME OVER GameObject")]
    [SerializeField] private float _gameOverCallTime=1f;

    private List<GameObject> _enemies;
    private List<GameObject> _bricks;
    private List<GameObject> _birdsList;
    private float _slingshotHoldLimit = 5f;

    public CameraFollow cameraFollow;
    int currentBirdIndex;
    public SlingShot slingshot;


    private void Awake()
    {
        //Creating instance for Singleton
        Instance = this;
    }
    void Start()
    {
        //CurrentGameState = GameState.Start;
        slingshot.enabled = false;
        //Not necesary the Find Game object function
        _bricks = new List<GameObject>();
        _birdsList = new List<GameObject>();
        _enemies = new List<GameObject>();
        slingshot.BirdThrown -= SlingshotBirdThrown; 
        slingshot.BirdThrown += SlingshotBirdThrown;
    }

    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Start:
                if (Input.GetMouseButtonUp(0))
                    AnimateBirdToSlingshot();
                break;
            case GameState.BirdMovingToSlingshot:
                break;
            case GameState.Playing:
                if (slingshot.slingshotState == SlingshotState.BirdFlying &&
                    (BricksBirdsPigsStoppedMoving() || Time.time - slingshot.TimeSinceThrown > _slingshotHoldLimit))
                {
                    slingshot.enabled = false;
                    AnimateCameraToStartPosition();
                    CurrentGameState = GameState.BirdMovingToSlingshot;
                }
                break;
            case GameState.Won:
            case GameState.Lost:
                StartCoroutine(GameOver());
                break;
            default:
                break;
        }
    }
    
    //Change Function Name
    private bool DestroyAllPigs()
    {
        return _enemies.All(x => x == null);
    }

    private void AnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
        if (duration == 0.0f) duration = 0.1f;
        //animate the camera to start
        Camera.main.transform.positionTo
            (duration,
            cameraFollow.StartingPosition). //end position
            setOnCompleteHandler((x) =>
            {
                cameraFollow.IsFollowing = false;
                if (DestroyAllPigs())
                    CurrentGameState = GameState.Won;
                else if (currentBirdIndex == _birdsList.Count - 1)
                    CurrentGameState = GameState.Lost;
                else
                {
                    slingshot.slingshotState = SlingshotState.Idle;
                    currentBirdIndex++;
                    AnimateBirdToSlingshot();
                }
            });
    }

    void AnimateBirdToSlingshot()
    {
        CurrentGameState = GameState.BirdMovingToSlingshot;
        _birdsList[currentBirdIndex].transform.positionTo
            (Vector2.Distance(_birdsList[currentBirdIndex].transform.position / 10,
            slingshot.BirdWaitPosition.transform.position) / 10, //duration
            slingshot.BirdWaitPosition.transform.position). //final position
                setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy();
                            CurrentGameState = GameState.Playing;
                            slingshot.enabled = true;
                            slingshot.BirdToThrow = _birdsList[currentBirdIndex];
                        });
    }

    //Changed Function Name, erased: _
    private void SlingshotBirdThrown(object sender, System.EventArgs e)
    {
        cameraFollow.BirdToFollow = _birdsList[currentBirdIndex].transform;
        cameraFollow.IsFollowing = true;
    }

    bool BricksBirdsPigsStoppedMoving()
    {
        foreach (var item in _bricks.Union(_birdsList).Union(_enemies))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.Min_Velocity)
            {
                return false;
            }
        }

        return true;
    }

    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }


    public void StartGame(int choosedBird)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject BirdGO = Instantiate(_birds[choosedBird]);
            BirdGO.transform.position = new Vector3(_birdsPosition[i].transform.position.x, _birdsPosition[i].transform.position.y, _birdsPosition[i].transform.position.z);
            _birdsList.Add(BirdGO);
        }
        foreach (Transform child in _enemyBuilding.transform)
        {
            if (child.gameObject.CompareTag("Brick"))
            {
                _bricks.Add(child.gameObject);
            }
            if (child.gameObject.CompareTag("Pig"))
            {
                _enemies.Add(child.gameObject);
            }
        }
        CurrentGameState = GameState.Start;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(_gameOverCallTime);
        _gameplayMenu.SetActive(false);
        _gameOverMenu.SetActive(true);
        scoreManager.SetFinalScore();
    }
}
