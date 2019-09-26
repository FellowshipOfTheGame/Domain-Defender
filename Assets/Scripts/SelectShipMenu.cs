using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectShipMenu : MonoBehaviour
{
    public void SelectShip(string type)
    {   
        ShipType newType = ShipType.Debian;
        if(Enum.TryParse<ShipType>(type, out newType))
            GameManager.currentShipType = newType;
        else
            Debug.LogError("Invalid string. The possible values are: Arch, Debian, Ubuntu, Fedora");
    }
}
