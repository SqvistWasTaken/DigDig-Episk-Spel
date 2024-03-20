using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private Vector2 entryPoint, destination, exitPoint;
    [SerializeField, Range(0,1)] public float speed;
    [SerializeField] private float destStandByTime;

    [SerializeField] private GameObject[] wagons;
    [SerializeField] private Sprite[] wagonSprites;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float enemySpawnCount;
    [SerializeField] private GameObject[] bosses;

    [HideInInspector] public RoomManager roomManager;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator TrainEntry(float speed)
    {
        transform.position = entryPoint;

        foreach(GameObject wagon in wagons)
        {
            wagon.GetComponent<SpriteRenderer>().sprite = wagonSprites[Random.Range(0,wagonSprites.Length)];
        }

        float step = 0;
        anim.SetBool("Moving", true);

        while (step < 1)
        {
            transform.position = Vector2.Lerp(entryPoint, destination, step);
            step += speed;
            step = Mathf.Clamp01(step);

            yield return new WaitForFixedUpdate();
        }
        transform.position = destination;

        anim.SetBool("Moving", false);

        yield return new WaitForSeconds(destStandByTime);

        bool isBossRoom = roomManager.room % roomManager.bossRoomFrequency == 0;
        if (isBossRoom)
        {
            Instantiate(bosses[Random.Range(0, bosses.Length)]);
        }
        else
        {
            for (int i = 0; i < enemySpawnCount;)
            {
                i++;
                Instantiate(enemies[Random.Range(0, enemies.Length)], Vector3.zero, Quaternion.identity);
            }
        }
        
        roomManager.enemiesSpawned = true;

        yield return StartCoroutine(TrainExit(speed));
    }

    public IEnumerator TrainExit(float speed)
    {
        float step = 0;
        anim.SetBool("Moving", true);

        while (step < 1)
        {
            transform.position = Vector2.Lerp(destination, exitPoint, step);
            step += speed;
            step = Mathf.Clamp01(step);

            yield return new WaitForFixedUpdate();
        }
        transform.position = exitPoint;

        anim.SetBool("Moving", false);

        yield return null;
    }
}
