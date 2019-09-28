using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerArea : MonoBehaviour
{
    [Tooltip("Referência ao script de movimentação do player")]
    [SerializeField] Movement player;
    [SerializeField] GameObject warningLeft, warningRight, warningBack;

    [SerializeField] AudioSource DangerSource;

    private bool[] lanesWarnings = new bool[6];
    private bool warnLeft, warnRight, warnBack;

    /// <summary>
    /// Disables all warnings
    /// </summary>
    private void Start()
    {
        warningLeft.SetActive(false);
        warningRight.SetActive(false);
        warningBack.SetActive(false);
    }

    /// <summary>
    /// Retruns the number turns needed for lane1 to reach lane2
    /// </summary>
    /// <param name="lane1"></param>
    /// <param name="lane2"></param>
    /// <returns>Number of turns needed</returns>
    private int DistanceBetweenLanes(int lane1, int lane2)
    {
        int diff = lane1 - lane2;
        if (Mathf.Abs(diff) > 3)
        {
            if (diff < 0)
                return 6  + diff;
            else 
                return -(6 - diff);
        }
        else
            return diff;
    }

    /// <summary>
    /// Resets the bools that indicates which warning will be shown
    /// </summary>
    private void ResetBools()
    {
        warnBack = false; 
        warnLeft = false;
        warnRight = false;
    }

    /// <summary>
    /// Activates or deactivates the warning objects according to the indicators
    /// </summary>
    private void UpdateWarnings()
    {
        if (warnBack)
            warningBack.SetActive(true);
        else 
            warningBack.SetActive(false);

        if (warnLeft)
            warningLeft.SetActive(true);
        else 
            warningLeft.SetActive(false);

        if (warnRight)
            warningRight.SetActive(true);
        else 
            warningRight.SetActive(false);

        if (warnBack || warnLeft || warnRight)
            DangerSource.enabled = true;
        else
            DangerSource.enabled = false;
    }

    /// <summary>
    /// Keeps checking the player position to update the warnings
    /// </summary>
    private void Update()
    {   
        ResetBools();

        // Checks for each line if it is dangerous. If so displays warnings in the better position        
        int difference = 0;
        for (int i = 0; i < 6; i++)
        {
            if (player.Lane == i)
                lanesWarnings[i] = false;
            else if (lanesWarnings[i] == true) 
            {
                difference = DistanceBetweenLanes(player.Lane, i);

                // If it is the opposite lane
                if (Mathf.Abs(difference) == 3)
                    warnBack = true;
                else if (difference > 0)
                    warnRight = true;
                else if (difference < 0)
                    warnLeft = true;
            }
        }
        // if (difference != 0)
        //     Debug.Log(difference);

        UpdateWarnings();
    }

    /// <summary>
    /// When an enemy comes nearby, marks its lane as dangerous
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            int lane = other.GetComponent<Enemy>().Lane;
            lanesWarnings[lane] = true;
        }
    }
}
