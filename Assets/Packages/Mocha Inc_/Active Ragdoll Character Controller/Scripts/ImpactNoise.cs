using UnityEngine;

public class ImpactNoise : MonoBehaviour
{
    public bool enable = true;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float impactForce;
    public bool randomVolumeEnabled = true;
    public float maxVolumeForce = 15f;
    public float baseVolume = 1f;

    private void Awake()
    {
        baseVolume = audioSource.volume;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!enable) return;
        if(!(collision.relativeVelocity.magnitude >= impactForce)) return;

        if(audioSource.isPlaying) return;

        int i = (int)Random.Range(0, audioClips.Length - 1);
        if(randomVolumeEnabled)
        {
            float newVolume = collision.relativeVelocity.magnitude / maxVolumeForce;
            //Debug.Log("AudioSource: newVolume: " + newVolume + " magnitude: " + collision.relativeVelocity.magnitude);
            audioSource.volume = newVolume;
        }

        audioSource.PlayOneShot(audioClips[i]);
    }
}