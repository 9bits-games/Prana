using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameSceneController: SceneController {
	protected CollectablesManager CollectableManager;
	protected GoalAreaController GoalArea;

    public abstract string LevelID { get; }

	protected virtual void Start () {
      
        CollectableManager = GetComponentInChildren<CollectablesManager>();
		GoalArea = GetComponentInChildren<GoalAreaController>();
		GoalArea.OnReached += GoalAreaReached;
	}

	protected virtual void GoalAreaReached(GameObject goalArea) {
        PersistentState.SetItemsCollectedForLevel(LevelID, CollectableManager.ItemsCollected);
        PersistentState.Sync();

        EndingCinematic();
	}

    protected virtual void EndingCinematic() {
        FinishScene();
    }
	
	protected virtual void FinishScene() {
		Application.LoadLevel(LevelSelectSceneID);
	}

	void Update () {}
}
