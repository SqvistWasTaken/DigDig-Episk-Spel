using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust this value to control movement speed
    private Vector2 pos;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private bool isPlayer1;
    private float xInput, yInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

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

        pos = new Vector2(xInput, yInput);
        pos.Normalize();
        rb.velocity = new Vector2(pos.x * speed * 100f * Time.deltaTime, pos.y * speed * 100f * Time.deltaTime);
        
        if (xInput > 0) // Flip X in the player's movement direction
        {
            sprite.flipX = false;
        }
        else if (xInput < 0)
        {
            sprite.flipX = true;
        }

        anim.SetFloat("Move", Mathf.Abs(rb.velocity.magnitude));

        //pos += new Vector2(xInput * speed * Time.deltaTime, yInput * speed * Time.deltaTime); // Adds horizontal & vertical movement to pos

        //transform.position = pos; // Moves the transform/position to pos
    }
}
