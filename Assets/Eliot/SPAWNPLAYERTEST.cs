using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNPLAYERTEST : MonoBehaviour
{
   

    public void SPAWNDUDE() {
       //GameObject holder = PhotonArenaManager.Instance.SpawnPlayer();
        this.gameObject.SetActive(false);
    }

    public void spawnobject () {
        PhotonArenaManager.Instance.SpawnObject("TestOBJ");
    }
}
