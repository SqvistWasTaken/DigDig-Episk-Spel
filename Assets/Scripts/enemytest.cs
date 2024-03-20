using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemytest : MonoBehaviour
{
    private PlayerMovement[] players;
    private Animator anim;

    [SerializeField] private float playerDetectionRange;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerMovement>();
        agent = GetComponentInChildren<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float p1Distance = Mathf.Abs(transform.position.x - players[0].transform.position.x)-(transform.position.y - players[0].transform.position.y);
        float p2Distance = Mathf.Abs(transform.position.x - players[1].transform.position.x) - (transform.position.y - players[1].transform.position.y);

        if (p1Distance < p2Distance && p1Distance <= playerDetectionRange)
        {
            anim.SetBool("Moving", true);
            agent.SetDestination(players[0].transform.position);
        }
        else if (p2Distance < p1Distance && p2Distance <= playerDetectionRange)
        {
            anim.SetBool("Moving", true);
            agent.SetDestination(players[1].transform.position);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("lalalala");
    }
}
