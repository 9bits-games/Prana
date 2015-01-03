using System;
using UnityEngine;
using System.Collections;

public class PickablePranaSpot : BaseMonoBehaviour {

    ParticleSystem[] ParticleSystems;
//    ParticleSystem.Particle[] Particles;
//    Transform Picker;

    void Start() {
        ParticleSystems = GetComponentsInChildren<ParticleSystem>();
//        Picker = null;
//
//        int totalParticles = 0;
//        foreach (ParticleSystem particleSystem in ParticleSystems) {
//            totalParticles += particleSystem.maxParticles;
//        }
//        Particles = new ParticleSystem.Particle[totalParticles];
    }

    void Picked(GameObject picker) {
        int lastIndexToConcat = 0;
        foreach (ParticleSystem particleSystem in ParticleSystems) {
            particleSystem.enableEmission = false;

//            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.maxParticles];
//            int count = particleSystem.GetParticles(particles);
//            for (int i = 0; i < count; i++) {
////                particles[i].lifetime = 1f;
//            }
//            particleSystem.SetParticles(particles, count);
        }
    }

    void Update() {

    }
}

