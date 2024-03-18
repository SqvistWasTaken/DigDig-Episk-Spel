using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] playerhealth script;
    [SerializeField] private bool isPlayer1 = true;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1)
        {
            image.fillAmount = script.currentHealth / script.startingHealth;
        }

        if(isPlayer1 == false)
        {
            image.fillAmount = script.currentHealth2 / script.startingHealth2;
        }
    }
}
