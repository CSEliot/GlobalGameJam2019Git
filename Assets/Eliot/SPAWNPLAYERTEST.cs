using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNPLAYERTEST : MonoBehaviour
{
   

    public void SPAWNDUDE() {
        PhotonArenaManager.Instance.SpawnPlayer();
        this.gameObject.SetActive(false);
    }
}
