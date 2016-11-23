using UnityEngine;
using System.Collections;

public class PateBehavior : MonoBehaviour {

    private AudioSource audioSource;
    private bool released = false;

	void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void Release()
    {
        Invoke("ReleaseTimed", 0.1f);
    }

    private void ReleaseTimed()
    {
        released = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (released && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
