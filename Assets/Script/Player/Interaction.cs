using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interaction : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] TargetCam targetCam;

    [SerializeField] CinemachineVirtualCamera camBase, camInteraction;

    [SerializeField] bool camIsZooming;

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
        if(activeCamInteraction)
        {
            playerController.enabled = false;
            targetCam.enabled = false;
            camInteraction.Priority = 11;
        }
        else
        {
            playerController.enabled = true;
            targetCam.enabled = true;

            if(camInteraction != null)
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

        if (other.CompareTag("Elevator"))
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
            if (Input.GetButtonDown("InteractButton"))
            {
                if (PlayerBlood.bloodQuantity >= 100)
                {
                    UIManager.ActiveManetteInputInteract(false);
                    StartCoroutine("OpenDoor");
                    PlayerBlood.LooseBlood(100);
                    other.GetComponent<Animator>().enabled = true;
                    EnableCamInteraction(true);
                    playerController.enabled = false;
                    targetCam.enabled = false;
                    camInteraction = other.transform.Find("Cam").GetComponent<CinemachineVirtualCamera>();

                    SoundManager.PlaySoundPlayerInteraction(other.GetComponent<AudioSource>(), SoundManager.soundAndVolumeListInteractionStatic[0]);
                    //SoundManager.PlaySoundPlayerInteraction(other.transform.parent.transform.GetChild(1).GetComponent<AudioSource>(), SoundManager.soundAndVolumeListInteractionStatic[1]);
                }
            }
        }
        if (other.CompareTag("Elevator"))
        {
            if (Input.GetButtonDown("InteractButton"))
            {
                if (PlayerBlood.bloodQuantity >= 100)
                {
                    other.transform.parent.GetComponent<Animator>().enabled = true;
                    UIManager.ActiveManetteInputInteract(false);
                    PlayerBlood.LooseBlood(100);

                    /*SoundManager.PlaySoundPlayerInteraction(other.GetComponent<AudioSource>(), SoundManager.soundAndVolumeListInteractionStatic[0]);
                    SoundManager.PlaySoundPlayerInteraction(other.transform.parent.transform.GetChild(1).GetComponent<AudioSource>(), SoundManager.soundAndVolumeListInteractionStatic[1]);*/
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
        if (other.CompareTag("End"))
        {
            UIManager.ActiveManetteInputInteractLeaveGame(false);
        }

        if (other.CompareTag("Elevator"))
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
            camIsZooming = true;
        }
    }

    public void EnableCamInteraction(bool activate)
    {
        activeCamInteraction = activate;
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(3f);
        EnableCamInteraction(false);
        yield break;
    }
}