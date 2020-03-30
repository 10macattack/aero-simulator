using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Math = System.Math;

public class flight : MonoBehaviour
{
    public Mesh wings;
    public TimeManager timeManager;
    public Rigidbody rb;
    public Transform aircraft;
    public Transform target;
    float g = Physics.gravity.y;
    float pi = Convert.ToSingle(Math.PI);
    float Cl;
    float densityOfAir = 1.225f;
    float initVelocity= 185;
    //defines the maximum thrust the engine can apply
    float maxThrust=64000f;
    float rotationSpeed = 20f;

    float CalculateSurfaceArea(Mesh mesh) {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        double sum = 0.0;
        for(int i = 0; i < triangles.Length; i += 3) {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;
            sum += Vector3.Cross(a, b).magnitude;
        }
        return (float)(sum/2.0);
    }
    float getAoA(){
        //Vector3 rotations = aircraft.rotation.eulerAngles;
        Vector3 rotations = aircraft.InverseTransformDirection(rb.velocity);
        Vector3 velocities = rb.velocity;
        Debug.Log(velocities);
        float AoA = Vector3.Angle(velocities, rotations);
        return AoA;
    }
    void applyFlightForces(){
        float angleOfAttack = getAoA();
        Debug.Log(angleOfAttack);
        float weight = g * rb.mass;
        //finds the angle of attack from the degree from the ground to the rotation of the plane in the local x direction
        //float angleOfAttack = -(aircraft.rotation.eulerAngles.x - 360f);
        float currentForwardVelocity = aircraft.InverseTransformDirection(rb.velocity).z;
        float currentYVelocity = rb.velocity.y;
        angleOfAttack = angleOfAttack - .75f; 
        //finds coefficient of lift (Cl)
        if (angleOfAttack <= 12.5f || angleOfAttack>= -12.5){
            Cl = ((angleOfAttack * pi)/180) * 2 * pi;
        }
        else if(12.6f <= angleOfAttack && angleOfAttack <15f || -12.6f >= angleOfAttack && angleOfAttack > -15f){
            Cl = 15.1f/angleOfAttack;
        }
        else{
            Cl = Convert.ToSingle(Math.Pow((15.1f/angleOfAttack),5));
        }
        //finds coefficient of drag using formula .01296*1.093^AoA*.75, with .75 being a bias
        float Cd = .01296f*(Convert.ToSingle(Math.Pow(1.093,angleOfAttack)))*.75f;
        //calculates the lift and drag applied on the airplan
        float velocityZSquared = Convert.ToSingle(Math.Pow(currentForwardVelocity,2));
        float lift = Cl * SA * .5f * densityOfAir * velocityZSquared;
        float drag = Cd * SA * .5f * densityOfAir * velocityZSquared;
         if(currentForwardVelocity<=55.555f && -angleOfAttack < -2.5f){
             aircraft.Rotate((Vector3.right * rotationSpeed * Time.deltaTime));
         }
        float thrustApplied = maxThrust * .9f;
        //Debug.Log(lift);
        //Debug.Log(weight);
        //makes flight ai recognize low speed and apply thrust until it reaches a set constant velocity (118.3m/s)
        if(currentYVelocity < 0 && angleOfAttack > 0 || currentForwardVelocity<118.3){
            rb.AddForce(transform.forward * thrustApplied);
        }
        rb.AddForce(transform.up * lift); 
        if (currentForwardVelocity<0){  
        rb.AddForce(transform.forward * (drag));
        }
        else{
            rb.AddForce(transform.forward * (-drag));
        }
    }

    public float SA;
    void Start(){
        SA = CalculateSurfaceArea(wings);
        rb.velocity = transform.forward * initVelocity;
        timeManager.BulletTime();

    }
    void FixedUpdate()
    {   //sets aircraft speed and applies forces
        applyFlightForces();
    }
}
