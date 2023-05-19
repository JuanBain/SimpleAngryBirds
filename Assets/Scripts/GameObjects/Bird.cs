using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    //Added Bird Configuration
    [Header("Bird Configuration Scriptable Object")]
    public BirdScriptableObject birdSO;

    public BirdState State { get; private set; }

    private float _destructionTime;

    //Re-ordered Fixed Update and Start

    private void Awake()
    {
        _destructionTime = birdSO.destructionTime;
    }
    void Start()
    {
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        //GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<CircleCollider2D>().radius = Constants.Bird_Collider_Radius_Big;
        State = BirdState.BeforeThrown;
    }

    void FixedUpdate()
    {
        if (State == BirdState.Thrown && GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.Min_Velocity)
            StartCoroutine(DestroyAfter(_destructionTime));
    }

    //Changed function Name
    public void ShootBird()
    {
        //Moved down instructions
        GetComponent<AudioSource>().Play(); 
        GetComponent<TrailRenderer>().enabled = true; 
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<CircleCollider2D>().radius = Constants.BirdColliderRadiusNormal; 
        State = BirdState.Thrown;
    }

    IEnumerator DestroyAfter(float seconds)  
    {  
        //Moved down Instructions
        yield return new WaitForSeconds(seconds); 
        Destroy(gameObject); 
    }

}
