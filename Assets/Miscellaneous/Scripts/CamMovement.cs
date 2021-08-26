using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
Converted to C# 27-02-13 - no credit wanted.
Simple flycam I made, since I couldn't find any others made public.  
Made simple to use (drag and drop, done) for regular keyboard layout  
wasd : basic movement
shift : Makes camera accelerate
space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

public class CamMovement : MonoBehaviour
{
    private float yaw = 90.0f;
    private float pitch = 0.0f;

    private float horizontalSpeed = 2.0f;
    private float verticalSpeed = 2.0f;

    private float mainSpeed = 10000.0f; //regular speed
    private float shiftAdd = 2500.0f; //multiplied by how long shift is held.  Basically running
    private float maxShift = 10000.0f; //Maximum speed when holdin gshift
    private float totalRun = 1.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float xoffset = horizontalSpeed * -Input.GetAxis("Mouse X");
        float yoffset = verticalSpeed * Input.GetAxis("Mouse Y");

        yaw += xoffset; pitch += yoffset;

        if (pitch > 89.0f) { pitch = 89.0f; }
        if (pitch < -89.0f) { pitch = -89.0f; }

        Vector3 front;
        front.x = Mathf.Cos(yaw * Mathf.Deg2Rad) * Mathf.Cos(pitch * Mathf.Deg2Rad);
        front.y = Mathf.Sin(pitch * Mathf.Deg2Rad);
        front.z = Mathf.Sin(yaw * Mathf.Deg2Rad) * Mathf.Cos(pitch * Mathf.Deg2Rad);

        transform.forward = front.normalized;

        //Keyboard commands
        //float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.unscaledDeltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.unscaledDeltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.LeftControl))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
