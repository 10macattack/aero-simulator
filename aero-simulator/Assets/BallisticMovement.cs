using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Math = System.Math;

public class BallisticMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform rocket;
    public Transform target;
    
    //sets time is time until impact in seconds
    public float time = 10f;
    public bool hasLaunched = false;
    //g is gravity coefficient, assigned to 9.81 by Unity
    public float g = Physics.gravity.y;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("i") && hasLaunched == false){
            //get distance between objects
            float deltaX  = target.position.x - rocket.position.x;
            float deltaY = target.position.y - rocket.position.y;   
            float deltaZ = target.position.z - rocket.position.z;

            //calculate the velocity in the Y direction
            double Double = Math.Pow(time,2);
            float timeSquared = Convert.ToSingle(Double);
            double doubleVelocityY= (deltaY - (.5*g*(timeSquared)))/time;
            float velocityY = Convert.ToSingle(doubleVelocityY);

            //assigns velocities to x y z components
            float initYVelocity = velocityY;
            float initXVelocity = deltaX/time;
            float initZVelocity = deltaZ/time;

            //applies initial velocity
            rb.velocity = new Vector3(initXVelocity, initYVelocity, initZVelocity);

            //sets it so cannot launch multiple times in a frame until cooldown/event
            hasLaunched = true;
        }
    }    
}
