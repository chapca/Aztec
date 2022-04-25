using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interaction : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] TargetCam targetCam;

    [SerializeField] GameObject canvasPuzzle;

    [SerializeField] CinemachineVirtualCamera camBase, camInteraction;

    [SerializeField] bool camIsZooming, camIsAdjuting;

    [SerializeField] float speedTransitionCam;

    [SerializeField] bool activeCamInteraction;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //canvasPuzzle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (camIsAdjuting)
        {
            SetCamPosition();
        }

        if(camIsZooming)
        {
            camInteraction.Priority = 10;
            camIsZooming = false;
        }

        if(activeCamInteraction)
        {
            playerController.enabled = false;
            targetCam.enabled = false;
            camIsAdjuting = true;

            if(Input.GetButtonDown("CancelButton"))
            {
                EnableCamInteraction(false);
            }
        }
        else
        {
            playerController.enabled = true;
            targetCam.enabled = true;
            camInteraction.Priority = 9;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            UIManager.ActiveManetteInputInteract(true);

            if (PlayerBlood.bloodQuantity < 100)
            {
                UIManager.ActiveTextCantInteract(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (PlayerBlood.bloodQuantity >= 100)
                {
                    UIManager.ActiveManetteInputInteract(false);
                    StartCoroutine(OpenDoor(other));
                    PlayerBlood.LooseBlood(100);
                    EnableCamInteraction(true);
                    playerController.enabled = false;
                    targetCam.enabled = false;
                    camIsAdjuting = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            UIManager.ActiveTextCantInteract(false);
            UIManager.ActiveManetteInputInteract(false);
        }
    }

    void SetCamPosition()
    {
        targetCam.transform.localRotation = Quaternion.Slerp(targetCam.transform.localRotation, Quaternion.Euler(Vector3.zero), speedTransitionCam);

        if(targetCam.transform.localRotation == Quaternion.Euler(Vector3.zero))
        {
            camIsAdjuting = false;
            camIsZooming = true;
        }
    }

    public void EnableCamInteraction(bool activate)
    {
        activeCamInteraction = activate;
    }

    IEnumerator OpenDoor(Collider other)
    {
        yield return new WaitForSeconds(3f);
        other.transform.GetComponent<Levier>().isActive = true;
        yield break;
    }
}