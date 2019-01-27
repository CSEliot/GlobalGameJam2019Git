using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CBUG.Do("Connecting ...");
        PhotonArenaManager.Instance.ConnectAndJoinRoomSingle();
        CBUG.Do("Connecting .......");
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonArenaManager.Instance.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom) {

        }
    }
}
