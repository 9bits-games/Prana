using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectSC: SceneController {
	public string SpringLevelName = "Spring";
	public string SpringLevelID = "Spring";


	void OnGUI() {
		Rect buttonRect = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 30);
		buttonRect.x -= buttonRect.width * 0.5f;
		buttonRect.y -= buttonRect.height * 0.5f;

		if (GUI.Button(buttonRect, SpringLevelName))
			Application.LoadLevel(SpringLevelID);
	}
}