using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiPlayerSettings;
    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiPlayerScene; // Index of level in build


    private void Awake()
    {
        if (MultiplayerSettings.multiPlayerSettings == null)
        {
            MultiplayerSettings.multiPlayerSettings = this;
        }
        else
        {
            if (MultiplayerSettings.multiPlayerSettings != this) {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
