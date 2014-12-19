using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectablesManager : BaseMonoBehaviour {

	private CollectableController[] collectables;
	private List<CollectableController> collected;

    public int ItemsCollected { get { return collected.Count; } }

	// Use this for initialization
	void Start () {
		collectables = gameObject.GetComponentsInChildren<CollectableController>();
		collected = new List<CollectableController>(collectables.Length);

		foreach (CollectableController collectable in collectables) {
			collectable.OnPicked += collectablePicked;
		}
	}

	void collectablePicked(GameObject collectable) {
		collected.Add(collectable.GetComponent<CollectableController>());

		if (collected.Count == collectables.Length) {

		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 200, 50), string.Format("Collectables: {0}/{1}", collected.Count, collectables.Length));
	}
}
