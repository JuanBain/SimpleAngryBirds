using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;

    [HideInInspector]
    public Vector3 StartingPosition;

    private const float minCameraX = 0;
    private const float maxCameraX = 13;

    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform BirdToFollow;

    void Start()
    {
        StartingPosition = transform.position;
    }
    void Update()
    {
        //Erased doble If, only one needed
        if (IsFollowing && BirdToFollow != null)
            {
                var birdPosition = BirdToFollow.transform.position;
                float x = Mathf.Clamp(birdPosition.x, minCameraX, maxCameraX);
                transform.position = new Vector3(x, StartingPosition.y, StartingPosition.z);
            }
            else
                IsFollowing = false;
    }
}
