using System.Collections;
using System.Collections.Generic;
using TMPro;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("---  Game Manager ---")]
    [SerializeField] private GameManager _gameManager;

    [Header("Buttons")]
    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _rigithArrowButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _endButton;
    [SerializeField] private Button _retryButton;

    [Header("Birds GameObject")]
    [SerializeField] private GameObject[] _birds;

    [Header("Bird Image")]
    [SerializeField] private Image _birdImage;

    [Header("Bird Name and type")]
    [SerializeField] private TextMeshProUGUI _birdName;
    [SerializeField] private TextMeshProUGUI _birdType;

    private int _choosedBird = 0;
    private const int LEFT = -1;
    private const int RIGHT = 1;

    private void Start()
    {
        //Listeners for buttons & UI pre order
        initialize();
    }

    private void initialize()
    {
        _birdImage.sprite = _birds[0].GetComponent<SpriteRenderer>().sprite;

        _leftArrowButton?.onClick.RemoveAllListeners();
        _rigithArrowButton?.onClick.RemoveAllListeners();
        _startButton?.onClick.RemoveAllListeners();
        _endButton?.onClick.RemoveAllListeners();
        _retryButton?.onClick.RemoveAllListeners();

        _leftArrowButton?.onClick.AddListener(onLeftArrowClicked);
        _rigithArrowButton?.onClick?.AddListener(onRightArrowClicked);
        _startButton?.onClick.AddListener(()=> StartGame(_choosedBird));
        _endButton?.onClick.AddListener(onExitButtonClicked);
        _retryButton.onClick.AddListener(onRetryButtonClicked);
        changeBird(0);
    }

    private void onLeftArrowClicked()
    {
        changeBird(LEFT);
    }

    private void onRightArrowClicked()
    {
        changeBird(RIGHT);
    }
    
    private void onRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.CurrentGameState = GameState.Menu;
    }

    private void onExitButtonClicked()
    {
        Application.Quit();
    }

    private void changeBird(int value)
    {
        _choosedBird += value;
        if (_choosedBird < 0 )
        {
            _choosedBird = _birds.Length-1;
        }
        else if (_choosedBird > _birds.Length-1)
        {
            _choosedBird = 0;
        }

        _birdName.text = _birds[_choosedBird].GetComponent<Bird>().birdSO.birdName;
        _birdType.text = BirdType(_birds[_choosedBird].GetComponent<Bird>());
        _birdImage.sprite = _birds[_choosedBird].GetComponent<SpriteRenderer>().sprite;
    }
    private string BirdType(Bird birdGO)
    {
        string type = "";
        if (birdGO.birdSO.explosiveBird)
        {
            type = "Explosive bird!";
            return type;
        }
        else if (birdGO.birdSO.multipleBird)
        {
            type = "Multiple bird!";
            return type;
        }
        else
        {
            type = "Normal bird!";
            return type;
        }
    }
    private void StartGame(int choosedBird)
    {
        _gameManager.StartGame(choosedBird);
    }

}
