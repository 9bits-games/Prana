using System;
using UnityEngine;
using System.Collections;

public class PickablePranaSpot : BaseMonoBehaviour {

    public float ParticlesLifetimeAfterPicked = 1f;
    public int TintTrailsCount = 5;
    public float TintTrailRandomRadius = 3f;

    public GameObject TintTrail;
    public GameObject TintParticles;
    public Color TintColor = Color.blue;

    private ParticleSystem[] ParticleSystems;
    private Animator LightAnimator;

//    ParticleSystem.Particle[] Particles;
//    Transform Picker;

    void Start() {
        ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        LightAnimator = GetComponentInChildren<Animator>();
//        Picker = null;
//
//        int totalParticles = 0;
//        foreach (ParticleSystem particleSystem in ParticleSystems) {
//            totalParticles += particleSystem.maxParticles;
//        }
//        Particles = new ParticleSystem.Particle[totalParticles];
    }

    void Picked(GameObject picker) {
//        int lastIndexToConcat = 0;
        LightAnimator.SetBool("Picked", true);
        shotPrana(picker.transform);

        foreach (ParticleSystem particleSystem in ParticleSystems) {
            particleSystem.enableEmission = false;

            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.maxParticles];
            int count = particleSystem.GetParticles(particles);
            for (int i = 0; i < count; i++) {
                float lifetime_percent = particles[i].lifetime / particles[i].startLifetime;
                particles[i].startLifetime = ParticlesLifetimeAfterPicked / (1f - lifetime_percent);
                particles[i].lifetime = lifetime_percent * particles[i].startLifetime;
            }
            particleSystem.SetParticles(particles, count);
        }

        Destroy(this.gameObject, 5f);
    }

    void shotPrana(Transform target) {
        Vector3 position = this.transform.position;
        for (int i = 0; i < TintTrailsCount; i++) {
            Vector3 trailPosition = position + Math3d.RandomVector3(-TintTrailRandomRadius, TintTrailRandomRadius);

            GameObject tintTrail = GameObject.Instantiate(TintTrail, trailPosition, Quaternion.identity) as GameObject;
            PranaTintTrail pranaTintTrailController  = tintTrail.GetComponent<PranaTintTrail>();
            pranaTintTrailController.Initialize(target, TintColor, this.transform.parent);
        }

        GameObject tintParticleSystem = GameObject.Instantiate(TintParticles, position, Quaternion.identity) as GameObject;

        TintParticles tintParticlesController  = tintParticleSystem.GetComponent<TintParticles>();
        tintParticlesController.Initialize(target, TintColor, this.transform.parent);
    }
}

