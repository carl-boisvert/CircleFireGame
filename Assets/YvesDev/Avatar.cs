using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    CharacterController cc;
    public Transform cam;

    [Header("Gravity")]
    [SerializeField] float gravity = -5f;
    [SerializeField] private float termVelY = -20;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float airAcceleration = 1.5f;
    float velX, velY, velZ;
    Vector3 movement;
    [Range(0f, 1f)] public float turnSmoothTime = 0.1f;
    float turnSmoothVel;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 5f;
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
    [SerializeField] bool hoverUnlocked = false;
    [SerializeField] bool airBoostUnlocked=false; 
    [Range(1f, 2f)] public float airBoostSpeedMult = 1.5f;
    [SerializeField] float hoverAcceleration = 5.5f;
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

        ////// TODO

        movement = new Vector3(velX, 0, velZ).normalized;
        movement.y = velY;

        if (StateMachine == "Walk")
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
        if (StateMachine == "Jump")
        {
            if (hoverUnlocked && Input.GetKey(KeyCode.LeftShift) && fuel > 0 && StateMachine == "Jump") StartHover();
            if (airBoostUnlocked && Input.GetKey(KeyCode.Q) && fuel > airBoostCost) StartAirBoost();
        }

        else if (Input.GetKeyDown(KeyCode.Space) && (StateMachine == "Idle" || StateMachine == "Walk")) StartJump();
        else if (StateMachine == "Idle" && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) StartWalk();
        else if (StateMachine == "none") StartIdle();


        //Ongoing States
        if (StateMachine == "Idle") Idle();
        if (StateMachine == "Walk") Walk();
        if (StateMachine == "Jump") Jump();
        if (StateMachine == "Hover") Hover();
        if (StateMachine == "AirBoost") AirBoost();
    }

    private void StartIdle()
    {
        StateMachine = "Idle";
    }

    private void Idle()
    {
        velX = Mathf.Lerp(velX, 0, 0.75f);
        velZ = Mathf.Lerp(velZ, 0, 0.75f);

        FuelRecovery();
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

        FuelRecovery();

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

    private void StartHover()
    {
        StateMachine = "Hover";
    }

    private void Hover()
    {
        if (velY <= hoverAcceleration) velY += hoverAcceleration * Time.deltaTime;
        fuel -= hoverCost * Time.deltaTime;

        FuelStabilizer();

        if (Input.GetKeyUp(KeyCode.LeftShift)) StopHover();

        /*float frameAcc = airAcceleration * Time.deltaTime;
        velX += Input.GetAxis("Horizontal") * frameAcc;
        velZ += Input.GetAxis("Vertical") * frameAcc;

        CapAirVelocities();

        if (Input.GetKeyUp(KeyCode.Space) && velY > jumpCancelSpeed) velY = jumpCancelSpeed;

        if (velY < 0 && cc.isGrounded) StopJump();*/
    }

    private void StopHover()
    {
        StateMachine = "Jump";
    }

    private void StartAirBoost()
    {
        StateMachine = "AirBoost";
        //velY = jumpSpeed;
        velY = airBoostSpeed;
        fuel -= airBoostCost;
        FuelStabilizer();
        StopAirBoost();
    }

    private void AirBoost()
    {
        /*float frameAcc = airAcceleration * Time.deltaTime;
        velX += Input.GetAxis("Horizontal") * frameAcc;
        velZ += Input.GetAxis("Vertical") * frameAcc;

        CapAirVelocities();

        if (Input.GetKeyUp(KeyCode.Space) && velY > jumpCancelSpeed) velY = jumpCancelSpeed;

        if (velY < 0 && cc.isGrounded) StopJump();*/
    }

    private void StopAirBoost()
    {
        StateMachine = "Jump";
    }

    private void CapAirVelocities()
    {
        if (velX > 1) velX = 1;
        else if (velX < -1) velX = -1;

        if (velZ > 1) velZ = 1;
        else if (velZ < -1) velZ = -1;
    }

    private void FuelStabilizer()
    {
        if (fuel < 0) fuel = 0;
    }

    private void FuelRecovery()
    {
        if (fuel < maxFuel) fuel += fuelRecovery;
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
}
