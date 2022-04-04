using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float height;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Rollaball inputs;

    private InputAction jump;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private int jumpsAvailable;
    private float jumpActiveCountdown;
    private bool doJump;

    private void Awake()
    {
        //Debug.Log("Awake");
        inputs = new Rollaball();
        jumpActiveCountdown = 0;
        doJump = false;
        jumpsAvailable = 2;
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable");
        jump = inputs.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void OnDisable()
    {
        //Debug.Log("OnDisable");
        jump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start");
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void OnMove(InputValue movementValue)
    {
        //Debug.Log("OnMove");
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        if (jumpActiveCountdown <= .05 && jumpsAvailable > 0)
        {
            doJump = true; // trigger next jump
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed / (3f-jumpsAvailable));
        
        if (jumpActiveCountdown > 0)
        {
            jumpActiveCountdown -= Time.deltaTime;
        }


        if (doJump && jumpActiveCountdown <= 0)
        {
            Debug.Log("Trying to jump");
            jumpActiveCountdown = .3f; //.3 seconds before you can jump again
            rb.AddForce(Vector3.up * 10 * height);
            doJump = false;
            jumpsAvailable -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
        }
        SetCountText();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision");
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpsAvailable = 2;
        }
    }
}
