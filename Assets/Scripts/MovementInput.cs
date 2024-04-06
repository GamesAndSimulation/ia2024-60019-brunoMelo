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
    public float InputV;
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
    private bool IsGrounded;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    private PlayerHealth playerHealthScript;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip footstepSound;

    public Transform[] shortcuts;
    private Transform player;


    void Start()
    {
        player = GameObject.Find("Main Player").transform;
        anim = GetComponent<Animator>();
        Camera cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        controller = GetComponent<CharacterController>();
        playerHealthScript = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        

        InputMagnitude();
        CheckGrounded(); // Step 4: Check if the player is grounded

        if (IsGrounded)
        {
            if(velocity.y < 0)
            {
                velocity.y = -2f;
            }
            
            anim.SetBool("IsGrounded", true);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);

            if (Input.GetButtonDown("Jump")) // Step 2: Check for jump input and grounded condition
            {
            
                Jump(); // Step 3: Trigger the jump
                anim.SetBool("IsJumping", true);
                SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f);
            }
        }

        else
        {
            anim.SetBool("IsGrounded", false);
            if(velocity.y < -2)
            {
                anim.SetBool("IsFalling", true);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Player position before: " + player.position);
            player.position = shortcuts[0].position;
            Debug.Log("Position 0");
            Debug.Log("Player position: " + player.position);
            Debug.Log("First Checkpoint position: " + shortcuts[0].position);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.position = shortcuts[1].position;
            Debug.Log("Position 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.position = shortcuts[2].position;
            Debug.Log("Position 2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.position = shortcuts[3].position;
            Debug.Log("Position 3");
        }
    }

    void InputMagnitude()
    {
        //Calculate Input Vectors
        InputX = Input.GetAxis("Horizontal");
        InputV = Input.GetAxis("Vertical");

        if(playerHealthScript.IsDead())
        {
            InputX = 0;
            InputV = 0;
        }

        anim.SetFloat("hzInput", InputX);
        anim.SetFloat("vInput", InputV);

        //anim.SetFloat ("InputV", InputV, VerticalAnimTime, Time.deltaTime * 2f);
        //anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        //Calculate the Input Magnitude
        Speed = new Vector2(InputX, InputV).sqrMagnitude;

        float inputMagnitude = Mathf.Sqrt(InputX * InputX + InputV * InputV);
        float normalizedMagnitude = Mathf.Clamp01(inputMagnitude);


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
        InputV = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //desiredMoveDirection = transform.right * InputX + transform.forward * InputV;

        desiredMoveDirection = forward * InputV + right * InputX;

        //if (blockRotationPlayer == false)
        //{
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
        //}
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
        IsGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }

    public void PlayFootstepSounds()
    {
        SoundFXManager.instance.PlaySoundFXClip(footstepSound, transform, 0.2f);
    }
}
