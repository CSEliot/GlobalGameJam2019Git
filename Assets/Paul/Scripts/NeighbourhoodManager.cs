using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class NeighbourhoodManager : MonoBehaviour
{
    public List<PlayerHome> allHomes = new List<PlayerHome>();

    public Dictionary<int, PersonalityType> Personalities { get; set; }


    public void DropItemOutside(int player, Collectable collecto)
    {
        collecto.transform.parent = null;
        collecto.collido.enabled = true;
        collecto.rby.isKinematic = false;

        RemoveFromAllRooms(collecto);
    }

    public void DropItemInHouseRoom(int player, int house, int room, Collectable collecto)
    {
        if(player == house)//placing in their house evaluate for postive points
        {

        }
        else //placing in someone elses house
        {

        }

        RemoveFromAllRooms(collecto);
        allHomes[house].allRooms[room].TakeThisObject(collecto);
    }

    public void TakeItemFromHouseRoom()
    {

    }

    public void RemoveFromAllRooms(Collectable collecto)
    {
        foreach (var room in allHomes.SelectMany(h => h.allRooms))
        {
            if (room.objects.Contains(collecto))
            {
                room.objects.Remove(collecto);
            }
        }
    }


 /*   Foreach(house.rooms)

        Foreach(obj)

            Add 1
			Foreach(obj.traits.where(trait == player.type)

                Add 4
			foreach(obj.negtraits.where(trait == player.type)
				Sub 6
				
		If(objs.count(obj.affinity == room.type) == room.slots)
			Add room.points */


    public int GetHouseScore(int house)
    {
        int score = 0;

        foreach (var room in allHomes[house].allRooms)
        {
            foreach (var obj in room.objects)
            {
                score += 1;


            }


        }

        return score;
    }

}
