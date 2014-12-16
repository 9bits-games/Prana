using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectSC: SceneController {
    public string SpringLevelName = "Spring";
    public string SpringLevelID = "Spring";

	void OnGUI() {
        DrawResetButton();
        DrawSpringButton();
	}

    void DrawResetButton() {
        Rect buttonRect = new Rect(Screen.width * 0.1f, Screen.height * 0.9f, 80, 30);
        buttonRect.y -= buttonRect.height;

        string text = string.Format("Reset");
        if (GUI.Button(buttonRect, text))
            PersistentState.ResetSate();
    }

    void DrawSpringButton() {
        Rect buttonRect = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 30);
        buttonRect.x -= buttonRect.width * 0.5f;
        buttonRect.y -= buttonRect.height * 0.5f;

        int itemsForSpring = PersistentState.GetItemsCollectedForLevel(SpringLevelID);
        string spring_button = string.Format("{0} ({1})", SpringLevelName, itemsForSpring);

        if (GUI.Button(buttonRect, spring_button))
            Application.LoadLevel(SpringLevelID);
    }
}