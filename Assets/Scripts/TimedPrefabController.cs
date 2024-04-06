using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPrefabController : MonoBehaviour
{
    [Header("Destruction properties")]
    [SerializeField, Tooltip("The delay to destroy after all playbacks are finished")] private float destroyDelay;

    [Header("Audio properties")]
    private bool audioFinished = false;
    [SerializeField] private float audioPlaybackDelay, minPitch, maxPitch;
    private AudioSource source;

    [Header("Particle properties")]
    private bool particlesFinished = false;
    [SerializeField] private float particlesPlaybackDelay;
    private ParticleSystem particles;

    private void Start()
    {
        if (GetComponent<AudioSource>()) { source = GetComponent<AudioSource>(); }
        if (GetComponent<ParticleSystem>()) { particles = GetComponent<ParticleSystem>(); }

        Invoke(nameof(PlayAudio), audioPlaybackDelay);
        Invoke(nameof(PlayParticles), particlesPlaybackDelay);
    }

    private void Update()
    {
        audioFinished = source.isPlaying ? false : true;
        particlesFinished = particles.isPlaying ? false : true;
    }

    private void PlayAudio()
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
    }

    private void PlayParticles()
    {
        particles.Play();
    }

    IEnumerator Destroy()
    {
        while(!audioFinished && !particlesFinished)
        {
            Debug.Log("Playing");
            yield return null;
        }
        Debug.Log("Destroy");
        Destroy(gameObject, destroyDelay);

        yield return null;
    }
}
