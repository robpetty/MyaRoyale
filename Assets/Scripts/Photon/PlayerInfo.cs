using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo pi;
    public int mySelectedCharacter;
    public GameObject[] allCharacters;

    private void OnEnable()
    {
        if (PlayerInfo.pi == null)
        {
            PlayerInfo.pi = this;
        }
        else
        {
            if (PlayerInfo.pi != this)
            {
                Destroy(PlayerInfo.pi.gameObject);
                PlayerInfo.pi = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter") == true)
        {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
        }
    }
}