using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    static public bool followTarget, desable;

    // Start is called before the first frame update
    void Start()
    {
        
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

    }

    static public void FollowTarget()
    {

    }
}
