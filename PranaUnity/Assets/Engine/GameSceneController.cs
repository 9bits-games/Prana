using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSceneController: SceneController {
	protected CollectablesManager CollectableManager;
	protected GoalAreaController GoalArea;

	protected new void Start () {
		CollectableManager = GetComponentInChildren<CollectablesManager>();
		GoalArea = GetComponentInChildren<GoalAreaController>();
		GoalArea.OnReached += GoalAreaReached;
	}

	protected virtual void GoalAreaReached(GameObject goalArea) {
		FinishScene();
	}

	protected virtual void FinishScene() {
		Debug.Log("Finishing Scene");
		Application.LoadLevel(LevelSelectSceneID);
	}

	void Update () {}
}
