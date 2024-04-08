using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus.Components;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviour
{
    [HideInInspector] public int room = 0;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private GameObject[] bossRooms;

    [SerializeField] public int bossRoomFrequency;
    private bool isBossRoom;

    [SerializeField] private TMP_Text roomIndicator;
    [SerializeField] private Vector2 indicatorBossOffset;
    private Vector2 indicatorDefaultPos;
    [SerializeField] private Image bossHealthUI, bossHealthBar;

    [SerializeField] private NavMeshSurface surface;

    [HideInInspector] public bool enemiesSpawned = true;

    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionTime, transitionStartDelay;
    private bool transitionStarted = false;
    private Vector4 colorVector;

    [SerializeField] private AudioSource transitionSource, defaultMusicSource, bossMusicSource;
    [SerializeField] private AudioClip transitionSound;

    [SerializeField] private GameObject train;

    [HideInInspector] public GameObject player1, player2;
    [SerializeField] private GameObject player1Prefab, player2Prefab;
    [SerializeField] private Vector2 player1SpawnPos, player2SpawnPos;

    private void Start()
    {
        train.GetComponent<Train>().roomManager = this;
        colorVector = transitionImage.color;

        indicatorDefaultPos = roomIndicator.transform.position;

        defaultMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        bossMusicSource.volume = 0;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T)) //Just for testing
        {
            StartCoroutine(Transition(transitionTime));
        }*/

        if (!FindObjectOfType<Enemy>() && enemiesSpawned) //If enemies are eliminated, go to next room
        {
            enemiesSpawned = false;
            StartCoroutine(Transition(transitionTime));

        }

        if (!FindObjectOfType<PlayerMovement>() || Input.GetButtonDown("MainMenu")) //If players are eliminated, or if button pressed, go to main menu
        {
            SceneManager.LoadScene(0);
        }

        HandleUI();
    }
    public void NextRoom()
    {
        room++;
        roomIndicator.text = room.ToString();

        transitionSource.PlayOneShot(transitionSound);

        if (player1) //Looks for player1
        {
            player1.transform.position = player1SpawnPos;
            player1.GetComponent<PlayerHealth>().health = player1.GetComponent<PlayerHealth>().maxHealth;
        }
        else //Respawns player1
        {
            player1 = Instantiate(player1Prefab, player1SpawnPos, Quaternion.identity);
        }
        if (player2) //Looks for player2
        {
            player2.transform.position = player2SpawnPos;
            player2.GetComponent<PlayerHealth>().health = player2.GetComponent<PlayerHealth>().maxHealth;
        }
        else //Respawns player2
        {
            player2 = Instantiate(player2Prefab, player2SpawnPos, Quaternion.identity);
        }

        isBossRoom = room % bossRoomFrequency == 0;

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
        if (!transitionStarted)
        {
            yield return new WaitForSeconds(transitionStartDelay);
            transitionStarted = true;
        }

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
        transitionStarted = false;
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

    private void HandleUI()
    {
        if (isBossRoom && enemiesSpawned)
        {
            Enemy boss = FindObjectOfType<Enemy>();
            roomIndicator.transform.position = new Vector2(indicatorDefaultPos.x + indicatorBossOffset.x, indicatorDefaultPos.y + indicatorBossOffset.y);

            bossHealthUI.gameObject.SetActive(true);
            bossHealthBar.fillAmount = boss.health / boss.maxHealth;
        }
        else
        {
            roomIndicator.transform.position = new Vector2(indicatorDefaultPos.x, indicatorDefaultPos.y);
            bossHealthUI.gameObject.SetActive(false);
        }
    }
}
