using UnityEngine;
using System.Collections;

public class StartGameBooklet : GrabbableObject, TrackedControllerBase.TrackedControllerTrackpadListener
{

    private int currentPage = 0;
    public Transform[] pages;
    public Transform binding;
    public BoxCollider boxCollider;

    private GrabController savedController;

    private bool isTurning = false;
    private bool isSliding = false;
    
    private AudioSource audioSource;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;

        audioSource = GetComponent<AudioSource>();

        UpdatePage();
    }

    public void TurnPageLeft()
    {
        if (currentPage < pages.Length && !isTurning && !isSliding)
        {
            currentPage++;
            StartCoroutine(Rotate(pages[currentPage - 1], currentPage/5f, 180-currentPage / 5f, 0.3f));
            audioSource.Play();
        }
    }

    public void TurnPageRight()
    {
        if (currentPage > 0 && !isTurning && !isSliding)
        {
            currentPage--;
            StartCoroutine(Rotate(pages[currentPage], 180 - currentPage / 5f, (pages.Length - 1 - currentPage) / 5f, 0.3f));
            audioSource.Play();
        }
    }

    public override void AfterOnGrab()
    {
        interationPoint.rotation = transform.rotation;
        interationPoint.Rotate(new Vector3(-90, 0, 0));

        controller.trackedControllerBase.RegisterTrackpadListener(this);
        savedController = controller;
    }

    public override void AfterOnUnGrab()
    {
        savedController.trackedControllerBase.UnregisterTrackpadListener(this);
        savedController = null;
    }

    private IEnumerator Rotate(Transform page, float startAngle, float finishAngle, float time)
    {
        if (page == null)
            yield return null;
        isTurning = true;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            Vector3 translate = binding.localPosition - page.localPosition;
            page.Translate(new Vector3(-0.1f, 0, 0), Space.Self);
            page.localRotation = Quaternion.Euler(new Vector3(page.localRotation.eulerAngles.x, Mathf.Lerp(startAngle, finishAngle, elapsedTime / time), page.localRotation.eulerAngles.z));
            page.Translate(new Vector3(0.1f, 0, 0), Space.Self);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        float diff = -0.1f;
        if (finishAngle < startAngle)
        {
            diff = 0.1f;
        }
        page.Translate(new Vector3(-0.1f, 0, 0), Space.Self);
        page.localRotation = Quaternion.Euler(0, finishAngle, 0);
        page.Translate(new Vector3(0.1f, 0, 0), Space.Self);
        //page.localPosition = new Vector3(diff, 0, 0);
        isTurning = false;
        UpdatePage();
    }

    bool isFront = false;
    bool isBack = false;
    private void UpdatePage()
    {
        if (currentPage == 0 && !isFront)
        {
            // should center the pages
            for (int i = 0; i < pages.Length; i++)
            {
                float savedY = pages[i].localPosition.y;
                float savedZ = pages[i].localPosition.z;
                pages[i].Translate(new Vector3(-0.1f, 0, 0), Space.Self);
                pages[i].localPosition = new Vector3(pages[i].localPosition.x, savedY, savedZ);

                //StartCoroutine(TranslateLerp(pages[i], pages[i].localPosition.x, pages[i].localPosition.x - 0.1f, 0.3f));
                
            }
            //StartCoroutine(TranslateLerp(binding, binding.localPosition.x, binding.localPosition.x - 0.1f, 0.3f));
            binding.Translate(new Vector3(-0.1f, 0, 0), Space.Self);
            isFront = true;
            boxCollider.size = new Vector3(0.2f, 0.2f, 0.01f);
        } else if (currentPage == pages.Length && !isBack)
        {
            // should center the pages
            for (int i = 0; i < pages.Length; i++)
            {
                float savedY = pages[i].localPosition.y;
                float savedZ = pages[i].localPosition.z;
                pages[i].Translate(new Vector3(-0.1f, 0, 0), Space.Self);
                pages[i].localPosition = new Vector3(pages[i].localPosition.x, savedY, savedZ);

                //StartCoroutine(TranslateLerp(pages[i], pages[i].localPosition.x, pages[i].localPosition.x + 0.1f, 0.3f));
            }
            binding.Translate(new Vector3(-0.1f, 0, 0), Space.Self);

            //StartCoroutine(TranslateLerp(binding, binding.localPosition.x, binding.localPosition.x + 0.1f, 0.3f));
            
            isBack = true;
            boxCollider.size = new Vector3(0.2f, 0.2f, 0.01f);
        } else if (isFront || isBack)
        {
            float diff = -0.1f;
            if (isBack)
            {
                diff = 0.1f;
            }
            // should not center the pages
            for (int i = 0; i < pages.Length; i++)
            {
                if (pages[i].localRotation.eulerAngles.y < 90)
                {
                    if (isBack)
                    {
                        diff = -0.1f;
                    } else if (isFront)
                    {
                        diff = 0.1f;
                    }
                } else
                {
                    if (isBack)
                    {
                        diff = 0.1f;
                    }
                    else if (isFront)
                    {
                        diff = -0.1f;
                    }
                }
                //StartCoroutine(TranslateLerp(pages[i], pages[i].localPosition.x, pages[i].localPosition.x + diff, 0.3f));
                
                float savedY = pages[i].localPosition.y;
                float savedZ = pages[i].localPosition.z;
                pages[i].Translate(new Vector3(diff, 0, 0), Space.Self);
                pages[i].localPosition = new Vector3(pages[i].localPosition.x, savedY, savedZ);
            }
            //StartCoroutine(TranslateLerp(binding, binding.localPosition.x, binding.localPosition.x + diff, 0.3f));
            
            binding.Translate(new Vector3(diff, 0, 0), Space.Self);
            isBack = false;
            isFront = false;
            boxCollider.size = new Vector3(0.4f, 0.2f, 0.01f);
        }
    }

    //someday this will work
    private IEnumerator TranslateLerp(Transform page, float startPosition, float endPosition, float time)
    {
        float savedY = page.localPosition.y;
        float savedZ = page.localPosition.z;
        isSliding = true;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            page.localPosition = Mathf.Lerp(startPosition, endPosition, elapsedTime / time) * page.right;//;new Vector3(Mathf.Lerp(startPosition, endPosition, elapsedTime / time), page.localPosition.y, page.localPosition.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        page.localPosition = new Vector3(page.localPosition.x, savedY, savedZ);
        isSliding = false;
    }

    public void OnTrackpadTouched(int location)
    {

    }

    public void OnTrackpadPress(int location)
    {

    }

    public void OnTrackpadPressUp(int location)
    {

    }

    public void OnTrackpadPressDown(int location)
    {
        if (controller != null && controller.selectedObject == gameObject)
        {
            if (location == TrackedControllerBase.TRACKPAD_LEFT)
            {
                TurnPageLeft();
            }
            else if (location == TrackedControllerBase.TRACKPAD_RIGHT)
            {
                TurnPageRight();
            }
        }
    }
}
