using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //rotate the object on X, Y, and  Z axes by specified amounts, adjusted for frame rate.
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        
    }
}
