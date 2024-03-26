using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    public float Velocity;
    public float JumpForce; // Step 1: Jump force variable
    public float GroundCheckDistance = 1f; // Distance to check if the player is grounded

    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed = 0.1f;
    public Animator anim;
    public float Speed;
    public float allowPlayerRotation = 0.1f;
    public Camera cam;
    public CharacterController controller;

    private Vector3 velocity;

    private Vector3 moveVector;
    private bool isGrounded;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        InputMagnitude();
        CheckGrounded(); // Step 4: Check if the player is grounded

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump")) // Step 2: Check for jump input and grounded condition
        {
            if(isGrounded)
            {
                Jump(); // Step 3: Trigger the jump
                anim.SetTrigger("Jump");
            }
            
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void InputMagnitude()
    {
        //Calculate Input Vectors
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        //anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
        //anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        //Calculate the Input Magnitude
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        float inputMagnitude = Mathf.Sqrt(InputX * InputX + InputZ * InputZ);
        float normalizedMagnitude = Mathf.Clamp01(inputMagnitude);

        Debug.Log("Speed: " + Speed);


        //Physically move player
        if (Speed > allowPlayerRotation)
        {
            anim.SetFloat("Speed", Speed);
            //anim.SetTrigger("Run");
            PlayerMoveAndRotation();
        }
        else if (Speed < allowPlayerRotation)
        {
            anim.SetFloat("Speed", Speed);
            //anim.ResetTrigger("Run");
        }
    }

    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
        }
    }

    void Jump()
    {
        // Apply a vertical impulse to the player's movement
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        anim.SetTrigger("Jump");
    }

    void CheckGrounded()
    {
        // Check if the player is grounded using a downward raycast
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }
}
