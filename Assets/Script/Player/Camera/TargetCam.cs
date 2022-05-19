using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCam : MonoBehaviour
{
    [Header("Rotation sur l'axe X")]
    [SerializeField] float rotSpeedX, rotSpeedY, rotationX, clampAxeMin, clampAxeMax;

    [Header("Rotation sur l'axe Y")]
    [SerializeField] float rotationY;

    [Header("Valeur d'interpolation entre 0 et 1 qui fait revenir la camera à sa rotation de base")]
    [SerializeField] float returnRotBase;

    public static bool canTurnCamAroundPlayer;

    [SerializeField] AnimationCurve animCurve;

    [SerializeField] float maxOffsetCam;

    float baseOffset;

    [SerializeField] CinemachineVirtualCamera cam1;

    Cinemachine3rdPersonFollow camBaseThirdPersonFollow;

    Transform follow;

    [SerializeField] Transform targetCamPlayer;

    // Start is called before the first frame update
    void Start()
    {
        follow = cam1.Follow;

        camBaseThirdPersonFollow = cam1.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        baseOffset = camBaseThirdPersonFollow.CameraDistance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = targetCamPlayer.position;

        Debug.Log(canTurnCamAroundPlayer);

        Rotation();

        ZoomOverRotation();
    }

    void Rotation()
    {
        if (PlayerController.CamYInverser)
        {
            rotationY -= rotSpeedY * Time.deltaTime * Input.GetAxis("RightJoystickX");
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
        else
        {
            rotationY += rotSpeedY * Time.deltaTime * Input.GetAxis("RightJoystickX");
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
        /*else
        {
            rotationY = Mathf.Lerp(rotationY, 0, returnRotBase);
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }*/

        if (PlayerController.CamXInverser)
        {
            rotationX -= rotSpeedX * Time.deltaTime * Input.GetAxis("RightJoystickY");
            rotationX = Mathf.Clamp(rotationX, clampAxeMin, clampAxeMax);
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
        else
        {
            rotationX += rotSpeedX * Time.deltaTime * Input.GetAxis("RightJoystickY");
            rotationX = Mathf.Clamp(rotationX, clampAxeMin, clampAxeMax);
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }

    void ZoomOverRotation()
    {
        float yDiff = Mathf.Abs(follow.position.y - cam1.transform.position.y);

        float ratio = animCurve.Evaluate(Mathf.Clamp01(yDiff / maxOffsetCam));

        camBaseThirdPersonFollow.CameraDistance = baseOffset * ratio;
    }
}