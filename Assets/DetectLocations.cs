using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLocations : MonoBehaviour
{
    public List<Location> locations = new List<Location>();


    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Collectable":
                {
                    if (!locations.Contains(Location.LivingRoom))
                    {
                        locations.Add(Location.LivingRoom);
                    }
                    break;
                }
            case "Home":
                {
                    //nearHouse = true;
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
