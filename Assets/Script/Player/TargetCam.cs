using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCam : MonoBehaviour
{
    [Header("Rotation sur l'axe X")]
    [SerializeField] float rotSpeed, rotationX, clampAxeMin, clampAxeMax;

    [Header("Rotation sur l'axe Y")]
    [SerializeField] float rotationY;

    [Header("Valeur d'interpolation entre 0 et 1 qui fait revenir la camera à sa rotation de base")]
    [SerializeField] float returnRotBase;

    public static bool canTurnCamAroundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationX -= rotSpeed * Time.deltaTime * Input.GetAxis("RightJoystickY");
        rotationX = Mathf.Clamp(rotationX, clampAxeMin, clampAxeMax);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);

        if(canTurnCamAroundPlayer)
        {
            rotationY += rotSpeed * Time.deltaTime * Input.GetAxis("RightJoystickX");
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
        else
        {
            rotationY = Mathf.Lerp(rotationY, 0, returnRotBase);
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}
