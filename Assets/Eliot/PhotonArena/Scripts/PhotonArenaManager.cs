using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using System.Collections.Generic;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PhotonArenaManager : Singleton<PhotonArenaManager>
{

    #region Spoof Server
    public struct FakeServer {
        public Dictionary<string, System.Object> DataStore;
        public int totalPlayers;

        public string Username { get; internal set; }
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

    public void ConnectAndJoinRoom(string username) {
        CBUG.Do("Connecting!");
        PhotonNetwork.GameVersion = "BlueCouch";

        PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = username + "_"+UnityEngine.Random.Range(0, 9999);

        string region = "us";
        bool _result = PhotonNetwork.ConnectToRegion(region);

        CBUG.Log("PunCockpit:ConnectToRegion(" + region + ") -> " + _result);
    }

    public void ConnectAndJoinOffline(string username) {
        _fakeServer.Username = username;
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

    public string GetLocalUsername() {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            return _fakeServer.Username;
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            return PhotonNetwork.LocalPlayer.UserId;
        }
        else {
            CBUG.Error("Username only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
            return null;
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


    public override void OnJoinedLobby() {
        CBUG.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnJoinedRoom() {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running.");

        //if (this.PrefabsToInstantiate != null) {
        //    foreach (GameObject o in this.PrefabsToInstantiate) {
        //        Debug.Log("Instantiating: " + o.name);

        //        Vector3 spawnPos = Vector3.up;
        //        if (this.SpawnPosition != null) {
        //            spawnPos = this.SpawnPosition.position;
        //        }

        //        Vector3 random = Random.insideUnitSphere;
        //        random.y = 0;
        //        random = random.normalized;
        //        Vector3 itempos = spawnPos + this.PositionOffset * random;

        //        PhotonNetwork.Instantiate(o.name, itempos, Quaternion.identity, 0);
        //    }
        //}
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        CBUG.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 100 }, null);
    }
    #endregion
}
