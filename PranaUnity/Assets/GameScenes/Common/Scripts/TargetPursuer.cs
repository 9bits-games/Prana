using System;
using UnityEngine;
using System.Collections;

public class TargetPursuer : BaseMonoBehaviour {
    public Transform Target;
    public float LifeTime = 4f;
    public float Acceleration = 1f;
    public float MaxSpeed = 3f;

    void Start() {
        Destroy(this.gameObject, LifeTime);
    }

    void FixedUpdate() {
        if (Target != null) {
            Vector3 toTarget = Target.position - this.transform.position;
            Vector3 direction = toTarget.normalized;

            this.rigidbody.velocity += direction * Acceleration * Time.fixedDeltaTime;
        }

        this.rigidbody.velocity = Vector3.ClampMagnitude(this.rigidbody.velocity, MaxSpeed);
    }
}

