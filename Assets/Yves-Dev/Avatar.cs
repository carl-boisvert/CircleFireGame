using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public CharacterController cc;
    public Rigidbody rb;

    public float gravity = 2.5f;

    public bool jumped;
    public float fuel, maxFuel;
    public float jetpackAcceleration = 3f;

    public float velX, velY, velZ, acc, jumpAcc;
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

        maxFuel = 100f;
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        velX = Mathf.Lerp(velX, 0, 0.75f);
        velZ = Mathf.Lerp(velZ, 0, 0.75f);

        if (velY > -gravity) velY -= gravity * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.W))
        {
            velX += acc;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velX -= acc;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velZ += acc;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velZ -= acc;
        }

        if (!jumped && fuel < maxFuel)
        {
            fuel++;
        }
        
        if (jumped && Input.GetKey(KeyCode.LeftShift) && fuel > 0 )
        {
            velX = velX * 2;
            velZ = velZ * 2;
            velY = 0.1f;
            fuel -= 0.1f;
        }

        if (!jumped && Input.GetKeyDown(KeyCode.Space))
        {
            velY = jumpAcc;
            jumped = true;
        }

        cc.Move(new Vector3(velX,velY,velZ));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Floor")
        {
            Debug.Log("Hello");
            jumped = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        jumped = false;
    }
}
