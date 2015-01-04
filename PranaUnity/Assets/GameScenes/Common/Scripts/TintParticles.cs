using System;
using UnityEngine;
using System.Collections;

public class TintParticles : BaseMonoBehaviour {

    public Color TintColor = Color.blue;
    private ParticleSystem ParticleSystem;

    public void Initialize(Transform target, Color tintColor, Transform parent = null) {
        ParticleSystem = GetComponent<ParticleSystem>();

        SetColor(tintColor);
        TargetPursuer targetPursuer = GetComponent<TargetPursuer>();
        targetPursuer.Target = target;
        if (parent != null)
            this.transform.parent = parent;

        Invoke("PreDeath", targetPursuer.LifeTime - ParticleSystem.startLifetime);
    }

    public void SetColor(Color color, bool setAlpha = false) {
//        Material material = this.renderer.material;
//        Color oldColor = material.GetColor("_Color");
//        if(!setAlpha) color.a = oldColor.a;
//        material.SetColor("_Color", color);
        if(!setAlpha) color.a = ParticleSystem.startColor.a;
        ParticleSystem.startColor = color;
    }

    void PreDeath() {
        ParticleSystem.enableEmission = false;
    }
}

