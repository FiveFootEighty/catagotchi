using UnityEngine;
using System.Collections;

public class CanOpenerBehavior : MonoBehaviour {

    public Transform placedCan;
    private Transform ghostCan;

    public AudioClip punctureSound;
    public AudioClip openedSound;
    public AudioClip canPlacedSound;

    private AudioSource audioSource;

	void Start () {
        ghostCan = transform.Find("GhostCan");
        audioSource = GetComponent<AudioSource>();
    }

    public Transform OnPateUnGrab()
    {
        ghostCan.gameObject.SetActive(false);

        audioSource.clip = canPlacedSound;
        audioSource.Play();

        return ghostCan;
    }

    public void OnCanPuntured()
    {
        audioSource.clip = punctureSound;
        audioSource.Play();
    }

    public void OnCanOpened()
    {
        audioSource.clip = openedSound;
        audioSource.Play();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<CanObject>() != null && !collider.gameObject.GetComponent<CanObject>().opened)
        {
            if (placedCan == null)
            {
                placedCan = collider.transform;
                placedCan.GetComponent<CanObject>().SetCanOpenerBehavior(this);
                ghostCan.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<CanObject>() != null && !collider.gameObject.GetComponent<CanObject>().opened)
        {
            if (placedCan == null)
            {
                placedCan = collider.transform;
                placedCan.GetComponent<CanObject>().SetCanOpenerBehavior(this);
                ghostCan.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform == placedCan)
        {
            placedCan.GetComponent<CanObject>().SetCanOpenerBehavior(null);
            placedCan = null;
            ghostCan.gameObject.SetActive(false);
        }
    }
}
