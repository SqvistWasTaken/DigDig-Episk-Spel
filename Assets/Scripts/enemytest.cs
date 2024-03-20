using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemytest : MonoBehaviour
{
    private PlayerMovement[] players;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(players[1].transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("lalalala");
    }
}
