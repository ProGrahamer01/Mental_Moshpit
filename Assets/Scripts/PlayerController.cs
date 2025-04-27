using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
    
    public class PlayerController : MonoBehaviour
{

    // ** variable assignments **

    // rigidbody of the player
    private Rigidbody rb;
    
    //declaring the count variable for later loop
    private int count = 0;
    // movement along x and y axes
    private float movementX, movementY;

    // Speed that the player moves at (editble from the inspector) 
    public float speed = 0;

    // This holds a refrence to the UItext component
    public TextMeshProUGUI countText;

    // UI object to display winning text.
    public GameObject WinTextObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    // Get and store the rigidbody component attached to the player 
        rb = GetComponent<Rigidbody>();

    // at the start of the game returns value to 0
        count = 0;

    // updates the text each time the count variable increases 
        SetCountText();

     // Initially set the win text to be inactive. 
        WinTextObject.SetActive(false);

        
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
    
    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText()
    { 
        // Update the count text with the current count.
        countText.text = "count: " + count.ToString();

         // Check if the count has reached or exceeded the win condition.
        if (count >= 16)
        {
             // Display the win text.
            WinTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));

        }
    }

    //This function is incharge of udating the game with the above settings
    void FixedUpdate()
    {
    // Crete a 3D movement vector using the X and Y inputs.
        Vector3 movement= new Vector3(movementX, 0.0f, movementY);

    // Apply force to the Rigidbody to move the player
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            //incrementing count after collectible picked up
            count = count + 1;

            // updates the text each time the count variable increases 
            SetCountText();

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //destroy game object
            Destroy(gameObject);

            //set text to "You Lose"
            WinTextObject.gameObject.SetActive(true);
            WinTextObject.GetComponent<TextMeshProUGUI>().text = "You lose!";

        }
    }

}
