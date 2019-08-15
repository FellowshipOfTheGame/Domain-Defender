using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Tooltip("The minimun swipe distance to rotate the object")]
    [SerializeField] private float minSwipeDistance;
    [Tooltip("Rotation speed of the object")]
    [SerializeField] private float speed;

    [Tooltip("Tolerance angle to enable input for rotation")]
    [SerializeField] private float inputToleranceAngle;
    // The object will rotate so that it has 6 positions (360/6 = 60)
    private int rotationAngle = 60;
    private float newAngle;
    private Vector3 clickPress, clickRelease;
    private bool rotating = false;
    private bool canRotate = true;

    private int lane = 0;

    public int Lane
    {
        get { return lane; }
        private set
        {
            if (value == 6)
                lane = 0;
            else if (value == -1)
                lane = 5;
            else
                lane = value;
        }
    }

    /// <summary>
    /// Sets new angle to the current angle
    /// </summary>
    private void Start()
    {
        newAngle = this.transform.eulerAngles.z;
    }
    

    /// <summary>
    /// Calculates the angle the object will have after the rotation, based on an angle passed as parameter,
    /// sets flag to enable rotation on FixedUpdate, and disables input
    /// </summary>
    /// <param name="angle"></param>
    private void Rotate(float angle)
    {
        canRotate = false;
        
        // Calculates the new angle, keeping it at values multiples of 60, and less than 360
        newAngle = (newAngle + angle) % 360f;

        if (angle < 0)
            Lane--;
        else
            Lane++;

        rotating = true;
    }

    /// <summary>
    /// Gets the input from keyboard and set flags to rotate it
    /// </summary>
    private void Update()
    {
        if (canRotate)
        {
            // Keyboard Input
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Rotate(rotationAngle);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                Rotate(-rotationAngle);
            
            // Mouse Swipe
            // Gets click press position
            if (Input.GetMouseButtonDown(0))
            {
                // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
                // clickPress = Camera.main.ScreenToWorldPoint(mousePosition);
                clickPress = Input.mousePosition;
            }

            // Gets click release position, compare with press, and rotates if it's considered a swipe.
            if (Input.GetMouseButtonUp(0))
            {
                // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
                // clickRelease = Camera.main.ScreenToWorldPoint(mousePosition);
                clickRelease = Input.mousePosition;

                float swipeDistance = (clickRelease.x - clickPress.x) * 50;
                // Debug.Log(swipeDistance);

                if (swipeDistance < -minSwipeDistance)
                    Rotate(-rotationAngle);
                else if (swipeDistance > minSwipeDistance)
                    Rotate(rotationAngle);
            }
        }
    }

    /// <summary>
    /// Rotates the object
    /// </summary>
    private void FixedUpdate()
    {
        if (rotating)
        {
            // Calculates the next angle using lerp
            float angle = Mathf.LerpAngle(this.transform.eulerAngles.z, newAngle, speed);
            this.transform.eulerAngles = new Vector3(0f, 0f, angle);

            // Calculates the angle remaining to reach the new angle. 
            float remainingAngle = Mathf.Abs(this.transform.eulerAngles.z - newAngle) % 360f;

            // If the angle is higher than 180, it means the reamining angle is incresing until it reaches 360, so a correction is needed.
            if (remainingAngle > 180)
                remainingAngle = 360f - remainingAngle;

            // If the remaining angle is too little, enables Input to make controls feel better
            if (remainingAngle < inputToleranceAngle)
                canRotate = true;

            // Checks if the angle is close enough. If so, stops the rotation
            if (remainingAngle < 0.01f)
                rotating = false;
        }
    }
}
