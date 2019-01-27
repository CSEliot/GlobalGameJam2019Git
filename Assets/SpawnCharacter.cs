using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    public Transform placePos;
    private bool isPlayerCreated;
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerCreated = false;
        PhotonArenaManager.Instance.ConnectAndJoinRoom("paul");
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
                    isPlayerCreated = true;
                    cam.SetActive(false);
                }
            }
        }
    }
}
