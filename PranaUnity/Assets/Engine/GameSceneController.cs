using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSceneController: SceneController {
	CollectablesManager collectableManager;

	// Use this for initialization
	void Start () {
		collectableManager = GetComponentInChildren<CollectablesManager>();

	}

	void FinishScene() {
		Application.LoadLevel(LevelSelectSceneID);
	}

	// Update is called once per frame
	void Update () {}
}
