using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerhealth : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    public float currentHealth;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float Takedamage)
    {
        currentHealth = Mathf.Clamp(currentHealth - Takedamage, 0, startingHealth);

        if (currentHealth == 0)
        {
            //player dies (generally in a single hit and gets sent back to main menu scene.
        }
    }
}
