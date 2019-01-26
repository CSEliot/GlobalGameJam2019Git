using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum RoomType
{
    LivingRoom, Kitchen, Bedroom, Bathroom, Store
}


[System.Serializable]
public enum SlotType
{

}


[System.Serializable]
public class SlotSpace
{
    public bool taken;
    public Transform anchorSpot;
}


[System.Serializable]
public class Room : MonoBehaviour
{
    public List<SlotSpace> slots = new List<SlotSpace>();

    public int nextSlot = 0;

    public void TakeThisObject(Collectable collecto)
    {
        collecto.transform.parent = slots[nextSlot].anchorSpot;
        collecto.transform.localPosition = new Vector3(0, 0, 0);
        slots[nextSlot].taken = true;
        nextSlot++;
    }
}

