using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


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

    // Checks how many pickups are in the level
    private int totalPickups;

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

        // Check how many pick ups are in the level and saves it to the variable 
        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;

        // updates the text each time the count variable increases 
        SetCountText();

        // Initially set the win text to be inactive. 
        WinTextObject.SetActive(false);

        Debug.Log($"Total Pickups in Scene: {totalPickups}, Current Count: {count}");

    }

    // This function becomes active when a move input is detected/
    private void OnMove(InputValue movementValue)
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
        //instead of hard picking a number find a way to count the amount of an object  
        if (count >= totalPickups && totalPickups > 0)
        {
            // Display the win text.
            WinTextObject.SetActive(true);

            // Destroy all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

            Debug.Log("Level complete. Reloading scene...");
            //StartCoroutine(ReloadSceneAfterDelay(2f)); // 2 seconds delay before reload
            StartCoroutine(AdvanceToNextScene(2f)); // 2 seconds delay before reload

        }
    }


    //This function is incharge of udating the game with the above settings
    void FixedUpdate()
    {
        // Crete a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

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

        if (other.gameObject.CompareTag("PowerUps"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            PowerUpManager manager = FindAnyObjectByType<PowerUpManager>();

            if (manager != null)
            {
                manager.TriggerPowerUp();
            }
            else
            {
                Debug.LogWarning("PowerUpManager not found in scene!");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //destroy game object
            Destroy(gameObject);

            //Stops camera from following the player into death
            Camera.main.GetComponent<CameraController>().player = null;

            //set text to "You Lose"
            WinTextObject.gameObject.SetActive(true);
            WinTextObject.GetComponent<TextMeshProUGUI>().text = "You lose!";

            Debug.Log("Level Failed. Reloading scene...");
            StartCoroutine(ReloadSceneAfterDelay(2f)); // 2 seconds delay before reload

        }
    }
    //Coroutines

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    IEnumerator AdvanceToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int totalScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("Last scene reached. Reloading current scene.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
        }
    }

}
