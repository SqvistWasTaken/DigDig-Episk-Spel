using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust this value to control movement speed
    private Vector2 pos;
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        pos += new Vector2(xInput * speed * Time.deltaTime, yInput * speed * Time.deltaTime); // Adds horizontal & vertical movement to pos

        transform.position = pos; // Moves the transform to pos
    }
}
