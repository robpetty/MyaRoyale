using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
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
        Debug.Log(pv.Owner.ActorNumber.ToString() + " is the PV actornumber of this PV, is it mine? " + pv.IsMine.ToString());

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
        if (pv != null)
        {
            Debug.Log("RPC_AddCharacter: Adding character " + whichCharacter + " for client " + pv.Owner.ActorNumber);
            PhotonPlayer.playerNameText = pv.Owner.ActorNumber.ToString();
        }
        else
        {
            pv = GetComponent<PhotonView>();
            Debug.Log("RPC_AddCharacter: Adding character " + whichCharacter + " while PV is null, unsure what impact in that case");
            PhotonPlayer.playerNameText = "pv was null";
        }

        // load the charactor model
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.pi.allCharacters[whichCharacter], transform.position, transform.rotation, this.transform);

        // set the avator name text above head
        if (pv != null)
        {
            if (pv.IsMine == true)
            {
                GameObject o = GameObject.FindWithTag("PlayerAvatar");
                Transform t = o.transform.GetChild(1);
                t.GetComponent<TextMeshPro>().text = PhotonPlayer.playerNameText;
            }
        }
    }
}
