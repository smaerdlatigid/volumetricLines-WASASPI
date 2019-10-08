using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class audioParticles : MonoBehaviour
{
    public LoopbackAudio Audio;
    VisualEffect visualEffect;
    public float sizeScale; 
    public float lifeTimeScale;
    public float spawnScale;

    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        visualEffect.SetFloat("LifeTime", Audio.WeightedAverage*lifeTimeScale);
        visualEffect.SetFloat("ParticleSize",Audio.WeightedPostScaledSpectrumData[0]*sizeScale);
        visualEffect.SetFloat("SpawnRate", Audio.WeightedAverage2*spawnScale);
    }
}
