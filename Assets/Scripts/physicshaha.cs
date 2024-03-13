using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class physicshaha : MonoBehaviour
{
    public Transform playeroneposition;
    public Transform playertwoposition;
    public SpriteRenderer playeronesprite;
    public SpriteRenderer playertwosprite;
    public SpriteRenderer playertwogunspr;
    public SpriteRenderer playeronegunspr;
    void Update()
    {
        if (playeroneposition.position.y > playertwoposition.position.y)
        {
            playeronesprite.sortingOrder = 2;
            playeronegunspr.sortingOrder = 3;

            playertwosprite.sortingOrder = 5;
            playertwogunspr.sortingOrder = 6;
        }
        else
        {
            playeronesprite.sortingOrder = 5;
            playeronegunspr.sortingOrder = 6;

            playertwosprite.sortingOrder = 2;
            playertwogunspr.sortingOrder = 3;
        }
    }
}
