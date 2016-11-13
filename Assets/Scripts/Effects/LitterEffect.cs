using UnityEngine;
using System.Collections;

public class LitterEffect : MonoBehaviour {

    public AudioClip sandSound1;

    private Transform interactionPoint;
    private AudioSource audioSource;

	void Start () {

        interactionPoint = transform.FindChild("SoundOrigin");

        audioSource = interactionPoint.GetComponent<AudioSource>();
    }
	
	void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "CatCafe_Scooper")
        {
            interactionPoint.position = new Vector3(collider.transform.position.x, interactionPoint.transform.position.y, collider.transform.position.z);
            audioSource.loop = true;
            audioSource.volume = collider.GetComponent<Rigidbody>().velocity.magnitude/2;
            audioSource.Play();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.name == "CatCafe_Scooper")
        {
            audioSource.loop = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.name == "CatCafe_Scooper")
        {
            audioSource.volume = collider.GetComponent<Rigidbody>().velocity.magnitude/2;
            interactionPoint.position = new Vector3(collider.transform.position.x, interactionPoint.transform.position.y, collider.transform.position.z);
        }
    }
}
