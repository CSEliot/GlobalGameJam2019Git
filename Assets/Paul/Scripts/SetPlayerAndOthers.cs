using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerAndOthers : MonoBehaviour
{
    public PaulMovementPlaceholder mainPlayer;

    public List<Transform> characterObjects = new List<Transform>();

    public List<Transform> places = new List<Transform>();

    void Start()
    {
        //Invoke("SetEveryonesStart", 2f);
    }

    public void SetEveryonesStart()
    {
        int mainPlayerSet = (int)mainPlayer.myPersonality;

        characterObjects[0].position = places[mainPlayerSet].position;
        characterObjects[0].rotation = places[mainPlayerSet].rotation;

        switch (mainPlayerSet)
        {
            case 0:
                {
                    
                    break;
                }
        }
    }

}
