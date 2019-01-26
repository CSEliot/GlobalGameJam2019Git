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
                        playerRef.locationTxt.text = "My LivingRoom";
                        locations.Add(Location.LivingRoom);
                        playerRef.location = Location.LivingRoom;
                        playerRef.currentHome = col.GetComponent<RoomHitbox>().homeRef;
                    }
                    break;
                }
            case "Kitchen":
                {
                    if (!locations.Contains(Location.Kitchen))
                    {
                        playerRef.locationTxt.text = "My Kitchen";
                        locations.Add(Location.Kitchen);
                        playerRef.location = Location.Kitchen;
                        playerRef.currentHome = col.GetComponent<RoomHitbox>().homeRef;
                    }
                    break;
                }
            case "Bedroom":
                {
                    if (!locations.Contains(Location.Bedroom))
                    {
                        playerRef.locationTxt.text = "My Bedroom";
                        locations.Add(Location.Bedroom);
                        playerRef.location = Location.Bedroom;
                        playerRef.currentHome = col.GetComponent<RoomHitbox>().homeRef;
                    }
                    break;
                }
            case "Bathroom":
                {
                    if (!locations.Contains(Location.Bathroom))
                    {
                        playerRef.locationTxt.text = "My Bathroom";
                        locations.Add(Location.Bathroom);
                        playerRef.location = Location.Bathroom;
                        playerRef.currentHome = col.GetComponent<RoomHitbox>().homeRef;
                    }
                    break;
                }
            case "Store":
                {
                    if (!locations.Contains(Location.Store))
                    {
                        playerRef.locationTxt.text = "Store";
                        locations.Add(Location.Store);
                        playerRef.location = Location.Store;
                        playerRef.currentHome = col.GetComponent<RoomHitbox>().homeRef;
                    }
                    break;
                }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        switch (col.tag)
        {
            case "LivingRoom":
                {
                    if (locations.Contains(Location.LivingRoom))
                    {
                        locations.Remove(Location.LivingRoom);
                        if(locations.Count == 0)
                        {
                            playerRef.location = Location.Outside;
                            playerRef.locationTxt.text = "Outside";
                        }
                        else
                        {
                            playerRef.location = locations[0];
                            playerRef.locationTxt.text = "My " + locations[0].ToString();
                        }
                    }
                    break;
                }
            case "Kitchen":
                {
                    if (locations.Contains(Location.Kitchen))
                    {
                        locations.Remove(Location.Kitchen);
                        if (locations.Count == 0)
                        {
                            playerRef.location = Location.Outside;
                            playerRef.locationTxt.text = "Outside";
                        }
                        else
                        {
                            playerRef.location = locations[0];
                            playerRef.locationTxt.text = "My " + locations[0].ToString();
                        }
                    }
                    break;
                }
            case "Bedroom":
                {
                    if (locations.Contains(Location.Bedroom))
                    {
                        locations.Remove(Location.Bedroom);
                        if (locations.Count == 0)
                        {
                            playerRef.location = Location.Outside;
                            playerRef.locationTxt.text = "Outside";
                        }
                        else
                        {
                            playerRef.location = locations[0];
                            playerRef.locationTxt.text = "My " + locations[0].ToString();
                        }
                    }
                    break;
                }
            case "Bathroom":
                {
                    if (locations.Contains(Location.Bathroom))
                    {
                        locations.Remove(Location.Bathroom);
                        if (locations.Count == 0)
                        {
                            playerRef.location = Location.Outside;
                            playerRef.locationTxt.text = "Outside";
                        }
                        else
                        {
                            playerRef.location = locations[0];
                            playerRef.locationTxt.text = "My " + locations[0].ToString();
                        }
                    }
                    break;
                }
            case "Store":
                {
                    if (locations.Contains(Location.Store))
                    {
                        locations.Remove(Location.Store);
                        if (locations.Count == 0)
                        {
                            playerRef.location = Location.Outside;
                            playerRef.locationTxt.text = "Outside";
                        }
                        else
                        {
                            playerRef.location = locations[0];
                            playerRef.locationTxt.text = "My " + locations[0].ToString();
                        }
                    }
                    break;
                }
        }

    }
}
