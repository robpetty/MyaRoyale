using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView pv;
    public int characterValue;
    public GameObject myCharacter;

    public Camera myCamera;
    public AudioListener myAL;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        if(pv.IsMine == true)
        {
            // AllBuffered, allows people who join later to get this so local character shows up to new players clients
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.pi.mySelectedCharacter);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAL);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.pi.allCharacters[whichCharacter], transform.position, transform.rotation, this.transform);
    }
}
