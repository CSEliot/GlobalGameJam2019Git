﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PersonalityType
{
    Perfectionist, Hoarder, Nerd, Tacky, Cheap, Expensive, Hermit, Athletic, SoccerMom
}


public class PlayerHome : MonoBehaviour
{
    public int homeRef;
    public PersonalityType houseType;
    public List<Room> allRooms = new List<Room>();

    //public List<objItems> needs = new List<objItems>();

    private void Start()
    {
        for(int i = 0; i < allRooms.Count; i++)
        {
            allRooms[i].roomHitbox.homeRef = homeRef;
        }
    }

    public void DropItemInRoom(int roomInt, Collectable cItem)
    {
        allRooms[0].TakeThisObject(cItem);
        
    }
}
