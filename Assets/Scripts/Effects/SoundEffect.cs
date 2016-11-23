using UnityEngine;
using System.Collections;

public class SoundEffect : MonoBehaviour {

    [HideInInspector]
    public static bool globalMute = false;

    private AudioSource audioSource;

    public float minVolume = 0;
    public float maxVolume = 1;

    public float randomizedPitchMarginMin = -1;
    public float randomizedPitchMarginMax = 1;

    public bool waitForSoundToFinish = false;

    private AudioClip clip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
    }

    public void PlaySound()
    {
        PlaySound(clip);
    }

    public void PlaySound(float intensity)
    {
        PlaySound(clip, intensity);
    }

    public void PlaySound(AudioClip clip)
    {
        PlaySound(clip, 1);
    }

    public void PlaySound(AudioClip clip, float intensity)
    {
        if (clip == null)
        {
            return;
        }
        if (waitForSoundToFinish)
        {
            if (audioSource.isPlaying)
            {
                return;
            }
        }
        if (!audioSource.isPlaying && !globalMute)
        {
            float savedPitch = audioSource.pitch;

            if (randomizedPitchMarginMax - randomizedPitchMarginMin > 0)
            {
                audioSource.pitch = audioSource.pitch + Random.Range(audioSource.pitch + randomizedPitchMarginMin, audioSource.pitch + randomizedPitchMarginMax);
            }

            if (maxVolume - minVolume > 0)
            {
                if (intensity < minVolume)
                {
                    return;
                }
                if (intensity > maxVolume)
                {
                    intensity = maxVolume;
                }
                audioSource.volume = intensity;
            }
            
            audioSource.clip = clip;
            audioSource.Play();

            audioSource.pitch = savedPitch;
        }
    }

    public void PlaySound(PhysicMaterial materialObject, PhysicMaterial contactObject)
    {
        PlaySound(materialObject, contactObject, 1);
    }

    public void PlaySound(PhysicMaterial materialObject, PhysicMaterial contactObject, float intensity)
    {
        PlaySound(SoundEffectRegistry.instance().GetSoundCombination(materialObject.name, contactObject.name), intensity);
    }
}
