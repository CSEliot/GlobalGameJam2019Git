using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using System.Collections.Generic;

public class PhotonArenaManager : Singleton<PhotonArenaManager>
{

    #region Spoof Server
    public struct FakeServer {
        public Dictionary<string, System.Object> DataStore;
        public int totalPlayers;
    }
    FakeServer _fakeServer;
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

    public ServerDepthLevel CurrentServerUserDepth = ServerDepthLevel.Offline;

    void Awake() {
        _fakeServer.DataStore = new Dictionary<string, object>();
        _fakeServer.totalPlayers = 0;
    }

    public void ConnectAndJoinRoomSingle() {
        if(SceneManager.GetActiveScene().name != "Eliot Test") {
            return;
        }
        CBUG.Do("Connecting!");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "BlueCouch";
    }

    public bool IsHost {
        get {
            if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
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
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            //            return DateTime.Now.TimeOfDay.Milliseconds;
            return Convert.ToInt32((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) & int.MaxValue);
        }
        else {
            return PhotonNetwork.ServerTimestamp;
        }
    }

    public Photon.Realtime.Room GetRoom() {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            return null; //??? TODO HANDLE BETTER
        }
        else if ( CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            return PhotonNetwork.CurrentRoom;
        } else {
            return null; //??? TODO HANDLE BETTER!
        }
    }

    public System.Object GetData(string label) {
        bool containsData = true;
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            if (_fakeServer.DataStore.ContainsKey(label)) {
                return _fakeServer.DataStore[label] as System.Object;
            } else {
                containsData = false;
            }
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            if (roomProps.ContainsKey(label)) {
                return roomProps[label] as System.Object;
            } else {
                containsData = false;
            }
        } else {
            CBUG.Error("GetData only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
        }
        if(containsData == false) {
            CBUG.Error("No data was found for " + label + ".");
        }
        return null;
    }

    public void SaveData(string label, System.Object data) {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            _fakeServer.DataStore.Add(label, data);
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            roomProps.Add(label, data);
        }
        else {
            CBUG.Error("SaveData only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
        }
    }

    public int SpawnPlayer(string ResourceName="PhotonArenaPlayer") {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            _fakeServer.totalPlayers++;
            return _fakeServer.totalPlayers;
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            //ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            //roomProps.Add(label, data);
        }
        else {
            //CBUG.Error("SaveData only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
        }
        return -1;
    }

    public bool IsLocalClient(PhotonView playerView) {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            return true;
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            return playerView.IsMine;
        } else {
            CBUG.Error("This can only be called when player is in a room or offline! You're currently in: " + CurrentServerUserDepth.ToString());
            return false;
        }
    }

    public int GetLocalPlayerID() {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            return 1;
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            return PhotonNetwork.LocalPlayer.ActorNumber; //??? unchanging? Unique? Todo;
        }
        else {
            CBUG.Error("This can only be called when player is in a room or offline! You're currently in: " + CurrentServerUserDepth.ToString());
            return PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }

    #region PUN OVERRIDES
    public override void OnConnected() {
        base.OnConnected();

        CBUG.Do("Connected!");
    }
    #endregion
}
