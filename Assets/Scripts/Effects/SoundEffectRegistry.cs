using UnityEngine;
using System.Collections;

public class SoundEffectRegistry : MonoBehaviour
{

    public SoundEffectItem[] soundEffectItems;

    [System.Serializable]
    public class SoundEffectItem
    {
        public AudioClip soundClip;
        public PhysicMaterial materialObject;
        public PhysicMaterial contactObject;
    }

    private static SoundEffectRegistry soundEffectRegistry;

    public static SoundEffectRegistry instance()
    {
        if (soundEffectRegistry == null)
        {
            soundEffectRegistry = FindObjectOfType(typeof(SoundEffectRegistry)) as SoundEffectRegistry;
        }

        // If it is still null, create a new instance
        if (soundEffectRegistry == null)
        {
            GameObject obj = new GameObject("SoundEffectRegistry");
            soundEffectRegistry = obj.AddComponent(typeof(SoundEffectRegistry)) as SoundEffectRegistry;
        }

        return soundEffectRegistry;
    }

    public AudioClip GetSoundCombination(string materialObjectName, string contactObjectName)
    {
        foreach (SoundEffectItem item in soundEffectItems)
        {
            if (contactObjectName.Contains(item.contactObject.name) && item.materialObject == null)
            {
                return item.soundClip;
            } else if (item.materialObject != null && materialObjectName.Contains(item.materialObject.name) && contactObjectName.Contains(item.contactObject.name))
            {
                return item.soundClip;
            }
        }

        return null;
    }
}
