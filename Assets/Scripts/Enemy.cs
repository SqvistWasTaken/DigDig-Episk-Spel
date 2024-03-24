using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private PlayerMovement[] players;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    [Header("Enemy properties")]
    [SerializeField, Tooltip("The maximum range where the enemy will detect a player")] private float playerDetectionRange;
    [SerializeField, Tooltip("The range to stop and perform an attack if a player is within it")] private float attackRange;
    [SerializeField, Tooltip("The amount of time in seconds after an attack before another attack can be done")] private float attackCooldown;
    [SerializeField, Tooltip("The amount of time in seconds it takes for the attack prefab to be instatiated after the attack has started (to sync with animations)")] private float attackDelay;
    [SerializeField, Tooltip("The prefab that is instantiated during attacks to deal damage to players")] private GameObject attackPrefab;
    [SerializeField, Tooltip("The movement speed of the enemy")] private float moveSpeed;
    [SerializeField, Tooltip("The starting health of the enemy")] private float maxHealth;
    private float health;
    private bool isAttackCooldown;

    private float p1Distance, p2Distance;
    private Transform target;

    NavMeshAgent agent;

    void Start()
    {
        players = FindObjectsOfType<PlayerMovement>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        health = maxHealth;
        isAttackCooldown = false;

    agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        TargetPlayers(players.Length); //Sorry, the code looks like a mess
    }

    private void TargetPlayers(int playerAmount)
    {
        if (players.Length > 0) //Calculates the distance from players if found
        {
            p1Distance = Mathf.Abs(transform.position.x - players[0].transform.position.x) - (transform.position.y - players[0].transform.position.y);
        }
        if (players.Length > 1)
        {
            p2Distance = Mathf.Abs(transform.position.x - players[1].transform.position.x) - (transform.position.y - players[1].transform.position.y);
        }

        if(players.Length > 1) //Targets closest player if both players are found
        {
            if (p1Distance < p2Distance && p1Distance <= playerDetectionRange)
            {
                if (p1Distance <= attackRange)
                {
                    if (!isAttackCooldown)
                    {
                        Attack(0);
                    }
                }
                else
                {
                    anim.SetBool("Moving", true);
                    agent.SetDestination(players[0].transform.position);
                }
                FlipTowards(0);
            }
            else if (p2Distance < p1Distance && p2Distance <= playerDetectionRange)
            {
                if (p2Distance <= attackRange)
                {
                    if (!isAttackCooldown)
                    {
                        Attack(1);
                    }
                }
                else
                {
                    anim.SetBool("Moving", true);
                    agent.SetDestination(players[1].transform.position);
                }
                FlipTowards(1);
            }
            else
            {
                anim.SetBool("Moving", false);
            }
        }
        else if(players.Length > 0) //If only one of the players are found, target it
        {
            if (p1Distance <= attackRange)
            {
                if (!isAttackCooldown)
                {
                    Attack(0);
                }
            }
            else
            {
                anim.SetBool("Moving", true);
                agent.SetDestination(players[0].transform.position);
            }
            FlipTowards(0);
        }
        
    }

    private void FlipTowards(int player)
    {
        if (transform.position.x < players[player].transform.position.x) //FlipX if targeted player is to the left of the transform
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    private void Attack(int player)
    {
        if(players.Length >= player)
        {
            target = players[player].transform;
        }
        else
        {
            target = players[0].transform;
        }

        isAttackCooldown = true;
        anim.SetTrigger("Attack");

        Invoke(nameof(InstantiateAttackPrefab), attackDelay);
        Invoke(nameof(EndAttackCooldown), attackCooldown);
    }

    private void EndAttackCooldown()
    {
        isAttackCooldown = false;
    }

    private void InstantiateAttackPrefab()
    {
        Vector3 spawnPos = transform.position;

        Vector3 directionToPlayer = target.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x);
        float angleDegrees = Mathf.Rad2Deg * angle;
        Quaternion direction = Quaternion.Euler(0f, 0f, angleDegrees);

        Instantiate(attackPrefab, spawnPos, direction);
    }
}
