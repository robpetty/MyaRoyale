using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
    private void OnGUI()
    {
        int xCenter = (Screen.width / 4);
        int yCenter = (Screen.height / 4);
        int width = 400;
        int height = 120;

        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("button"));
        fontSize.fontSize = 32;

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Multiplayer")
        {
            // Show a button to allow scene2 to be switched to.
            if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Load second scene", fontSize))
            {
                //SceneManager.LoadScene("scene2");
                MultiplayerSettings.multiPlayerSettings.multiPlayerScene++;
                PhotonNetwork.LoadLevel(MultiplayerSettings.multiPlayerSettings.multiPlayerScene);
            }
        }
        else if (scene.name == "Multiplayer2")
        {
            // Show a button to allow scene1 to be returned to.
            if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Return to first scene", fontSize))
            {
                //SceneManager.LoadScene("scene1");
                MultiplayerSettings.multiPlayerSettings.multiPlayerScene--;
                PhotonNetwork.LoadLevel(MultiplayerSettings.multiPlayerSettings.multiPlayerScene);
            }
        }
    }
}
