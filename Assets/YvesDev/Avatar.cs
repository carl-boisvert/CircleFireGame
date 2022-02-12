using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    CharacterController cc;
    public Transform cam;

    [Header("Gravity")]
    [SerializeField] float gravity = -2.5f;
    [SerializeField] private float termVelY = -5;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float airAcceleration = 0.5f;
    float velX, velY, velZ;
    Vector3 movement;
    float turnSmoothTime = 0.1f;
    float turnSmoothVel;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 2.5f;
    [Range(0f, 1f)] public float jumpCancelSpeedMult = 0.5f;
    float jumpCancelSpeed;
    string StateMachine = "none";

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

        //Movement
        RunState();

        movement = new Vector3(velX, 0, velZ).normalized;
        movement.y = velY;

        if(StateMachine == "Walk")
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDir * moveSpeed * Time.deltaTime);
        }
        else
        {
            cc.Move(movement * moveSpeed * Time.deltaTime);
        }

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
        if (Input.GetKeyDown(KeyCode.Space) && (StateMachine == "Idle" || StateMachine == "Walk")) StartJump();
        else if (StateMachine == "Idle" && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) StartWalk();
        else if (StateMachine == "none") StartIdle();


        //Ongoing States
        if (StateMachine == "Idle") Idle();
        if (StateMachine == "Walk") Walk();
        if (StateMachine == "Jump") Jump();
    }

    private void StartIdle()
    {
        StateMachine = "Idle";
    }

    private void Idle()
    {
        velX = Mathf.Lerp(velX, 0, 0.75f);
        velZ = Mathf.Lerp(velZ, 0, 0.75f);
    }

    private void StopIdle()
    {
        StateMachine = "none";
    }

    private void StartWalk()
    {
        StateMachine = "Walk";
    }

    private void Walk()
    {
        // Change the Velocities
        velX = Input.GetAxis("Horizontal");
        velZ = Input.GetAxis("Vertical");

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) StopWalk();
    }

    private void StopWalk()
    {
        StateMachine = "none";
    }

    private void StartJump()
    {
        StateMachine = "Jump";
        velY = jumpSpeed;
    }

    private void Jump()
    {
        float frameAcc = airAcceleration * Time.deltaTime;
        velX += Input.GetAxis("Horizontal") * frameAcc;
        velZ += Input.GetAxis("Vertical") * frameAcc;

        CapAirVelocities();

        if (Input.GetKeyUp(KeyCode.Space) && velY > jumpCancelSpeed) velY = jumpCancelSpeed;

        if (velY < 0 && cc.isGrounded) StopJump();
    }

    private void StopJump()
    {
        StateMachine = "none";
    }

    private void CapAirVelocities()
    {
        if (velX > 1) velX = 1;
        else if (velX < -1) velX = -1;

        if (velZ > 1) velZ = 1;
        else if (velZ < -1) velZ = -1;
    }
}
