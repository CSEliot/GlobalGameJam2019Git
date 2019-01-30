using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public NeighbourhoodManager nMan;
    public Transform placePos;
    public List<Transform> spawnPlayerPos = new List<Transform>();
    private bool isPlayerCreated;
    public GameObject cam;

    public Transform compPlace;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerCreated = false;

        string[] singletons = {};
        if(SceneManager.sceneCount == 1) {
            PhotonArenaManager.Instance.ConnectAndJoinRoom("paul", singletons);
        }
    }

    void Update()
    {
        if (!isPlayerCreated)
        {
            if (PhotonArenaManager.Instance.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom)
            {

                Debug.Log("Set player pos fix");
                GameObject newPlayer = PhotonArenaManager.Instance.SpawnPlayer(spawnPlayerPos[PhotonNetwork.LocalPlayer.ActorNumber].position, spawnPlayerPos[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
                //GameObject newPlayer = PhotonArenaManager.Instance.SpawnPlayer(placePos.position, placePos.rotation);

                if(newPlayer != null)
                {
                    newPlayer.GetComponent<PaulMovementPlaceholder>().neighbourhoodMan = nMan;
                    newPlayer.GetComponent<PaulMovementPlaceholder>().hudMan = hudMan;
                    //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
                    //newPlayer.GetComponent<PaulMovementPlaceholder>().myPersonality = (PersonalityType)PhotonNetwork.LocalPlayer.ActorNumber;
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
