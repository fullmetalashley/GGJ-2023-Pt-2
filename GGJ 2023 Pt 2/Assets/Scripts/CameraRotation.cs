using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float speed;
    float lerpDuration = 0.5f;
    bool rotating;
    public void RotateLeft()
    {
        //Camera.main.transform.Rotate(new Vector3(0, 90, 0));
    }
    
    public void RotateRight()
    {
        //Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, new Vector3(0, 90, 0), Time.deltaTime*speed);
    }


    public void Rotate()
    {
        StartCoroutine(Rotate90());
    }
    
    
    IEnumerator Rotate90()
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(0, 90, 0);
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
