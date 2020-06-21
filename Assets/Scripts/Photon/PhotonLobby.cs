using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://doc.photonengine.com/en-us/pun/v2/getting-started/initial-setup
// changed base class from MonoBehaviour to MonoBehaviourPunCallbacks
public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject battleButton;
    public GameObject cancelButton;

    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // PhotonServerSettings in Assets->Photon->PhotonUnityNetworking->Resources
        // 2020-06-20 Set App Version to 0 until next client compatability break
        PhotonNetwork.ConnectUsingSettings();   // connects to master server
    }

    // callback from PUN once connected via ConnectingUsingSettings
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player connected to Photon master server.");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle button clicked.");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Tried to join random game, failed. No open game available.");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating new room.");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiPlayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed to create room, must be room with name already.");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button clicked.");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }
}