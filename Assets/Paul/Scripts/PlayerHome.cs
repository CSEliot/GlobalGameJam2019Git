using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public enum PersonalityType
{
    Perfectionist, Hoarder, Nerd, Tacky, Cheap, Expensive, Hermit, Athlete, SoccerMom
}


public class PlayerHome : MonoBehaviour
{

    public int homeRef;
    public PersonalityType houseType;
    public List<Room> allRooms = new List<Room>();

    public TextMeshPro myPointsTxt;
    public GameObject showHome;//turn on for each person based on their home int

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
