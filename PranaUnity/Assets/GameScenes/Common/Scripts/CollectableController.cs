using UnityEngine;
using System.Collections;

public class CollectableController : BaseMonoBehaviour {
    public delegate void PickedDelegate(GameObject collectable, GameObject picker);
	public event PickedDelegate OnPicked;

    private bool IsPicked;

	// Use this for initialization
	void Start () {
        IsPicked = false;
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
        if (!IsPicked) {
            if (other.gameObject.tag == "Player") {
                picked(other.gameObject);
            }
        }
	}

    void picked(GameObject picker) {
		//gameObject.SetActive(false);
        IsPicked = true;
		if (OnPicked != null)
            OnPicked(this.gameObject, picker);
        SendMessage("Picked", picker);
	}
}
