using UnityEngine;
using System.Collections;

public class CollectableController : BaseMonoBehaviour {
	public delegate void PickedDelegate(GameObject collectable);
	public event PickedDelegate OnPicked;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			picked();
		}
	}

	void picked() {
		gameObject.SetActive(false);
		if (OnPicked != null)
			OnPicked(this.gameObject);
	}
}
