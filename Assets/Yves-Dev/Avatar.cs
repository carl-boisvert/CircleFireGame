using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public CharacterController cc;
    public Rigidbody rb;

    public float gravity = -2.5f;

    public bool isJumping;
    public float fuel, maxFuel;
    public float jetpackAcceleration = 3f;
    public float hoverSpeed = 0.1f;

    public float velX, velY, velZ, acc;
    private float termVelY = -5;
    public float jumpAcc, jumpCancelAcc;

    private string jumpStateMachine = "none";

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        velX = 0;
        velY = 0;
        velZ = 0;

        acc = 0.5f;
        jumpAcc = 2;
        jumpCancelAcc = 1;

        maxFuel = 100f;
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();

        //Horizontal Movement
        RunState();

        //Jetpack
        if (cc.isGrounded && fuel < maxFuel)
        {
            fuel++;
        }
        
        if (!cc.isGrounded && Input.GetKey(KeyCode.LeftShift) && fuel > 0 )
        {
            velX = velX * 1.5f;
            velZ = velZ * 1.5f;
            velY = hoverSpeed;
            fuel -= 0.5f;
        }
        
        if (!cc.isGrounded && Input.GetKeyDown(KeyCode.Q) && fuel > 0 )
        {
            velY = jetpackAcceleration;
            fuel -= 25f;
        }

        cc.Move(new Vector3(velX,velY,velZ));
    }

    private void LateUpdate()
    {
        
    }

    private void Gravity()
    {
        if (!cc.isGrounded)
        {
            if (velY > termVelY) velY += gravity * Time.deltaTime;
        }
        else velY = termVelY;
    }

    //STATE MACHINES

    private void RunState()
    {

        //New State
        if (Input.GetKeyDown(KeyCode.Space) && (jumpStateMachine == "Idle" || jumpStateMachine == "Walk")) StartJump();
        else if (jumpStateMachine == "Idle" && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) StartWalk();
        else if(jumpStateMachine == "none") StartIdle();

        
        //Ongoing States
        if(jumpStateMachine == "Idle") Idle();
        if(jumpStateMachine == "Walk") Walk();
        if(jumpStateMachine == "Jump") Jump();
    }

    private void StartIdle()
    {
        jumpStateMachine = "Idle";
    }

    private void Idle()
    {
        velX = Mathf.Lerp(velX, 0, 0.75f);
        velZ = Mathf.Lerp(velZ, 0, 0.75f);
    }

    private void StopIdle()
    {
        jumpStateMachine = "none";
    }
    
    private void StartWalk()
    {
        jumpStateMachine = "Walk";
    }

    private void Walk()
    {
        // Change the Velocities
        velX = Input.GetAxis("Vertical") * acc;
        velZ = Input.GetAxis("Horizontal") * -acc;

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) StopWalk();
    }

    private void StopWalk()
    {
        jumpStateMachine = "none";
    }
    
    private void StartJump()
    {
        jumpStateMachine = "Jump";
        velY = jumpAcc;
    }

    private void Jump()
    {
        if (Input.GetKeyUp(KeyCode.Space) && velY > jumpCancelAcc) velY = jumpCancelAcc;

        if (velY < 0 && cc.isGrounded) StopJump();
    }

    private void StopJump()
    {
        jumpStateMachine = "none";
    }
}
