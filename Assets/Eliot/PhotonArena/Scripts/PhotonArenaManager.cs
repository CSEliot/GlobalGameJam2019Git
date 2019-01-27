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

    public static Vector3 DefaultSpawnLocation = new Vector3(0f, 15f, 0f);

    public ServerDepthLevel CurrentServerUserDepth = ServerDepthLevel.Offline;
    public static class Constants {
        public static readonly Vector3 DefaultSpawnLoc = Vector3.one;
    }

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

    public ServerDepthLevel GetCurrentDepthLevel() {
        return CurrentServerUserDepth;
    }

    public Photon.Realtime.Room GetRoom() {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            return null; //??? TODO HANDLE BETTER
        }
        else if ( CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            return PhotonNetwork.CurrentRoom;
        } else {
            CBUG.Error("We are not currently in a room!");
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

    public void SpawnObject(string resourceName) {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            GameObject instance = Instantiate(Resources.Load(resourceName, typeof(GameObject)), DefaultSpawnLocation, Quaternion.Euler(Vector3.zero)) as GameObject;
            /// ??? todo make playerlist local ref 
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            GameObject PlayerObj = PhotonNetwork.Instantiate(resourceName, DefaultSpawnLocation, Quaternion.Euler(Vector3.zero));
        }
        else {
            CBUG.Error("SpawnObject only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
        }
    }
    public void SpawnObject(string resourceName, Vector3 spawnLoc, Quaternion spawnRot) {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            GameObject instance = Instantiate(Resources.Load(resourceName, typeof(GameObject)), spawnLoc, spawnRot) as GameObject;
            /// ??? todo make playerlist local ref 
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            GameObject PlayerObj = PhotonNetwork.Instantiate(resourceName, spawnLoc, spawnRot);
        }
        else {
            CBUG.Error("SpawnObject only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
        }
    }

    public string GetLocalUsername() {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            return _fakeServer.Username;
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            string username = PhotonNetwork.LocalPlayer.UserId;
            username = username.Substring(0, username.IndexOf('_'));
            return username;
        }
        else {
            CBUG.Error("Username only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
            return null;
        }
    }

    public GameObject SpawnPlayer(Vector3 pos, Quaternion rot, string ResourceName="PhotonArenaPlayer") {
        if (CurrentServerUserDepth == ServerDepthLevel.Offline) {
            _fakeServer.totalPlayers++;
            //spawn player? ???todo
            return null;
        }
        else if (CurrentServerUserDepth == ServerDepthLevel.InRoom) {
            //ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            //roomProps.Add(label, data);
           return PhotonNetwork.Instantiate("PhotonArenaPlayer", pos, rot);
        }
        else {
            CBUG.Error("Spawn Player only available when Offline or InRoom, this was called at " + CurrentServerUserDepth.ToString() + ".");
        }
        return null;
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
        CurrentServerUserDepth = ServerDepthLevel.InServer;
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();

        CBUG.Do("Connected To Master! Joining Lobby ...");
        bool success = PhotonNetwork.JoinLobby();

        if (!success) {
            CBUG.Do("PunCockpit: Could not join Lobby ...");
        }
    }

    public override void OnJoinedLobby() {
        CBUG.Log("Lobby Joined!");
        CBUG.Log("Joining Random Room ...");
        PhotonNetwork.JoinRandomRoom();
        CurrentServerUserDepth = ServerDepthLevel.InLobby;
    }

    public override void OnDisconnected(DisconnectCause cause) {
        CBUG.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnJoinedRoom() {
        CBUG.Log("Joined Room!");

        CurrentServerUserDepth = ServerDepthLevel.InRoom;
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
        CBUG.Log("Room Join failed. Creating a room ...");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 100 }, null);
    }
    #endregion
}
