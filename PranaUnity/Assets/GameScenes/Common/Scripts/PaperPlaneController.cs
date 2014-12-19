using System;
using UnityEngine;
using System.Collections;

public class PaperPlaneController : BaseMonoBehaviour {

	public float baseUpwardForce = 0.095f;
	public float speedUpwardForce = 0.0029f;
	public float maxSpeed = 3f;
    public float UpVectorRotationSpeed = 1f;

	// Use this for initialization
	void Start () {
		//rigidbody.AddRelativeForce(Vector3.forward * 1f);
	}

	void Update() {
		Debug.DrawRay(transform.position, rigidbody.velocity, Color.green);
	}

	void FixedUpdate () {
		Vector3 globalVel = rigidbody.velocity;
		Vector3 localVel = transform.InverseTransformDirection(globalVel);

		float liftByVel = Mathf.Clamp(localVel.z * speedUpwardForce, -speedUpwardForce, speedUpwardForce);
		rigidbody.AddForce(Vector3.up * (baseUpwardForce + liftByVel));

		ApplyAerodynamics();
	}

	void OnCollisionEnter(Collision collision) {
		bool isTerrain = false;
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			isTerrain = true;
		}

		if(isTerrain) {
		}
	}

	void ApplyAerodynamics() {
		if (rigidbody.velocity.sqrMagnitude > 2) {
			Vector3 spdDir = rigidbody.velocity.normalized;
			Vector3 forward = transform.forward;
			Vector3 heading = forward + (spdDir - forward) * 0.08f;

            Vector3 up = Vector3.RotateTowards(transform.up, Vector3.up, UpVectorRotationSpeed * Time.deltaTime, 0f);

            transform.LookAt(heading + transform.position, up);
		}
		
		rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
	}
}
