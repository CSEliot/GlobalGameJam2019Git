﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerState
{
    Normal, Holding, Stunned, Boosted
}

[System.Serializable]
public enum Location
{
    Outside, Store, LivingRoom, Kitchen, Bedroom, Bathroom
}

public class PaulMovementPlaceholder : MonoBehaviour
{
    public PlayerState playerState;
    public Location location;

    public NeighbourhoodManager neighbourhoodMan;

    public int myPlayerID;//0-8
    public int myHome;//0-8
    public int currentHome;//0-8

    public Transform camRef;
    public Rigidbody rby;
    public float speed = 1f;

    public DetectCollects detector;

    public Transform holdPos;

    public Collectable cItem;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rby.velocity = ((this.transform.forward * speed) * Input.GetAxis("Vertical")) + 
            ((this.transform.right * speed) * Input.GetAxis("Horizontal"));

        this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));


        

        if (Input.GetMouseButton(0))
        {
            Click();
        }

        if (Input.GetMouseButton(1))
        {
            RightClick();
        }

        if (Input.GetButton("Jump"))
        {
            Debug.Log("simulate get hit");
        }
    }

    void Click()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                {
                    PickupOrAttack();
                    break;
                }
            case PlayerState.Holding://click while holding
                {
                    DropOrPlace();
                    break;
                }
            case PlayerState.Stunned:
                {
                    break;
                }
            case PlayerState.Boosted:
                {
                    break;
                }
        }
    }

    void RightClick()
    {

    }

    void PickupOrAttack()
    {
        if (detector.hasNearObj)
        {
            cItem = detector.closeObj;
            cItem.transform.parent = this.transform;
            cItem.transform.position = holdPos.position;

            playerState = PlayerState.Holding;
        }
    }

    void DropOrPlace()
    {
        int roomLocation = 0;

        switch (location)
        {
            case Location.Outside:
                {

                    break;
                }
            case Location.LivingRoom:
                {
                    roomLocation = 0;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, myHome, roomLocation, cItem);
                    break;
                }
            case Location.Kitchen:
                {
                    roomLocation = 1;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, myHome, roomLocation, cItem);
                    break;
                }
            case Location.Bedroom:
                {
                    roomLocation = 2;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, myHome, roomLocation, cItem);
                    break;
                }
            case Location.Bathroom:
                {
                    roomLocation = 3;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, myHome, roomLocation, cItem);
                    break;
                }
        }

        playerState = PlayerState.Normal;
    }
}
