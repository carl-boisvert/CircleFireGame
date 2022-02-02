using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public CharacterController cc;
    public float gravity = 2.5f;

    public bool jumped;
    public float fuel, maxFuel;
    public float jetpackAcceleration = 3f;

    public float velX, velY, velZ, acceleration;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        velX = 0;
        velY = 0;
        velZ = 0;

        acceleration = 0.5f;

        maxFuel = 100f;
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        velX = velX / 2;
        velZ = velZ / 2;

        if (velY > -gravity) velY -= gravity * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.W))
        {
            velX += acceleration;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velX -= acceleration;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velZ += acceleration;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velZ -= acceleration;
        }

        if (!jumped && fuel < maxFuel)
        {
            fuel++;
        }
        
        if (jumped && Input.GetKeyDown(KeyCode.Space) && fuel > 0 )
        {
            velY += jetpackAcceleration;
            fuel-=25;
        }

        if (!jumped && Input.GetKeyDown(KeyCode.Space))
        {
            velY = acceleration*4;
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
