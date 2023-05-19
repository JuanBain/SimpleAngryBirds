using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    [Header("Structure config file")]
    [SerializeField] private StructureScriptableObject structureSO;

    private float Health = 70f;
    private float receivedDamage = 0f;
    private float explotionDamage = 150f;
    private float damage = 0f;
    private void Start()
    {
        if (structureSO != null)
        {
            Health = 70f;
        }
        else
        {
            Health = structureSO.lifePoints;
        }
        explotionDamage = 150f;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject collisioner = col.gameObject;
        if (collisioner.GetComponent<Rigidbody2D>() == null) return;
        damage = collisioner.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
        receivedDamage = damage;
        if (collisioner.CompareTag("Explotion"))
        {
            receivedDamage = explotionDamage;
        }
        CheckHealth(receivedDamage);
    }

    private void CheckHealth(float damage)
    {
        if (damage >= 10)
            GetComponent<AudioSource>().Play();
        Health -= damage;
        if (Health <= 0) Destroy(this.gameObject);
    }
}
