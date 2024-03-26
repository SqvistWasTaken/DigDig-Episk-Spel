using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus.Components;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [HideInInspector] public int room = 0;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private GameObject[] bossRooms;

    [SerializeField] public int bossRoomFrequency;

    [SerializeField] private TMP_Text roomIndicator;

    [SerializeField] private NavMeshSurface surface;

    public bool enemiesSpawned = true;

    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionTime;
    private Vector4 colorVector;

    [SerializeField] private AudioSource defaultMusicSource, bossMusicSource;

    [SerializeField] private GameObject train;

    [HideInInspector] public GameObject player1, player2;
    [SerializeField] private GameObject player1Prefab, player2Prefab;
    [SerializeField] private Vector2 player1SpawnPos, player2SpawnPos;

    private void Start()
    {
        enemiesSpawned = false;
        train.GetComponent<Train>().roomManager = this;
        colorVector = transitionImage.color;

        defaultMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        bossMusicSource.volume = 0;

        StartCoroutine(Transition(transitionTime));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Transition(transitionTime));
        }

        if (!FindObjectOfType<Enemy>() && enemiesSpawned) //If enemies are eliminated, go to next room
        {
            enemiesSpawned = false;
            StartCoroutine(Transition(transitionTime));

        }

        if (!FindObjectOfType<PlayerMovement>() || Input.GetButtonDown("MainMenu")) //If players are eliminated, or if button pressed, go to main menu
        {
            SceneManager.LoadScene(0);
        }
    }
    public void NextRoom()
    {
        room++;
        roomIndicator.text = room.ToString();

        if (player1) //Looks for player1
        {
            player1.transform.position = player1SpawnPos;
            player1.GetComponent<playerhealth>().health = player1.GetComponent<playerhealth>().maxHealth;
        }
        else //Respawns player1
        {
            player1 = Instantiate(player1Prefab, player1SpawnPos, Quaternion.identity);
        }
        if (player2) //Looks for player2
        {
            player2.transform.position = player2SpawnPos;
            player2.GetComponent<playerhealth>().health = player2.GetComponent<playerhealth>().maxHealth;
        }
        else //Respawns player2
        {
            player2 = Instantiate(player2Prefab, player2SpawnPos, Quaternion.identity);
        }

        bool isBossRoom = room % bossRoomFrequency == 0;

        if (isBossRoom)
        {
            int randomIndex = Random.Range(0, bossRooms.Length);
            InstantiateRoom(randomIndex, true);
            //Debug.Log("Instantiated boss room: " + randomIndex);

            StartCoroutine(TransitionMusic(true, transitionTime));
        }
        else
        {
            int randomIndex = Random.Range(0, rooms.Length);
            InstantiateRoom(randomIndex, false);
            //Debug.Log("Instantiated room: " + randomIndex);

            StartCoroutine(TransitionMusic(false, transitionTime));
        }
    }
    public void InstantiateRoom(int roomIndex, bool isBossRoom)
    {
        StartCoroutine(DestroyRooms());
        if (isBossRoom)
        {
            Instantiate(bossRooms[roomIndex]);
        }
        else
        {
            Instantiate(rooms[roomIndex]);
        }

        surface.BuildNavMesh(); //Update navmesh
        
    }

    IEnumerator DestroyRooms()
    {
        DestroyOnRoomLoad[] objectsToDestroy = FindObjectsOfType<DestroyOnRoomLoad>();
        foreach (DestroyOnRoomLoad objectToDestroy in objectsToDestroy)
        {
            //Debug.Log("Destroyed: " + objectToDestroy.gameObject.name);
            Destroy(objectToDestroy.gameObject);
        }
        yield return null;
    }

    IEnumerator Transition(float time)
    {
        int step = 0;
        float startAlpha = 0;
        float targetAlpha = 1;

        colorVector.w = startAlpha;
        transitionImage.color = colorVector;

        while (colorVector.w != targetAlpha)
        {
            colorVector.w += (step / time * Time.fixedDeltaTime);
            step++;

            colorVector.w = Mathf.Clamp01(colorVector.w);

            transitionImage.color = colorVector;

            yield return new WaitForFixedUpdate();
        }
        targetAlpha = 0;
        NextRoom();

        while (colorVector.w != targetAlpha)
        {
            colorVector.w += (step / time * Time.fixedDeltaTime);
            step--;

            colorVector.w = Mathf.Clamp01(colorVector.w);

            transitionImage.color = colorVector;

            yield return new WaitForFixedUpdate();
        }

        colorVector.w = 0;
        transitionImage.color = colorVector;

        StartCoroutine(train.GetComponent<Train>().TrainEntry(train.GetComponent<Train>().speed));
    }
    IEnumerator TransitionMusic(bool toBossSource, float time)
    {
        int step = 0;

        if (toBossSource)
        {
            while (bossMusicSource.volume < PlayerPrefs.GetFloat("MusicVolume"))
            {
                bossMusicSource.volume += (step / time * Time.fixedDeltaTime); //Increase boss source
                defaultMusicSource.volume -= (step / time * Time.fixedDeltaTime); //Decrease default source
                step++;

                yield return new WaitForFixedUpdate();
            }
            bossMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            defaultMusicSource.volume = 0;
        }
        else
        {
            while (defaultMusicSource.volume < PlayerPrefs.GetFloat("MusicVolume"))
            {
                defaultMusicSource.volume += (step / time * Time.fixedDeltaTime); //Increase default source
                bossMusicSource.volume -= (step / time * Time.fixedDeltaTime); //Decrease boss source
                step++;

                yield return new WaitForFixedUpdate();
            }
            defaultMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            bossMusicSource.volume = 0;
        }

        yield return null;
    }
}
