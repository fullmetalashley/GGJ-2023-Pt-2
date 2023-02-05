using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float lerpDuration = 0.5f;
    bool rotating;
    public bool atDesk;

    public bool underDesk;

    public Transform atDeskTransform;
    public Transform underDeskTransform;

    public GameObject deskArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;
    

    public void Rotate(int value)
    {
        //Index -1 is the left button.
        //Index 1 is the right button.

        //First, check what direction we're facing.
        //What is our current rotation value? 
        if (!rotating){
            if (Camera.main.transform.rotation.y == 0 && !underDesk)
            {
                //First, rotate up. Then we can rotate 90. 
                StartCoroutine(RotateUp(value));
            }
            else
            {
                StartCoroutine(Rotate90(value));
            }
        }
    }

    public void DeskMove()
    {
        Transform target = atDeskTransform;
        //This means we have clicked the arrow. The arrow should only show up if we're back at our desk. 
        //First, are we under the desk?
        if (underDesk)
        {
            //We are under the desk. So now we're moving back out of the desk.
            underDesk = false;
        }
        else
        {
            underDesk = true;
            target = underDeskTransform;
        }
        FindObjectOfType<ItemTracker>().ToggleColliders();

        rightArrow.SetActive(!underDesk);
        leftArrow.SetActive(!underDesk);
        
        StartCoroutine(Move(target));
    }

    IEnumerator Move(Transform target)
    {
        rotating = true;
        float timeElapsed = 0;
        
        while (timeElapsed < lerpDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target.position, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rotating = false;

        //Rotate to look at the desk, or down if we're back to desk facing
        StartCoroutine(underDesk ? RotateUpUnderDesk() : RotateDownUnderDesk());
    }

    IEnumerator Rotate90(int rotationValue)
    {
        rotating = true;
        float timeElapsed = 0;
        
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(0, 90 * rotationValue, 0);
        while (timeElapsed < lerpDuration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.rotation = targetRotation;
        rotating = false;
        
        //If the y position is now 0, rotate down.
        if (Camera.main.transform.rotation.y == 0)
        {
            StartCoroutine(RotateDown());
        }
    }
    
    IEnumerator RotateUp(int value)
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(-21, 0, 0);
        while (timeElapsed < lerpDuration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.rotation = targetRotation;
        rotating = false;
        atDesk = false;
        deskArrow.SetActive(false);
        
        //Now, start the coroutine to rotate the other way. 
        StartCoroutine(Rotate90(value));
    }

    //Rotate up to look at the carving under the desk
    IEnumerator RotateUpUnderDesk()
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(-24, 0, 0);
        while (timeElapsed < lerpDuration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.rotation = targetRotation;
        rotating = false;
    }

    //Rotate back down to look at the desk
    IEnumerator RotateDownUnderDesk()
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(21, 0, 0);
        while (timeElapsed < lerpDuration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.rotation = targetRotation;
        rotating = false;
    }
    
    //Happening when we're looking back at our desk. 
    IEnumerator RotateDown()
    {
        rotating = true;
        atDesk = true;
        deskArrow.SetActive(true);
        
        
        float timeElapsed = 0;
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(21, 0, 0);
        while (timeElapsed < lerpDuration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.rotation = targetRotation;
        rotating = false;
    }
}
