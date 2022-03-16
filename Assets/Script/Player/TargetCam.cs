using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCam : MonoBehaviour
{
    [SerializeField] float rotationX, rotSpeed, clampAxeMin, clampAxeMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationX -= rotSpeed * Time.deltaTime * Input.GetAxis("RightJoystickY");
        rotationX = Mathf.Clamp(rotationX, clampAxeMin, clampAxeMax);
        transform.localRotation = Quaternion.Euler(rotationX, transform.parent.rotation.y, 0);
    }
}
