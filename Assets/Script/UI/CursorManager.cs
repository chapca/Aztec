using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    static public bool followTarget, desable;

    static public GameObject objectToFollow;

    static Camera cam;

    static Transform thisTansform;

    static Image imageCursorActive;

    // Start is called before the first frame update
    void Start()
    {
        imageCursorActive = GetComponent<Image>();

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        thisTansform = transform;
    }


    // Update is called once per frame
    void Update()
    {
        if(followTarget)
        {
            FollowTarget();
        }
        else
        {
            Disable();
        }
    }

    static public void Disable()
    {
        imageCursorActive.enabled = false;
        thisTansform.localPosition = new Vector3(-883, 465,0);
    }

    static public void FollowTarget()
    {
        imageCursorActive.enabled = true;
        Vector3 newPos = cam.WorldToScreenPoint(objectToFollow.transform.position);
        thisTansform.position = newPos;

    }
}
