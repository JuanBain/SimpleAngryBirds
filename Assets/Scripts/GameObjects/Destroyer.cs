using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D col)
    {
        //Created gameobject for referencing and used CompareTag instead of ==
        GameObject collisioner = col.gameObject;
        if(collisioner.CompareTag("Brick") || collisioner.CompareTag("Pig") || collisioner.CompareTag("Bird"))
        {
            Destroy(collisioner);
        }
    }
}
