using UnityEngine;
using System;
using System.Collections;

public class CameraController : BaseMonoBehaviour {

	public Transform target = null;

	public float heightOverTarget;
	public float maxDistanceToTarget;
	public float angleTolerance;
	public float maxAceleration;
	public float multiplyToLookAheadOfTarget = 2f;
	public float SpiritTurbulenceSize = 0.2f;
	public float SpiritTurbulenceFrequency = 1.2f;

	public float minFoV = 35f;
	public float maxFoV = 80f;
	public float FoVChangeFactor = 0.01f;

	//private Vector3 velocity;
	private Vector3 matchingVelocity;
	private Vector3 chaseVelocity;
	private Vector3 lookAtPoint;

	void Start () {
		matchingVelocity = target.rigidbody.velocity;
		chaseVelocity = Vector3.zero;
		lookAtPoint = target.position;
	}

	void Update () {
		float speedLerp = Mathf.InverseLerp(0, target.GetComponent<PaperPlaneController>().maxSpeed, target.rigidbody.velocity.magnitude);
		float desiredFoV= Mathf.Lerp(maxFoV, minFoV, speedLerp);
		camera.fieldOfView += (desiredFoV - camera.fieldOfView) * FoVChangeFactor;
		
		transform.LookAt(lookAtPoint);
		Debug.DrawRay(transform.position, lookAtPoint - transform.position, Color.blue);
		Debug.DrawRay(target.position, positionObjetctive() - target.position, Color.red);
	}

	void FixedUpdate () {
		float dt = Time.deltaTime;
		chaseVelocity *= 0.9f;
		matchingVelocity += (target.rigidbody.velocity - matchingVelocity) * 0.1f;
		Vector3 targetVelocity = target.rigidbody.velocity;
		Vector3 directionToTarget = positionObjetctive() - transform.position;

		if(canBreakSafeTo(target.position)) {
			chaseVelocity += directionToTarget * maxAceleration * dt;
		} else {
			float angleBetwenTargetAndVeolocity = Vector3.Angle(chaseVelocity, directionToTarget);
			if(angleBetwenTargetAndVeolocity < 90) {
				chaseVelocity -= directionToTarget * maxAceleration * dt;
			} else {
				chaseVelocity += directionToTarget * maxAceleration * dt;
			}
		}

		Vector3 desiredLookAtDirection = targetVelocity * multiplyToLookAheadOfTarget;
		Vector3 desiredLookAtPoint = target.position + desiredLookAtDirection;
		desiredLookAtPoint.y = target.position.y + desiredLookAtDirection.y * 0.5f;
		lookAtPoint += (desiredLookAtPoint - lookAtPoint) * 0.1f;

		Vector3 velocity = matchingVelocity + chaseVelocity;
		Debug.DrawRay(transform.position, velocity, Color.green);
		transform.position += velocity * dt;
	}
	
	bool canBreakSafeTo(Vector3 position) {
		Vector3 directionToTarget = target.position - positionObjetctive();
		float distanceToTarget = directionToTarget.magnitude;
		float sqrChaceSpeed = chaseVelocity.sqrMagnitude;

		bool doBreak = maxAceleration * distanceToTarget >= sqrChaceSpeed;
		return doBreak;
	}
	
	Vector3 positionObjetctive() {
		Vector3 desiredPosition;
		Vector3 vecFromTarget = transform.position - target.position;
		vecFromTarget.y = 0f;
		vecFromTarget = Math3d.SetVectorLength(vecFromTarget, maxDistanceToTarget);
		
		Vector3 backward = -target.forward;
		backward.y = 0f;
		backward.Normalize();
		
		float angle = Vector3.Angle(vecFromTarget, backward);
		if(angle > angleTolerance) {
			float diffAngle = angle - angleTolerance;

			vecFromTarget =  Vector3.RotateTowards(vecFromTarget, backward, diffAngle, 0.0F);
		}
		
		desiredPosition = target.position + vecFromTarget;
		desiredPosition.y += heightOverTarget;
		
		desiredPosition += new Vector3(
			(float) Math.Sin(Time.time * SpiritTurbulenceFrequency),
			(float) Math.Cos(Time.time * SpiritTurbulenceFrequency / 3f),
			(float) -Math.Sin(Time.time * SpiritTurbulenceFrequency / 5f)
		) * SpiritTurbulenceSize;
		
		return desiredPosition;
	}
}
