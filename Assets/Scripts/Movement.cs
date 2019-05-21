using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // The object will rotate so that it has 6 positions (360/6 = 60)
    private int rotationAngle = 60;
    [Tooltip("The minimun swipe distance to rotate the object")]
    [SerializeField] private float minSwipeDistance;
    private Vector3 clickPress, clickRelease;
    
    /// <summary>
    /// Rotates the object to left or right in "rotationAngle" degrees
    /// </summary>
    /// <param name="direction"> Rotation direction </param>
    private void Rotate(string direction)
    {
        if (direction.Equals("Left"))
            this.transform.eulerAngles = new Vector3(0f, 0f, this.transform.eulerAngles.z + rotationAngle);
        else if (direction.Equals("Right"))
            this.transform.eulerAngles = new Vector3(0f, 0f, this.transform.eulerAngles.z - rotationAngle);
    }

    /// <summary>
    /// Gets the input from keyboard or touch screen and rotates the object
    /// </summary>
    void Update()
    {
        // Keyboard Input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Rotate("Left");
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Rotate("Right");

        // Mouse Swipe
        // Gets click press position
        if (Input.GetMouseButtonDown(0))
            clickPress = Input.mousePosition;

        // Gets click release position, compare with press, and rotates if it's considered a swipe
        if (Input.GetMouseButtonUp(0))
        {
            clickRelease = Input.mousePosition;

            float swipeDistance = clickRelease.x - clickPress.x;
            // Debug.Log("Swipe:\t" + swipeDistance);
            if (swipeDistance < -minSwipeDistance)
                Rotate("Left");
            else if (swipeDistance > minSwipeDistance)
                Rotate("Right");
        }
    }
}
