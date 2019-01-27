using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class NeighbourhoodManager : MonoBehaviour
{
    public List<PlayerHome> allHomes = new List<PlayerHome>();


    public void DropItemOutside(int player, Collectable collecto)
    {
        collecto.transform.parent = null;
        collecto.collido.enabled = true;
        collecto.rby.isKinematic = false;

        foreach (var room in allHomes.SelectMany(h => h.allRooms))
        {
            if (room.objects.Contains(collecto))
            {
                room.objects.Remove(collecto);
            }
        }
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
