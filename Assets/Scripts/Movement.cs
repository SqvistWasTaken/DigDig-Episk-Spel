using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust this value to control movement speed
    private Vector2 pos;
    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] public Animator anim;
    [SerializeField] private bool isPlayer1;
    private float xInput, yInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        anim.SetFloat("Move", Mathf.Abs(rb.velocity.magnitude));

        if(xInput > 0)
        {
            playerSprite.flipX = false;
        }
        else if (xInput < 0)
        {
            playerSprite.flipX = true;
        }

        //pos += new Vector2(xInput * speed * Time.deltaTime, yInput * speed * Time.deltaTime); // Adds horizontal & vertical movement to pos

        //transform.position = pos; // Moves the transform/position to pos
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(pos.x * speed, pos.y * speed);
    }
}