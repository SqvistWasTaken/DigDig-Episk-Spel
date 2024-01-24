using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust this value to control movement speed
    private Vector2 pos;
    [SerializeField] private bool isPlayer1;
    private float xInput, yInput;

    void Update()
    {
        if (isPlayer1)
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }
        else
        {
            xInput = Input.GetAxis("Horizontal2");
            yInput = Input.GetAxis("Vertical2");
        }



        pos += new Vector2(xInput * speed * Time.deltaTime, yInput * speed * Time.deltaTime); // Adds horizontal & vertical movement to pos

        transform.position = pos; // Moves the transform/position to pos
    }
}

