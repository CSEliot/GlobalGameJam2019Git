using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLocations : MonoBehaviour
{
    public PaulMovementPlaceholder playerRef;

    public List<Location> locations = new List<Location>();


    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "LivingRoom":
                {
                    if (!locations.Contains(Location.LivingRoom))
                    {
                        locations.Add(Location.LivingRoom);
                        playerRef.location = Location.LivingRoom;
                        playerRef.currentHome = col.GetComponent<RoomHitbox>().homeRef;
                    }
                    break;
                }
            case "Kitchen":
                {
                    break;
                }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        switch (col.tag)
        {
            case "Collectable":
                {
                    //closeObj = null;
                    //hasNearObj = false;
                    break;
                }
            case "Home":
                {
                    // nearHouse = false;
                    break;
                }
        }

    }
}
