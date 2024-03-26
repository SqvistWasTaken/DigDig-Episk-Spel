using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private playerhealth script;
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (script != null)
        {
            if (collision.CompareTag("EnemyBullet"))
            {
                script = collision.GetComponent<playerhealth>();
                Debug.Log("Collision");
                Debug.Log(script.health);
            }
        }
    }
}
