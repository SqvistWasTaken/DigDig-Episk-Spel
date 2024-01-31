using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    
    private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
  

        
    }
}
