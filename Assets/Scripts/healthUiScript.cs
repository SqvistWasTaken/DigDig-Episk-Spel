using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] playerhealth script;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = script.currentHealth/script.startingHealth;
    }
}
