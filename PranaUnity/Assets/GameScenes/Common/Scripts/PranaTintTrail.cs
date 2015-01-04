using System;
using UnityEngine;
using System.Collections;

public class PranaTintTrail : BaseMonoBehaviour {
    public float TimeToPrepareBeforeDeath = 1f;

    public void Initialize(Transform target, Color tintColor, Transform parent = null) {
        SetColor(tintColor);
        TargetPursuer targetPursuer = GetComponent<TargetPursuer>();
        targetPursuer.Target = target;
        if (parent != null)
            this.transform.parent = parent;

        Invoke("PreDeath", targetPursuer.LifeTime - TimeToPrepareBeforeDeath);
    }

    public void SetColor(Color color, bool setAlpha = false) {
        Material material = this.renderer.material;
        Color oldColor = material.GetColor("_TintColor");
        if(!setAlpha) color.a = oldColor.a;
        material.SetColor("_TintColor", color);
    }

    void Start() {
    }

    void PreDeath() {
        this.GetComponent<Animator>().SetBool("Alive", false);
    }
}

