using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [HideInInspector] public int room = 0;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private GameObject[] bossRooms;

    [SerializeField] public int bossRoomFrequency;

    [SerializeField] private TMP_Text roomIndicator;

    public bool enemiesSpawned = true;

    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionTime;
    private Vector4 colorVector;

    [SerializeField] private AudioSource defaultMusicSource, bossMusicSource;

    [SerializeField] private GameObject train;

    [SerializeField] private GameObject player1, player2;
    [SerializeField] private Vector2 player1SpawnPos, player2SpawnPos;

    private void Start()
    {
        train.GetComponent<Train>().roomManager = this;
        colorVector = transitionImage.color;

        PlayerPrefs.SetFloat("MusicVolume", 1f);
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

        if (!FindObjectOfType<PlayerMovement>() && enemiesSpawned)
        {
            enemiesSpawned = false;
            StartCoroutine(Transition(transitionTime));

        }
    }
    public void NextRoom()
    {
        room++;
        roomIndicator.text = room.ToString();

        player1.transform.position = player1SpawnPos;
        player2.transform.position = player2SpawnPos;

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
            BuildNavMesh(bossRooms[roomIndex].transform);
        }
        else
        {
            Instantiate(rooms[roomIndex]);
            BuildNavMesh(rooms[roomIndex].transform);
        }
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

    private void BuildNavMesh(Transform xform)
    {
        // Delete the existing NavMesh if there is one
        NavMesh.RemoveAllNavMeshData();

        // Collect sources (surfaces to walk on or obstacles to avoid)
        List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(xform, -1, NavMeshCollectGeometry.RenderMeshes, 0, new List<NavMeshBuildMarkup>(), buildSources);

        // Define the bounds for the NavMesh
        Bounds bounds = new Bounds(Vector2.zero, new Vector2(10, 10));

        // Build the NavMeshData
        NavMeshData navData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0), // Use default settings (you can customize this)
            buildSources,
            bounds,
            Vector3.down, // Up direction (e.g., for walkable vertical surfaces)
            Quaternion.Euler(Vector3.up) // Rotation (optional)
        );

        // Add the NavMeshData
        NavMesh.AddNavMeshData(navData);
    }
}
