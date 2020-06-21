using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room details
    public static PhotonRoom room;
    private PhotonView pv;

    public bool isGameLoaded;
    public int currentScene;

    // Player details
    private Player[] players;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    // Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else if (PhotonRoom.room != this) {
            Destroy(PhotonRoom.room.gameObject);
            PhotonRoom.room = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joining room.");

        players = PhotonNetwork.PlayerList;
        playersInRoom = players.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if (MultiplayerSettings.multiPlayerSettings.delayStart == true)
        {
            Debug.Log("Playesrs in room out of max possible playesr (" + playersInRoom + ":" + MultiplayerSettings.multiPlayerSettings.maxPlayers + ")");

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }

            if (playersInRoom == MultiplayerSettings.multiPlayerSettings.maxPlayers)
            {
                readyToStart = true;

                if (PhotonNetwork.IsMasterClient != true)
                {
                    return;
                }

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log("A new player has entered the room.");
        players = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (MultiplayerSettings.multiPlayerSettings.delayStart == true)
        {
            Debug.Log("Playesrs in room out of max possible playesr (" + playersInRoom + ":" + MultiplayerSettings.multiPlayerSettings.maxPlayers + ")");

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }

            if (playersInRoom == MultiplayerSettings.multiPlayerSettings.maxPlayers)
            {
                if (PhotonNetwork.IsMasterClient == true)
                {
                    return;
                }

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MultiplayerSettings.multiPlayerSettings.delayStart == true)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
            }

            if (isGameLoaded != true)
            {
                if (readyToStart == true)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount == true)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;

                }

                Debug.Log("Display time to start to the players" + timeToStart);

                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    private void StartGame()
    {
        isGameLoaded = true;

        if (PhotonNetwork.IsMasterClient != true)
        {
            return;
        }

        if (MultiplayerSettings.multiPlayerSettings.delayStart == true)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        PhotonNetwork.LoadLevel(MultiplayerSettings.multiPlayerSettings.multiPlayerScene);
    }
    private void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if (currentScene == MultiplayerSettings.multiPlayerSettings.multiPlayerScene)
        {
            isGameLoaded = true;

            if (MultiplayerSettings.multiPlayerSettings.delayStart == true)
            {
                // tell master client is ready
                pv.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                // no delay, jump right in
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;

        // to avoid duplicates
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            // now create player on all clients
            pv.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"),transform.position, Quaternion.identity, 0);
    }
}
