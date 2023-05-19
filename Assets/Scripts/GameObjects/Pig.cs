using UnityEngine;
using System.Collections;

public class Pig : MonoBehaviour
{
    //Added PIG configuration
    [SerializeField] private EnemySriptableObject pigSO;

    //Change order of variables and declarations, using SO configuration
    private float _health = 150f;
    private int _pointsToGiveWhenHit = 10;
    private int _pointsToGiveWhenDestroyed = 20;
    private float _receivedDamage = 0f;
    private float _explotionDamage = 0f;

    private void Start()
    {
        if (pigSO != null) 
        {
            _health = 150f;
            _pointsToGiveWhenHit = 10;
            _pointsToGiveWhenDestroyed = 20;
        }
        else
        {
            _health = pigSO.lifePoints;
            _pointsToGiveWhenHit = pigSO.pointsGivenWhenHit;
            _pointsToGiveWhenDestroyed = pigSO.pointsGivenWhenDestroyed;
        }
        _explotionDamage = 150f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Used declared game object instead of constant calling of Collision
        GameObject collisioner = col.gameObject;

        if (collisioner.GetComponent<Rigidbody2D>() == null) return;
        if (collisioner.CompareTag("Bird"))
        {
            float damage = collisioner.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            _receivedDamage = damage;
            CheckHealth(_receivedDamage);
            if (collisioner.GetComponent<Bird>().birdSO.explosiveBird)
            {
                var explotion = Instantiate(collisioner.GetComponent<Bird>().birdSO.explotion, collisioner.transform.position, Quaternion.identity);
                //Turn off gameobject visually
                collisioner.GetComponent<SpriteRenderer>().enabled = false;

                //Turn off collider so only hits once
                if (collisioner.GetComponent<CircleCollider2D>())
                {
                    collisioner.GetComponent<CircleCollider2D>().enabled = false;
                }
                Destroy(explotion, 1f);
            }
            //Destroy Bird Gameobject
            Destroy(collisioner, 1.5f);
        }

        //ExplotionCollision For Exploding Bird
        if (collisioner.CompareTag("Explotion"))
        {
            _receivedDamage = _explotionDamage;
            CheckHealth(_receivedDamage);
        }
    }
    private void CheckHealth(float damage)
    {
        if (damage >= 10)
            GetComponent<AudioSource>().Play();
        _health -= damage;
        if (_health <= 0)
        {
            callAddPoints(_pointsToGiveWhenDestroyed);
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
        else
        {
            callAddPoints(_pointsToGiveWhenHit);
        }
    }
    private void callAddPoints(int points)
    {
        GameManager.Instance.scoreManager.AddPoints(points);
        GameManager.Instance.scoreManager.StartCoroutine(GameManager.Instance.scoreManager.ShowAddingPoints());
    }
}
