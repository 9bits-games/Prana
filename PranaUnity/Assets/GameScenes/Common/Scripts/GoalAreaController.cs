using UnityEngine;
using System.Collections;

public class GoalAreaController : BaseMonoBehaviour {
	public delegate void ReachedDelegate(GameObject goalArea);
	public event ReachedDelegate OnReached;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			reached();
		}
	}

	void reached() {
		gameObject.SetActive(false);
		if (OnReached != null)
			OnReached(this.gameObject);
	}
}


