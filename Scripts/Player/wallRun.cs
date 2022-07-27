using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;
    [SerializeField] private float wallRunHoldForce;
    [SerializeField] private float wallRunForwardForce;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt { get; private set; }

    private bool wallLeft = false;
    private bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;

    [Header("Footstep sound")]
    public AudioSource footstepaudio;
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, whatIsWall);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, whatIsWall);
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
                /*
                if (!footstepaudio.isPlaying)
                {

                    footstepaudio.Play();
                }
                */
                Debug.Log("wall running on the left");
            }
            else if (wallRight)
            {
                StartWallRun();
                /*
                if (!footstepaudio.isPlaying)
                {

                    footstepaudio.Play();
                }
                */
                Debug.Log("wall running on the right");
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        //Aku sero sound wallrun sini
        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            Vector3 wallMoveDirLeft = Vector3.ProjectOnPlane(orientation.forward, leftWallHit.normal);
            Vector3 wallHoldLeft = Vector3.ProjectOnPlane(-leftWallHit.normal, orientation.forward);
            rb.AddForce(wallMoveDirLeft * wallRunForwardForce);
            rb.AddForce(wallHoldLeft * wallRunHoldForce);
            if (!footstepaudio.isPlaying)
            {

                footstepaudio.Play();
            }
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            Vector3 wallMoveDirRight = Vector3.ProjectOnPlane(orientation.forward, rightWallHit.normal);
            Vector3 wallHoldRight = Vector3.ProjectOnPlane(-rightWallHit.normal, orientation.forward);
            rb.AddForce(wallMoveDirRight * wallRunForwardForce);
            rb.AddForce(wallHoldRight * wallRunHoldForce);
            if (!footstepaudio.isPlaying)
            {

                footstepaudio.Play();
            }
        }
          
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                FindObjectOfType<AudioManager>().Play("PlayerJump");
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                FindObjectOfType<AudioManager>().Play("PlayerJump");
            }
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
