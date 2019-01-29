using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjAndPos
{
    public string resourceName;
    public Transform pos;
}

public class SpawnCharacter : MonoBehaviour
{
    public HUDManager hudMan;

    public List<ObjAndPos> objsPos;
    public List<string> allObjects;
    public List<Transform> placePoss;

    public NeighbourhoodManager nMan;
    public Transform placePos;
    private bool isPlayerCreated;
    public GameObject cam;

    public Transform compPlace;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerCreated = false;
        //PhotonArenaManager.Instance.ConnectAndJoinRoom("paul");
    }

    void Update()
    {
        if (!isPlayerCreated)
        {
            if (PhotonArenaManager.Instance.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom)
            {
                GameObject newPlayer = PhotonArenaManager.Instance.SpawnPlayer(placePos.position, placePos.rotation);

                if(newPlayer != null)
                {
                    newPlayer.GetComponent<PaulMovementPlaceholder>().neighbourhoodMan = nMan;
                    newPlayer.GetComponent<PaulMovementPlaceholder>().hudMan = hudMan;
                    hudMan.scoreText.text = "0";

                    isPlayerCreated = true;
                    cam.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && isPlayerCreated)
        {
            //PhotonArenaManager.Instance.SpawnObject("Computer", compPlace.position, compPlace.rotation);

            for(int i = 0; i < objsPos.Count; i++)
            {
                Collectable newC = PhotonArenaManager.Instance.SpawnObject(objsPos[i].resourceName, objsPos[i].pos.position, objsPos[i].pos.rotation).GetComponent<Collectable>();
                newC.myItemRef = i;
                nMan.allItems.Add(newC);
            }
        }
    }
}
