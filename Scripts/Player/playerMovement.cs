using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("References")]
    public Climbing clilmbingScript;
    public WeaponBobSway weaponBobSwayScript;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    //[SerializeField] private bool useFootsteps = true;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;
    //private bool isSprinting => Input.GetKey(sprintKey);

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    [Header("Footstep sound")]
    public AudioSource footstepaudio;
    /*[Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    */

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;


    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
            FindObjectOfType<AudioManager>().Play("PlayerJump");
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        

        //Reference to weaponBobWayScript
        weaponBobSwayScript.currentSpeed = AllowWeaponSway() ? moveSpeed : 0;
    }

    //Weapon Sway
    private bool AllowWeaponSway()
    {
        if(Input.GetAxisRaw("Horizontal") != 0|| Input.GetAxisRaw("Vertical") != 0)
        {
            if (isGrounded)
            {
                return true;
            }
        }

        return false;
    }

    

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        if (moveDirection.magnitude > 0)
        {
            if (!footstepaudio.isPlaying && isGrounded)
            {
                Debug.Log("movespeed"+moveSpeed);

                footstepaudio.pitch = 1.5f;

                if (moveSpeed >8)
                {
                    footstepaudio.pitch = 1.5f;

                }
                else
                {
                    footstepaudio.pitch = 1f;

                }
                
                footstepaudio.Play();
            }
        }
        
        
    }   

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKeyDown(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }

    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        /*
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 2;
        }
        */
        

        if (clilmbingScript.exitingWall) return;

        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            /*
            if (Input.GetKeyDown("w")) 
            {
                FindObjectOfType<AudioManager>().Play("PlayerWalk");
            }

            if (Input.GetKeyDown("s"))
            {
                FindObjectOfType<AudioManager>().Play("PlayerWalk");
            }

            if (Input.GetKeyDown("a"))
            {
                FindObjectOfType<AudioManager>().Play("PlayerWalk");
            }

            if (Input.GetKeyDown("d"))
            {
                FindObjectOfType<AudioManager>().Play("PlayerWalk");
            }

            if (Input.GetKeyUp("w"))
            {
                FindObjectOfType<AudioManager>().StopPlaying("PlayerWalk");
            }

            if (Input.GetKeyUp("s"))
            {
                FindObjectOfType<AudioManager>().StopPlaying("PlayerWalk");
            }

            if (Input.GetKeyUp("a"))
            {
                FindObjectOfType<AudioManager>().StopPlaying("PlayerWalk");
            }

            if (Input.GetKeyUp("d"))
            {
                FindObjectOfType<AudioManager>().StopPlaying("PlayerWalk");
            }
            */
           
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
