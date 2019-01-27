using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using System.Collections.Generic;

public class PhotonArenaManager : Singleton<PhotonArenaManager>
{

    #region Spoof Server
    Dictionary<string, System.Object> DataStore = new Dictionary<string, object>();
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public enum ServerDepthLevel {
        Offline,
        InServer,
        InLobby,
        InRoom
    }

    public ServerDepthLevel currentServerUserDepth = ServerDepthLevel.Offline;

    void Awake() {
        if (SceneManager.GetActiveScene().name != "Eliot Test") {
            return;
        }

    }

    public void Connect() {
        if(SceneManager.GetActiveScene().name != "Eliot Test") {
            return;
        }
        CBUG.Do("Connecting!");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "BlueCouch";
    }

    public bool IsHost {
        get {
            if (currentServerUserDepth == ServerDepthLevel.Offline) {
                return true;
            } else {
                return PhotonNetwork.LocalPlayer.IsMasterClient;
            }
        }
    }

    /// <summary>
    /// Time in Milliseconds. Use this to sync!
    /// </summary>
    /// <returns>Accurate down to 1/15 of a second.</returns>
    public int GetClock() {
        if (currentServerUserDepth == ServerDepthLevel.Offline) {
            return DateTime.Now.TimeOfDay.Milliseconds;
        }
        else {
            return PhotonNetwork.ServerTimestamp;
        }
    }

    public Photon.Realtime.Room GetRoom() {
        if (currentServerUserDepth == ServerDepthLevel.Offline) {
            return null; //??? TODO HANDLE BETTER
        }
        else if ( currentServerUserDepth == ServerDepthLevel.InRoom) {
            return PhotonNetwork.CurrentRoom;
        } else {
            return null; //??? TODO HANDLE BETTER!
        }
    }

    public System.Object GetData(string label) {
        bool containsData = true;
        if (currentServerUserDepth == ServerDepthLevel.Offline) {
            if (DataStore.ContainsKey(label)) {
                return DataStore[label] as System.Object;
            } else {
                containsData = false;
            }
        }
        else if (currentServerUserDepth == ServerDepthLevel.InRoom) {
            ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            if (roomProps.ContainsKey(label)) {
                return roomProps[label] as System.Object;
            } else {
                containsData = false;
            }
        } else {
            CBUG.Error("GetData only available when Offline or InRoom, this was called at " + currentServerUserDepth.ToString() + ".");
        }
        if(containsData == false) {
            CBUG.Error("No data was found for " + label + ".");
        }
        return null;
    }

    public void SaveData(string label, System.Object data) {
        if (currentServerUserDepth == ServerDepthLevel.Offline) {
            DataStore.Add(label, data);
        }
        else if (currentServerUserDepth == ServerDepthLevel.InRoom) {
            ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            roomProps.Add(label, data);
        }
        else {
            CBUG.Error("SaveData only available when Offline or InRoom, this was called at " + currentServerUserDepth.ToString() + ".");
        }
    }

    #region PUN OVERRIDES
    public override void OnConnected() {
        base.OnConnected();

        CBUG.Do("Connected!");
    }
    #endregion
}
