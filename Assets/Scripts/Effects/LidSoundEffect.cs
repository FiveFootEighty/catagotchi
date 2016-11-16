using UnityEngine;
using System.Collections;

public class LidSoundEffect : MonoBehaviour {

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool playSound = true;
    
    void Start () {
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (transform.localRotation.eulerAngles.z > -4 && transform.localRotation.eulerAngles.z < 4)
        {
            if (playSound)
            {
                playSound = false;

                if (rb.angularVelocity.magnitude < 0.005f)
                {
                    audioSource.volume = 6/10f;
                } else
                {
                    audioSource.volume = rb.angularVelocity.magnitude / 10f;
                }
                
                audioSource.Play();
            }
        } else
        {
            playSound = true;
        }
	}
}
