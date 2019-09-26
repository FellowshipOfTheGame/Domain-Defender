using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipType { Arch, Debian, Fedora, Ubuntu }

public class ShipSelect : MonoBehaviour
{
    [System.Serializable]
    struct Ship
    {
        public Sprite art;
        public Sprite shadow;
        public Color color;
    }

    [SerializeField] Ship arch, debian, fedora, ubuntu;
    [SerializeField] SpriteRenderer shipRenderer, shadowRenderer, hexCapsule;
    [SerializeField] TrailRenderer trail; 
    [SerializeField] ShipType defaultShip;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameManager.currentShipType != null)
            SelectShip((ShipType)GameManager.currentShipType);
        else
            SelectShip(defaultShip);

        GameManager.backFromGameScene = true;
    }

    void SelectShip(ShipType newType)
    {
        switch(newType)
        {
            case ShipType.Arch:
                SwapShip(arch);
                return;

            case ShipType.Debian:
                SwapShip(debian);
                return;

            case ShipType.Fedora:
                SwapShip(fedora);
                return;

            case ShipType.Ubuntu:
                SwapShip(ubuntu);
                return;

            default:
                SelectShip(defaultShip);
                return;
        }
    }

    void SwapShip(Ship newShip)
    {
        shipRenderer.sprite = newShip.art;
        shadowRenderer.sprite = newShip.shadow;
        shipRenderer.color = newShip.color;
        shadowRenderer.color = newShip.color;
        trail.startColor = newShip.color;
        hexCapsule.color = newShip.color;
    }

}
