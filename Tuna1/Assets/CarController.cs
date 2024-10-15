using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class S_CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }
    [Serializable]
    public struct Wheel
    {
        public GameObject WheelModel;
        public WheelCollider WheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30f;
    public float brakeAcceleration = 50f;
    public float maxSteerAngle = 30f;
    public float turnSensivity = 1.0f;
    public int score = 0;

    public List<Wheel> wheels;

    float moveInput;
    float brakeInput;
    float steerInput;

    private Rigidbody carRb;

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = new Vector3(0, -0.5f, 0);
        
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
    }

    void LateUpdate()
    {
        Move();
        Brake();
        Steer();
    }
    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetAxis("Jump");
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = moveInput * -600 * maxAcceleration * Time.deltaTime;
        }
    }
    private void Brake()
    {
        float brakeForce = brakeInput * 800 * brakeAcceleration * Time.deltaTime;
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = brakeForce;
        }
    }
    private void Steer()
    {

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensivity * maxSteerAngle;
                wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.WheelCollider.GetWorldPose(out pos, out rot);
            wheel.WheelModel.transform.position = pos;
            wheel.WheelModel.transform.rotation = rot;
        }

    }

    
        }
    
