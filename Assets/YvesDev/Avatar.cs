using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    #region Variables

    [Header("Other GameObjects")]
    CharacterController cc;
    public Audio_AudioPlayer audioPlayer;
    public Animator animator;

    public Transform cam;

    [Header("Inputs")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode hoverKey = KeyCode.LeftShift;
    [SerializeField] KeyCode airBoostKey = KeyCode.Space;

    [Header("Gravity")]
    [SerializeField] float gravity = -5f;
    [SerializeField] private float termVelY = -20;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float airAcceleration = 3f;
    float velX, velY, velZ;
    Vector3 movement;
    [Range(0f, 1f)] public float turnSmoothTime = 0.1f;
    float turnSmoothVel;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 5f;
    [Range(0f, 1f)] public float jumpCancelSpeedMult = 0.5f;
    [Range(0f, 2f)] public float jumpSpeedMult = 1;
    float jumpCancelSpeed;
    public string StateMachine = "none";

    [Header("Fuel")]
    [SerializeField] float maxFuel = 100;
    [SerializeField] float fuelRecovery = 1;
    [SerializeField] float hoverCost = 10;
    [SerializeField] float airBoostCost = 35;
    float fuel;

    [Header("Jetpack")]
    [SerializeField] bool hoverUnlocked = false;
    [SerializeField] bool airBoostUnlocked = false;
    [Range(1f, 2f)] public float airBoostSpeedMult = 1.5f;
    [SerializeField] float hoverAcceleration = 5.5f;
    [SerializeField] float maxHoverSpeed = 1f;
    float airBoostSpeed;
    [SerializeField] ParticleSystem hoverPS;
    [SerializeField] ParticleSystem airBoostPS;
    [SerializeField] GameObject[] jetpackLights;

    [Header("Grappling Drone")]
    public LayerMask whatsIsGrappleable;
    Vector3 grappleTo;
    Collider grappleCollider;
    public GameObject drone;
    public ParticleSystem grappleEffect;
    [SerializeField] bool grappleUnlocked = false;
    [SerializeField] float grappleSpeed = 6f;
    [SerializeField] float grappleYSpeedMofidier = 5f;
    [SerializeField] float grapplingCapsuleRadius = 5f;
    public Transform GrappleMax;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float jumpVolume = 1f;
    [Range(0f, 1f)] public float landVolume = 1f;
    [Range(0f, 1f)] public float hoverVolume = 1f;
    [Range(0f, 1f)] public float airBoostVolume = 1f;
    [Range(0f, 1f)] public float grappleVolume = 1f;

    #endregion

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

        grappleEffect.gameObject.SetActive(false);
        ActivateJetpackLights(false);
        hoverPS.Stop();
        airBoostPS.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (StateMachine != "Grapple") Gravity();


        if (grappleUnlocked && StateMachine != "Grapple") CheckForGrapplePoints();

        //Movement
        RunState();

        //if (StateMachine != "Grapple") Gravity();

        movement.y = velY;

        cc.Move(movement * Time.deltaTime);
    }

    #region Utilities
    private void Gravity()
    {
        if (!cc.isGrounded)
        {

            if (velY > termVelY) velY += gravity * Time.deltaTime;
        }
        else if (velY < 0)
        {
            velY = gravity * 0.5f;
        }
    }

    private float Orientation(bool rotate)
    {
        //Angle to face
        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        if (rotate)
        {
            // Smoothened angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            //Applying rotation
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        return targetAngle;
    }

    private void HorizontalMove(bool orient, float speedMult)
    {
        movement = new Vector3(velX, 0, velZ);

        float targetAngle = Orientation(orient);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        movement = moveDir * moveSpeed * speedMult;
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
        if (fuel < maxFuel)
        {
            fuel += fuelRecovery * Time.deltaTime;
        }
        else fuel = maxFuel;
    }

    private void CheckForGrapplePoints()
    {
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position, GrappleMax.position, grapplingCapsuleRadius, whatsIsGrappleable);

        if (hitColliders.Length <= 0)
        {
            grappleCollider = null;
            grappleEffect.gameObject.SetActive(false);
            return;
        }

        float minDis = 1000f, distance;

        //Find closest
        foreach (var hitCollider in hitColliders)
        {
            Vector3 closestPoint = hitCollider.ClosestPoint(transform.position);
            distance = (transform.position - closestPoint).magnitude;

            if (distance < minDis)
            {
                minDis = distance;
                grappleCollider = hitCollider;
                grappleTo = closestPoint;
            }

            grappleEffect.transform.position = grappleTo;
            grappleEffect.gameObject.SetActive(true);
        }
    }

    private void ActivateJetpackLights(bool val)
    {
        foreach (GameObject light in jetpackLights) light.SetActive(val);
    }

    private void StopAirBoostEffect()
    {
        airBoostPS.Stop();
        ActivateJetpackLights(false);
    }

    #endregion

    #region State Machine
    //STATE MACHINES
    private void RunState()
    {
        if (Input.GetMouseButtonDown(0) && grappleUnlocked && grappleCollider) StartGrapple();
        else if (StateMachine == "Jump")
        {
            if (hoverUnlocked && Input.GetKey(hoverKey) && fuel > 0 && StateMachine == "Jump") StartHover();
            if (airBoostUnlocked && Input.GetKeyDown(airBoostKey) && fuel > airBoostCost) StartAirBoost();
        }
        else if (Input.GetKeyDown(jumpKey) && (StateMachine == "Idle" || StateMachine == "Walk")) StartJump();
        else if (StateMachine == "Idle" && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) StartWalk();
        else if (StateMachine == "none") StartIdle();


        //Ongoing States
        if (StateMachine == "Idle") Idle();
        if (StateMachine == "Walk") Walk();
        if (StateMachine == "Jump") Jump();
        if (StateMachine == "Hover") Hover();
        if (StateMachine == "AirBoost") AirBoost();
        if (StateMachine == "Grapple") Grapple();
    }

    private void StartIdle()
    {
        StateMachine = "Idle";
    }

    private void Idle()
    {
        velX = Mathf.Lerp(velX, 0, 0.75f);
        velZ = Mathf.Lerp(velZ, 0, 0.75f);

        movement = new Vector3(velX, 0, velZ);
        movement *= moveSpeed;

        FuelRecovery();
    }

    private void StopIdle()
    {
        StateMachine = "none";
    }

    private void StartWalk()
    {
        StateMachine = "Walk";
        animator.SetBool("Walking", true);
    }

    private void Walk()
    {
        // Change the Velocities
        velX = Input.GetAxis("Horizontal");
        velZ = Input.GetAxis("Vertical");

        HorizontalMove(true, 1);

        FuelRecovery();

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) StopWalk();
    }

    private void StopWalk()
    {
        StateMachine = "none";
        animator.SetBool("Walking", false);
    }

    private void StartJump()
    {
        StateMachine = "Jump";
        velY = jumpSpeed;

        audioPlayer.PlayAudioClipRandomFromRange(0, 4, jumpVolume);
        animator.SetBool("Walking", false);
        animator.SetBool("Jumping", true);

    }

    private void Jump()
    {
        /*
        float frameAcc = airAcceleration * Time.deltaTime;
        velX += Input.GetAxis("Horizontal") * frameAcc;
        velZ += Input.GetAxis("Vertical") * frameAcc;
        */
        velX = Input.GetAxis("Horizontal");
        velZ = Input.GetAxis("Vertical");

        CapAirVelocities();

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) HorizontalMove(true, jumpSpeedMult);

        if (Input.GetKeyUp(KeyCode.Space) && velY > jumpCancelSpeed) velY = jumpCancelSpeed;

        if (velY < 0 && cc.isGrounded) StopJump();

    }

    private void StopJump()
    {
        animator.SetBool("Jumping", false);
        audioPlayer.PlayAudioClipRandomFromRange(5, 9, landVolume);
        StateMachine = "none";
    }

    private void StartHover()
    {
        StateMachine = "Hover";
        audioPlayer.PlayAudioClip(10, hoverVolume);
        ActivateJetpackLights(true);
        hoverPS.Play();
    }

    private void Hover()
    {
        if (velY <= maxHoverSpeed) velY += hoverAcceleration * Time.deltaTime;
        fuel -= hoverCost * Time.deltaTime;

        float frameAcc = airAcceleration * Time.deltaTime;
        velX += Input.GetAxis("Horizontal") * frameAcc;
        velZ += Input.GetAxis("Vertical") * frameAcc;

        //CapAirVelocities();

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) HorizontalMove(true, 1);

        FuelStabilizer();

        if (Input.GetKeyUp(KeyCode.LeftShift) || fuel <= 0) StopHover();
    }

    private void StopHover()
    {
        StateMachine = "Jump";
        audioPlayer.StopAudio();

        hoverPS.Stop();
        ActivateJetpackLights(false);
    }

    private void StartAirBoost()
    {
        StateMachine = "AirBoost";
        velY = airBoostSpeed;
        fuel -= airBoostCost;
        FuelStabilizer();

        airBoostPS.Play();
        Invoke("StopAirBoostEffect", 1f);
        ActivateJetpackLights(true);

        audioPlayer.PlayAudioClipRandomFromRange(11, 16, airBoostVolume);
        
        StopAirBoost();
    }

    private void AirBoost()
    {
    }

    private void StopAirBoost()
    {
        StateMachine = "Jump";
    }

    private void StartGrapple()
    {

        movement = Vector3.zero;
        velY = 0;

        StateMachine = "Grapple";
        drone.SendMessage("StartGrapple", grappleTo);

        animator.SetBool("Walking", false);

        audioPlayer.StopAudio();
        audioPlayer.PlayAudioClip(17, grappleVolume);
        audioPlayer.PlayAudioClip(18, grappleVolume);

        hoverPS.Stop();
        ActivateJetpackLights(false);
    }

    private void Grapple()
    {
        Vector3 direction = grappleTo - transform.position;
        if (direction.magnitude > 1f)
        {
            //direction.Normalize();
            movement = direction.normalized * grappleSpeed;
            velY = direction.y * (grappleSpeed * grappleYSpeedMofidier) * Time.deltaTime;
        }
        else
        {
            movement = Vector3.zero;
            velY = 0;
        }

        if (Input.GetMouseButtonUp(0)) StopGrapple();
    }

    private void StopGrapple()
    {
        drone.SendMessage("StopGrapple");

        movement = Vector3.zero;
        velX = 0;
        velZ = 0;
        velY = 0;

        audioPlayer.StopAudio();
        audioPlayer.PlayAudioClip(19, grappleVolume);

        StateMachine = "Jump";
    }

    #endregion

    #region Get & Set

    public float GetFuel()
    {
        return fuel;
    }

    public float GetMaxFuel()
    {
        return maxFuel;
    }

    public float GetVelocityY()
    {
        return velY;
    }

    public void SetVelocityY(float newVelY)
    {
        velY = newVelY;
    }

    public void SetHoverUnlocked(bool newV)
    {
        hoverUnlocked = newV;
    }

    public void SetAirBoostUnlocked(bool newV)
    {
        airBoostUnlocked = newV;
    }

    public void SetGrappleUnlocked(bool newV)
    {
        grappleUnlocked = newV;
    }

    public bool GetGrappleUnlocked()
    {
        return grappleUnlocked;
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

    #endregion
}
