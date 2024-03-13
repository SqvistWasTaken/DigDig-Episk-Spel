using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
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
            StartCoroutine(Transition(false, transitionTime));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(Transition(true, transitionTime));
        }

        if (!FindObjectOfType<PlayerMovement>() && enemiesSpawned)
        {
            enemiesSpawned = false;

            int randomIndex = Random.Range(0, rooms.Length);
            InstantiateRoom(randomIndex);

            Debug.Log("Instantiated room: " + randomIndex);
        }
        else if (FindObjectOfType<PlayerMovement>())
        {
            enemiesSpawned = true;
        }
    }

    public void InstantiateRoom(int roomIndex)
    {
        StartCoroutine(DestroyRooms());
        Instantiate(rooms[roomIndex]);
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

    IEnumerator Transition(bool transitionOut, float time)
    {
        Debug.Log("transitioning");

        colorVector.w = 1;
        transitionImage.color = colorVector;
        if (transitionOut)
        {
            if (colorVector.w != 0)
            {
                colorVector.w -= 0.1f;
            }
            else { yield return null; }
        }
        else
        {
            if (colorVector.w != 1)
            {
                colorVector.w += 0.1f;
            }
            else { yield return null; }
        }
        
        yield return new WaitForSeconds(time);
    }
}
