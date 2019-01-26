using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourhoodManager : MonoBehaviour
{
    public List<PlayerHome> allHomes = new List<PlayerHome>();


    public void DropItemOutside(int player, Collectable collecto)
    {
        collecto.transform.parent = null;
        collecto.collido.enabled = true;
        collecto.rby.isKinematic = false;
    }

    public void DropItemInHouseRoom(int player, int house, int room, Collectable collecto)
    {
        if(player == house)//placing in their house evaluate for postive points
        {

        }
        else //placing in someone elses house
        {

        }

        allHomes[house].allRooms[room].TakeThisObject(collecto);
    }

    public void TakeItemFromHouseRoom()
    {

    }
}
