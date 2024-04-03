using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthScript : MonoBehaviour
{
    [SerializeField] private bool isPlayer1;
    [SerializeField] private RoomManager roomManager;
    playerhealth script;
    private Image image;
    

    void Start()
    {
        GetScript();
        
        image = GetComponent<Image>();
        image.fillAmount = 1;
    }

    void Update()
    {
        if (script)
        {
            image.fillAmount = script.health / script.maxHealth;
        }
        else
        {
            GetScript();
        }
    }

    void GetScript()
    {
        if (isPlayer1)
        {
            if (roomManager.player1)
            {
                script = roomManager.player1.GetComponent<playerhealth>();
            }
            else
            {
                image.fillAmount = 0;
            }
        }
        else
        {
            if (roomManager.player2)
            {
                script = roomManager.player2.GetComponent<playerhealth>();
            }
            else
            {
                image.fillAmount = 0;
            }
        }
    }
}
