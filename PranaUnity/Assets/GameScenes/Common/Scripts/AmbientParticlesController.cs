using System;
using UnityEngine;
using System.Collections;

public class AmbientParticlesController : BaseMonoBehaviour {
    public Transform TargetToFollow;

    void Awake() {
    }

    void Update() {
        transform.position = TargetToFollow.position;
    }
}

