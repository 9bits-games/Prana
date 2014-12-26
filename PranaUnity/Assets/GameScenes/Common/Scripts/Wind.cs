using UnityEngine;
using System;
using System.Collections;

public class Wind : BaseMonoBehaviour {
	public GameObject windBlow;
	public Transform player;
	public float linealWindWeakening = 3f;
	public float MaxWindStrenght = 3f;
	public float VerticalDisplacementToForce = 1f;
	public float HorizontalDisplacementToForce = 0.1f;
	public bool spawnWindBlow = false; 

    public void ApplyWind(Vector3 windForce) {
        linealWindStrenght += windForce;
    }


	private Vector3 linealWindStrenght;
	private bool draggingL;
	private bool draggingR;
	private Vector3 prevLWorldPos;
	private Vector2 prevLScreenPos;
	private Vector2 prevRPos;

//	public float rotationalWindWeakening;
//	private Vector3 rotationalWindStrenght;

	void Start () {
		draggingL = false;
		draggingR = false;
	}

	private const float tta = 0f;

	void Update () {
		Debug.DrawLine(player.position, player.position + linealWindStrenght, Color.blue);

		//Left click dragging
		if (Input.GetMouseButtonDown(0)) {
			if (!draggingL) {
				Vector3? point_maybe = projectPointOnPlane(Input.mousePosition, player.position.y);

				if (point_maybe.HasValue) {
					Vector3 point = point_maybe.Value;
					draggingL = true;
					prevLWorldPos = point;
					prevLScreenPos = Input.mousePosition;
				} else {
					Debug.LogWarning("Mouse not interescting with player plane.");
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			draggingL = false;
		}
			
		//Right click dragging
		if (Input.GetMouseButtonDown(1)) {
			if (!draggingR) {
				draggingR = true;
				prevRPos = Input.mousePosition;
			}
		} else if (Input.GetMouseButtonUp(1)) {
			draggingR = false;
		}
	}

	void FixedUpdate() {
		if (draggingL) {
			Vector3? point_maybe = projectPointOnPlane(Input.mousePosition, player.position.y);

			if (point_maybe.HasValue) {
				Vector3 point = point_maybe.Value;
				Vector2 screenPos = Input.mousePosition;
				computeAndApplyPlanarWind(point, (point - prevLWorldPos).normalized, (screenPos - prevLScreenPos).magnitude );
				prevLWorldPos = point;
				prevLScreenPos = screenPos;
			} else {
				Debug.LogWarning("Mouse not interescting with player plane.");
			}
		}

		if (draggingR) {
			Vector3 point = Input.mousePosition;
			sendVerticalWind(Vector3.zero, point.y - prevRPos.y);
			prevRPos = point;
		}

		ComputeWindForces();
	}

	Vector3? projectPointOnPlane(Vector3 position, float height) {
		Ray ray = Camera.main.ScreenPointToRay(position);
		Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
		float distance = 0; 
		if (plane.Raycast(ray, out distance)) {
			return ray.GetPoint(distance) - player.transform.position;
		} else {
			return null;
		}
	}


    void computeAndApplyPlanarWind(Vector3 lPos, Vector3 gDir, float scale) {
		Vector3 force = gDir * scale * HorizontalDisplacementToForce;
		linealWindStrenght += force;

        Vector3 gPost = player.transform.position + lPos;
        Debug.DrawLine(gPost, gPost + gDir * scale, Color.blue);
        SpawnWindBlow(gPost, gDir * (1f + scale / 2f) + player.rigidbody.velocity * 0.7f);
	}

	void sendVerticalWind(Vector3 lPos, float displacement) {
		Vector3 force = Vector3.up * displacement * VerticalDisplacementToForce;
		linealWindStrenght += force;

        Vector3 gPost = player.transform.position + lPos;
        SpawnWindBlow(gPost, Vector3.up * (1f + displacement / 2f) + player.rigidbody.velocity * 0.7f);
	}

	void SpawnWindBlow(Vector3 pos, Vector3 vel) {
		if (spawnWindBlow) {
			GameObject wb = Instantiate(windBlow, RandomPos(pos), Quaternion.identity) as GameObject;
			wb.transform.localScale *= 0.1f;
			wb.rigidbody.velocity = Vector3.ClampMagnitude(vel, 4f);
		}
	}

	Vector3 RandomPos(Vector3 pos) {
		float rad = 2f;
		Vector3 rnd = new Vector3(UnityEngine.Random.Range(-rad, rad), UnityEngine.Random.Range(-rad, rad), UnityEngine.Random.Range(-rad, rad));
		return pos + rnd;
	}

	void ComputeWindForces() {
		/* Reducing wind Strenght */
		Vector3 lswDir = linealWindStrenght.normalized;
		linealWindStrenght -= lswDir * linealWindWeakening * Time.deltaTime;

		/* Clamping the wind Strenght */
		linealWindStrenght = Vector3.ClampMagnitude(linealWindStrenght, MaxWindStrenght);
			
		player.rigidbody.velocity += linealWindStrenght * Time.deltaTime; 
	}
}
