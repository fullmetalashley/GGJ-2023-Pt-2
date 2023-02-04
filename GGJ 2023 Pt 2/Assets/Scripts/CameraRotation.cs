using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float lerpDuration = 0.5f;
    bool rotating;
    private bool atDesk;

    public void Rotate(int value)
    {
        //Index -1 is the left button.
        //Index 1 is the right button.

        //First, check what direction we're facing.
        //What is our current rotation value? 
        if (!rotating){
            if (Camera.main.transform.rotation.y == 0)
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
        
        //Now, start the coroutine to rotate the other way. 
        StartCoroutine(Rotate90(value));
    }
    
    IEnumerator RotateDown()
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
}
