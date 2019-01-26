using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CurrentPlace
{
    Store, Held, Outside, InHouse
}

public class Collectable : MonoBehaviour
{
    public List<PersonalityType> traits = new List<PersonalityType>();

    public Vector3 localPos; //for parenting to character 
    public Vector3 localErot;

    public CurrentPlace currentPlace;
    public int heldPlayer; //0-8
    public int inHouse; //0-8
}
