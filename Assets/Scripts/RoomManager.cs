using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    private int room = 0;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private GameObject[] bossRooms;

    [SerializeField] private int bossRoomFrequency;

    private bool enemiesSpawned = true;

    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionTime;
    private Vector4 colorVector;

    private void Start()
    {
        colorVector = transitionImage.color;
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
        else if (FindObjectOfType<PlayerMovement>())
        {
            enemiesSpawned = true;
        }
    }
    public void NextRoom()
    {
        room++;

        bool isBossRoom = room % bossRoomFrequency == 0;

        if (isBossRoom)
        {
            Debug.Log("Boss room");
            int randomIndex = Random.Range(0, bossRooms.Length);
            InstantiateRoom(randomIndex, true);
            Debug.Log("Instantiated boss room: " + randomIndex);
        }
        else
        {
            int randomIndex = Random.Range(0, rooms.Length);
            InstantiateRoom(randomIndex, false);
            Debug.Log("Instantiated room: " + randomIndex);
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
    }

    IEnumerator DestroyRooms()
    {
        DestroyOnRoomLoad[] objectsToDestroy = FindObjectsOfType<DestroyOnRoomLoad>();
        foreach (DestroyOnRoomLoad objectToDestroy in objectsToDestroy)
        {
            Debug.Log("Destroyed: " + objectToDestroy.gameObject.name);
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
            colorVector.w += (step / time * Time.deltaTime);
            step++;

            colorVector.w = Mathf.Clamp01(colorVector.w);

            transitionImage.color = colorVector;

            yield return new WaitForFixedUpdate();
        }
        targetAlpha = 0;
        NextRoom();

        while (colorVector.w != targetAlpha)
        {
            colorVector.w += (step / time * Time.deltaTime);
            step--;

            colorVector.w = Mathf.Clamp01(colorVector.w);

            transitionImage.color = colorVector;

            yield return new WaitForEndOfFrame();
        }

        colorVector.w = 0;
        transitionImage.color = colorVector;
    }

}
