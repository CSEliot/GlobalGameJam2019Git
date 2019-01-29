using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class NeighbourhoodManager : MonoBehaviourPunCallbacks, IPunObservable  {
    public List<PlayerHome> allHomes = new List<PlayerHome>();

    public List<Collectable> allItems = new List<Collectable>();


    public Dictionary<int, PersonalityType> Personalities { get; set; }
 
    private void Awake() {
        if(GameObject.FindGameObjectsWithTag("NeighbourhoodManager").Length > 1) {
            CBUG.SrsError("MORE THAN ONE EXISTS");
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

    }

    public void DropItemOutside(int player, Collectable collecto)
    {
        collecto.transform.parent = null;
        collecto.collido.enabled = true;
        collecto.rby.isKinematic = false;

        RemoveFromAllRooms(collecto);
    }

    public void DropItemInHouseRoom(int player, int house, int room, Collectable collecto)
    {
            bool isPositive = false;
            bool isNegative = false;
            
            for(int i = 0; i < collecto.traits.Count; i++)
            {
                if(allHomes[house].houseType == collecto.traits[i])
                {
                    isPositive = true;
                }
            }

            if (isPositive)
            {
                //+4 points
            }
            else
            {
                for (int i = 0; i < collecto.negativeTraits.Count; i++)
                {
                    if (allHomes[house].houseType == collecto.negativeTraits[i])
                    {
                        isNegative = true;
                    }
                }

                if (isNegative)
                {
                    //-2 points
                }
                else
                {
                    //+1 point
                }
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
