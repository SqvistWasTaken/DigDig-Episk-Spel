using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemytest : MonoBehaviour
{
    public Transform player1;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player1.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("lalalala");
    }
}
