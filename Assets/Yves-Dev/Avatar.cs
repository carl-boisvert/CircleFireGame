using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    CharacterController cc;

    [Header("Gravity")]
    [SerializeField] float gravity = -2.5f;
    [SerializeField] private float termVelY = -5;

    [Header("Fuel")]
    [SerializeField] float maxFuel = 100;
    [SerializeField] float fuelRecovery = 1;
    [SerializeField] float hoverCost = 10;
    [SerializeField] float airBoostCost = 35;
    float fuel;

    [Header("Jetpack")]
    [Range(1f, 2f)] public float airBoostSpeedMult = 2;
    [SerializeField] float hoverAcceleration = 0.2f;
    float airBoostSpeed;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float airAcceleration = 0.5f;
    float velX, velY, velZ;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 2.5f;
    [Range(0f, 1f)] public float jumpCancelSpeedMult = 0.5f;
    float jumpCancelSpeed;
    string jumpStateMachine = "none";

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        velX = 0;
        velY = 0;
        velZ = 0;

        jumpCancelSpeed = jumpSpeed * jumpCancelSpeedMult;
        airBoostSpeed = jumpSpeed * airBoostSpeedMult;

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
            fuel += fuelRecovery;
        }

        if (!cc.isGrounded && Input.GetKey(KeyCode.LeftShift) && fuel > 0)
        {
            if (velY <= hoverAcceleration) velY += hoverAcceleration;
            fuel -= hoverCost * Time.deltaTime;
        }

        if (!cc.isGrounded && Input.GetKeyDown(KeyCode.Q) && fuel > 0)
        {
            velY = airBoostSpeed;
            fuel -= airBoostCost;
            FuelStabilizer();
        }

        cc.Move(new Vector3(velX, velY, velZ));
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

    public float GetVelocityY()
    {
        return velY;
    }

    public void SetVelocityY(float newVelY)
    {
        velY = newVelY;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void SetGravity(float newGravity)
    {
        gravity = newGravity;
    }

    public void SetJumpSpeed(float newSpeed)
    {
        jumpSpeed = newSpeed;
    }

    private void FuelStabilizer()
    {
        if (fuel < 0) fuel = 0;
    }

    //STATE MACHINES
    private void RunState()
    {

        //New State
        if (Input.GetKeyDown(KeyCode.Space) && (jumpStateMachine == "Idle" || jumpStateMachine == "Walk")) StartJump();
        else if (jumpStateMachine == "Idle" && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) StartWalk();
        else if (jumpStateMachine == "none") StartIdle();


        //Ongoing States
        if (jumpStateMachine == "Idle") Idle();
        if (jumpStateMachine == "Walk") Walk();
        if (jumpStateMachine == "Jump") Jump();
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
        velX = Input.GetAxis("Vertical") * moveSpeed;
        velZ = Input.GetAxis("Horizontal") * -moveSpeed;

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) StopWalk();
    }

    private void StopWalk()
    {
        jumpStateMachine = "none";
    }

    private void StartJump()
    {
        jumpStateMachine = "Jump";
        velY = jumpSpeed;
    }

    private void Jump()
    {
        velX += Input.GetAxis("Vertical") * airAcceleration;
        velZ += Input.GetAxis("Horizontal") * -airAcceleration;

        CapAirVelocities();

        if (Input.GetKeyUp(KeyCode.Space) && velY > jumpCancelSpeed) velY = jumpCancelSpeed;

        if (velY < 0 && cc.isGrounded) StopJump();
    }

    private void StopJump()
    {
        jumpStateMachine = "none";
    }

    private void CapAirVelocities()
    {
        if (velX > moveSpeed) velX = moveSpeed;
        else if (velX < -moveSpeed) velX = -moveSpeed;

        if (velZ > moveSpeed) velZ = moveSpeed;
        else if (velZ < -moveSpeed) velZ = -moveSpeed;
    }
}
