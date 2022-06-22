using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interaction : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] TargetCam targetCam;

    [SerializeField] CinemachineVirtualCamera camBase, camInteraction;

    [SerializeField] ElevatorState elevatorState;

    [SerializeField] bool camIsZooming;

    [SerializeField] float speedTransitionCam;

    [SerializeField] bool activeCamInteraction, elevatorHasBeenActive;

    [SerializeField] GameObject bouttonHaut, bigBouttonHaut;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //canvasPuzzle.SetActive(false);

        elevatorState = GameObject.FindWithTag("Elevator").GetComponentInParent<ElevatorState>();

        bouttonHaut = GameObject.Find("BouttonHaut");
        bigBouttonHaut = GameObject.Find("BigBouttonHaut");
        bigBouttonHaut.SetActive(false);
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
            else
            {
                bigBouttonHaut.SetActive(true);
                bouttonHaut.SetActive(false);
            }
        }

        if (other.CompareTag("Elevator"))
        {
            UIManager.ActiveManetteInputInteract(true);

            if (PlayerBlood.bloodQuantity < 100 && !elevatorHasBeenActive)
            {
                UIManager.ActiveTextCantInteract(true);
            }
            if (PlayerBlood.bloodQuantity >= 100 || elevatorHasBeenActive)
            {
                bigBouttonHaut.SetActive(true);
                bouttonHaut.SetActive(false);
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
                    bigBouttonHaut.SetActive(true);
                    bouttonHaut.SetActive(false);
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
                if(!elevatorHasBeenActive)
                {
                    if (PlayerBlood.bloodQuantity >= 100)
                    {
                        playerController.enabled = false;
                        other.transform.parent.GetComponent<Animator>().SetBool("Down", false);
                        other.transform.parent.GetComponent<Animator>().SetBool("Up", true);
                        UIManager.ActiveManetteInputInteract(false);
                        PlayerBlood.LooseBlood(100);
                        elevatorHasBeenActive = true;
                        /*SoundManager.PlaySoundPlayerInteraction(other.GetComponent<AudioSource>(), SoundManager.soundAndVolumeListInteractionStatic[0]);
                        SoundManager.PlaySoundPlayerInteraction(other.transform.parent.transform.GetChild(1).GetComponent<AudioSource>(), SoundManager.soundAndVolumeListInteractionStatic[1]);*/
                    }
                }
                else
                {
                    if (elevatorState.up)
                    {
                        other.transform.parent.GetComponent<Animator>().SetBool("Down", true);
                        other.transform.parent.GetComponent<Animator>().SetBool("Up", false);
                    }
                    else
                    {
                        other.transform.parent.GetComponent<Animator>().SetBool("Down", false);
                        other.transform.parent.GetComponent<Animator>().SetBool("Up", true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            bigBouttonHaut.SetActive(false);
            bouttonHaut.SetActive(true);

            UIManager.ActiveTextCantInteract(false);
            UIManager.ActiveManetteInputInteract(false);
        }
        if (other.CompareTag("End"))
        {
            UIManager.ActiveManetteInputInteractLeaveGame(false);
        }

        if (other.CompareTag("Elevator"))
        {
            bigBouttonHaut.SetActive(false);
            bouttonHaut.SetActive(true);

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