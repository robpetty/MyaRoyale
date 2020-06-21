using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pv;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.gameSetup.spawnPoints.Length);

        if (pv.IsMine == true)
        {
            // randomly spawn player
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.gameSetup.spawnPoints[spawnPicker].position, GameSetup.gameSetup.spawnPoints[spawnPicker].rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
