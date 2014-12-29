using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Wind : BaseMonoBehaviour {
	public GameObject WindBlow;
	public Transform Player;
    public Camera PlayerCamera;
	public float LinealWindWeakening = 3f;
	public float MaxWindStrenght = 3f;
	public float VerticalDisplacementToForce = 1f;
	public float HorizontalDisplacementToForce = 0.1f;
    public bool DoSpawnWindBlow = false;
    public float TotalTimeToSpawnWindBlow = 2f;
    public float WindBlowRandomPlacementRadius = 0.5f;

    public void ApplyWind(Vector3 windForce) {
        LinealWindStrenght += windForce;
    }

    private Vector3 LinealWindStrenght;
    private bool DraggingL;
    private bool DraggingR;
    private Vector3 PrevLWorldPos;
    private Vector2 PrevLScreenPos;
    private Vector2 PrevRPos;
    private List<GameObject> WindBlows;
    private float TimeToSpawnWindBlow;

//	public float rotationalWindWeakening;
//	private Vector3 rotationalWindStrenght;

    void Awake() {
        WindBlows = new List<GameObject>();
    }

	void Start () {
		DraggingL = false;
		DraggingR = false;
        TimeToSpawnWindBlow = 0;
	}

	private const float tta = 0f;

	void Update () {
		Debug.DrawLine(Player.position, Player.position + LinealWindStrenght, Color.blue);

		//Left click dragging
		if (Input.GetMouseButtonDown(0)) {
			if (!DraggingL) {
				Vector3? point_maybe = ProjectPointOnPlane(Input.mousePosition, Player.position.y);

				if (point_maybe.HasValue) {
					Vector3 point = point_maybe.Value;
					DraggingL = true;
					PrevLWorldPos = point;
					PrevLScreenPos = Input.mousePosition;
				} else {
					Debug.LogWarning("Mouse not interescting with player plane.");
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			DraggingL = false;
		}
			
		//Right click dragging
		if (Input.GetMouseButtonDown(1)) {
			if (!DraggingR) {
				DraggingR = true;
				PrevRPos = Input.mousePosition;
			}
		} else if (Input.GetMouseButtonUp(1)) {
			DraggingR = false;
		}
	}

	void FixedUpdate() {
        TimeToSpawnWindBlow -= Time.fixedDeltaTime;

		if (DraggingL) {
			Vector3? point_maybe = ProjectPointOnPlane(Input.mousePosition, Player.position.y);

			if (point_maybe.HasValue) {
				Vector3 point = point_maybe.Value;
				Vector2 screenPos = Input.mousePosition;
				ComputeAndApplyPlanarWind(point, (point - PrevLWorldPos).normalized, (screenPos - PrevLScreenPos).magnitude );
				PrevLWorldPos = point;
				PrevLScreenPos = screenPos;
			} else {
				Debug.LogWarning("Mouse not interescting with player plane.");
			}
		}

		if (DraggingR) {
			Vector3 point = Input.mousePosition;
			SendVerticalWind(Vector3.zero, point.y - PrevRPos.y);
			PrevRPos = point;
		}

		ComputeWindForces();
	}

	Vector3? ProjectPointOnPlane(Vector3 position, float height) {
		Ray ray = Camera.main.ScreenPointToRay(position);
		Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
		float distance = 0; 
		if (plane.Raycast(ray, out distance)) {
			return ray.GetPoint(distance) - Player.transform.position;
		} else {
			return null;
		}
	}


    void ComputeAndApplyPlanarWind(Vector3 lPos, Vector3 gDir, float scale) {
		Vector3 force = gDir * scale * HorizontalDisplacementToForce;
		LinealWindStrenght += force;

        Vector3 gPost = Player.transform.position + lPos;
        Debug.DrawLine(gPost, gPost + gDir * scale, Color.blue);
        SpawnWindBlow(gPost, gDir * (1f + scale / 2f) + Player.rigidbody.velocity * 0.7f);
	}

	void SendVerticalWind(Vector3 lPos, float displacement) {
		Vector3 force = Vector3.up * displacement * VerticalDisplacementToForce;
		LinealWindStrenght += force;

        Vector3 gPost = Player.transform.position + lPos;
        SpawnWindBlow(gPost, Vector3.up * (1f + displacement / 2f) + Player.rigidbody.velocity * 0.7f);
	}

	void ComputeWindForces() {
		/* Reducing wind Strenght */
		Vector3 lswDir = LinealWindStrenght.normalized;
		LinealWindStrenght -= lswDir * LinealWindWeakening * Time.deltaTime;

		/* Clamping the wind Strenght */
		LinealWindStrenght = Vector3.ClampMagnitude(LinealWindStrenght, MaxWindStrenght);
			
		Player.rigidbody.velocity += LinealWindStrenght * Time.deltaTime; 
	}

    void SpawnWindBlow(Vector3 position, Vector3 velocity) {
        if (DoSpawnWindBlow) {

            if(TimeToSpawnWindBlow <= 0f) {
                GameObject wb = Instantiate(WindBlow, RandomPos(position), Quaternion.identity) as GameObject;

                WindBlows.Add(wb);
                wb.GetComponent<WindBlowController>().OnDestroyEvent += () => WindBlows.Remove(wb);

                TimeToSpawnWindBlow = TotalTimeToSpawnWindBlow;
            }

            foreach (GameObject wb in WindBlows) {
                WindBlowController WindBlowCtrl = wb.GetComponent<WindBlowController>();
                WindBlowCtrl.addVelocity(velocity);
            }
        }
    }

    Vector3 RandomPos(Vector3 position) {
        Vector3 finalPosition = position; 

        Vector3 randomPerturbation = new Vector3(
            UnityEngine.Random.Range(-WindBlowRandomPlacementRadius, WindBlowRandomPlacementRadius),
            UnityEngine.Random.Range(-WindBlowRandomPlacementRadius, WindBlowRandomPlacementRadius),
            UnityEngine.Random.Range(-WindBlowRandomPlacementRadius, WindBlowRandomPlacementRadius)
        );
        finalPosition += randomPerturbation;

        Vector3 cameraFocusPosition = PlayerCamera.transform.position + PlayerCamera.transform.forward * 10f;
        Vector3 towardCameraFocus = (cameraFocusPosition - finalPosition) * 0.5f;
        finalPosition += towardCameraFocus;

        return finalPosition;
    }
}
