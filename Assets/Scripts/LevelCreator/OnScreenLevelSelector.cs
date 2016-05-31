using UnityEngine;
using System.Collections;

public class OnScreenLevelSelector : MonoBehaviour {

    bool isDebugON = false;

    void OnGUI()
    {
        float buttonWidth = 50f;
        float buttonHeight = 40f;

        buttonWidth *= Screen.width / 500f;
        buttonHeight *= Screen.height / 500f;

        isDebugON = GUI.Toggle(new Rect(10, Screen.height - 30f, buttonWidth / 2f, buttonHeight / 2f), isDebugON, isDebugON ? "LESS" : "MORE" );

        if (isDebugON)
        {
            if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Main"))
            {
                Application.LoadLevel("GamePlay");
            }

            if (GUI.Button(new Rect(0, 1 * buttonHeight, buttonWidth, buttonHeight), "L-1"))
            {
                Application.LoadLevel("L-1");
            }
        }
    }
}
