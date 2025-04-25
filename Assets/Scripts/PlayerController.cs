using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
    
    public class PlayerController : MonoBehaviour
{

    // variable assignments

    // rigidbody of the player
    private Rigidbody rb;
    
    // movement along x and y axes
    private float movementX, movementY;

    // Speed that the player moves at (editble from the inspector) 
    public float speed = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    // Get and store the rigidbody component attached to the player 
        rb = GetComponent<Rigidbody>();
        
    }

    // This function becomes active when a move input is detected/
    private void OnMove (InputValue movementValue)
    { 
    // Convert the input value into a vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

    // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    
    }

    //This function is incharge of udating the game with the above settings
    void FixedUpdate()
    {
    // Crete a 3D movement vector using the X and Y inputs.
        Vector3 movement= new Vector3(movementX, 0.0f, movementY);

    // Apply force to the Rigidbody to move the player
        rb.AddForce(movement * speed);
    }

}
