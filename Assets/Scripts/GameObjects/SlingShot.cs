using UnityEngine;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;

public class SlingShot : MonoBehaviour
{
    [SerializeField]
    private string sortingLayerNameString = "Foreground";

    [SerializeField]
    private float _timeToDivideBird = 0.25f;

    [SerializeField]
    private float _minRandomRangeY;

    [SerializeField]
    private float _maxRandomRangeY;

    [SerializeField]
    private float _minRandomRangeX;

    [SerializeField]
    private float _maxRandomRangeX;

    [HideInInspector]
    public float TimeSinceThrown;

    [HideInInspector]
    public GameObject BirdToThrow;
    //Changed Position of variables & declarations
    private Vector3 SlingshotMiddleVector;

    [HideInInspector]
    public SlingshotState slingshotState;

    public Transform LeftSlingshotOrigin, RightSlingshotOrigin;
    public Transform BirdWaitPosition;

    public LineRenderer SlingshotLineRenderer1;
    public LineRenderer SlingshotLineRenderer2;
    public LineRenderer TrajectoryLineRenderer;

    public float ThrowSpeed;

    private int _segmentCount = 15;
    private int _segmentScale = 2;
    private float _setStringshotLimit = 1.5f;
    private float _newBirdDistance =0.5f;
    private float _newLineDistance = 0.5f;
    private float _slingshotLimit;

    void Start()
    {
        SlingshotLineRenderer1.sortingLayerName = sortingLayerNameString;
        SlingshotLineRenderer2.sortingLayerName = sortingLayerNameString;
        TrajectoryLineRenderer.sortingLayerName = sortingLayerNameString;

        slingshotState = SlingshotState.Idle;
        SlingshotLineRenderer1.SetPosition(0, LeftSlingshotOrigin.position);
        SlingshotLineRenderer2.SetPosition(0, RightSlingshotOrigin.position);

        SlingshotMiddleVector = new Vector3((LeftSlingshotOrigin.position.x + RightSlingshotOrigin.position.x) / 2,
            (LeftSlingshotOrigin.position.y + RightSlingshotOrigin.position.y) / 2, 0);

        _slingshotLimit = _setStringshotLimit;
    }

    void Update()
    {
        switch (slingshotState)
        {
            case SlingshotState.Idle:
                InitializeBird();
                DisplaySlingshotLineRenderers();
                IdleSlingshot();
                break;
            case SlingshotState.UserPulling:
                DisplaySlingshotLineRenderers();

                if (Input.GetMouseButton(0))
                {
                    UserPulling();
                }
                else 
                {
                    BirdFliying();
                }
                break;
            case SlingshotState.BirdFlying:
                break;
            default:
                break;
        }

    }
    private void IdleSlingshot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (BirdToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
            {
                slingshotState = SlingshotState.UserPulling;
            }
        }
    }
    private void UserPulling()
    {
        Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location.z = 0;
        //Changing Magic Numbers
        if (Vector3.Distance(location, SlingshotMiddleVector) > _slingshotLimit)
        {
            var maxPosition = (location - SlingshotMiddleVector).normalized * _slingshotLimit + SlingshotMiddleVector;
            BirdToThrow.transform.position = maxPosition;
        }
        else
        {
            BirdToThrow.transform.position = location;
        }
        float distance = Vector3.Distance(SlingshotMiddleVector, BirdToThrow.transform.position);
        ShowThrownLine(distance);
    }
    private void BirdFliying()
    {
        SetTrajectoryLineRenderesActive(false);
        TimeSinceThrown = Time.time;
        float distance = Vector3.Distance(SlingshotMiddleVector, BirdToThrow.transform.position);
        if (distance > 1)
        {
            SetSlingshotLineRenderersActive(false);
            slingshotState = SlingshotState.BirdFlying;
            ShotBird(distance);
        }
        else
        {
            BirdToThrow.transform.positionTo(distance / 10,
                BirdWaitPosition.transform.position).
                setOnCompleteHandler((x) =>
                {
                    x.complete();
                    x.destroy();
                    InitializeBird();
                });

        }
    }
    private void ShotBird(float distance)
    {
        Vector3 velocity = SlingshotMiddleVector - BirdToThrow.transform.position;
        BirdToThrow.GetComponent<Bird>().ShootBird();
        BirdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;
        if (BirdToThrow.GetComponent<Bird>().birdSO.multipleBird)
        {
            StartCoroutine(DivideBird(_timeToDivideBird));
        }
        if (BirdThrown != null)
            BirdThrown(this, EventArgs.Empty);
    }

    public event EventHandler BirdThrown;

    private void InitializeBird()
    {
        BirdToThrow.transform.position = BirdWaitPosition.position;
        slingshotState = SlingshotState.Idle;
        SetSlingshotLineRenderersActive(true);
    }

    void DisplaySlingshotLineRenderers()
    {
        SlingshotLineRenderer1.SetPosition(1, BirdToThrow.transform.position);
        SlingshotLineRenderer2.SetPosition(1, BirdToThrow.transform.position);
    }

    void SetSlingshotLineRenderersActive(bool active)
    {
        SlingshotLineRenderer1.enabled = active;
        SlingshotLineRenderer2.enabled = active;
    }

    void SetTrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }

    //Changed Function Name
    void ShowThrownLine(float distance)
    {
        SetTrajectoryLineRenderesActive(true);
        Vector3 v2 = SlingshotMiddleVector - BirdToThrow.transform.position;
        int segmentCount = _segmentCount;
        float segmentScale = _segmentScale;
        Vector2[] segments = new Vector2[segmentCount];

        segments[0] = BirdToThrow.transform.position;
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;

        float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        float time = segmentScale / segVelocity.magnitude;
        for (int i = 1; i < segmentCount; i++)
        {
            float time2 = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * time2 + _newLineDistance * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        TrajectoryLineRenderer.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }

    IEnumerator DivideBird(float seconds) 
    {
        List<GameObject> dividedBirds = new List<GameObject>();
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < 3; i++)
        {
            GameObject newBird = Instantiate(BirdToThrow,BirdToThrow.transform.position, Quaternion.identity);
            newBird.GetComponent<Rigidbody2D>().velocity = BirdToThrow.GetComponent<Rigidbody2D>().velocity;
            newBird.GetComponent<Rigidbody2D>().angularVelocity = BirdToThrow.GetComponent<Rigidbody2D>().angularVelocity;
            newBird.transform.localScale -= new Vector3(_newBirdDistance, _newBirdDistance, _newBirdDistance);
            dividedBirds.Add(newBird);
        }
        BirdToThrow.GetComponent<SpriteRenderer>().enabled = false;
        BirdToThrow.GetComponent<CircleCollider2D>().enabled = false;
        BirdToThrow.GetComponent<TrailRenderer>().enabled = false;
        for (int i = 0; i < dividedBirds.Count; i++)
        {
            dividedBirds[i].transform.SetParent(BirdToThrow.transform);
            dividedBirds[i].transform.position = new Vector3(
                dividedBirds[i].transform.position.x,
                dividedBirds[i].transform.position.y,
                dividedBirds[i].transform.position.z
                );
            if (i%2==0)
            {
                dividedBirds[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(
                    dividedBirds[i].gameObject.GetComponent<Rigidbody2D>().velocity.x * (UnityEngine.Random.Range(_minRandomRangeX, _maxRandomRangeX)),
                    dividedBirds[i].gameObject.GetComponent<Rigidbody2D>().velocity.y * (UnityEngine.Random.Range(_minRandomRangeY, _maxRandomRangeY))
                    );
            }
        }
    }
}
