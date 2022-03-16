using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interaction : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] TargetCam targetCam;

    [SerializeField] GameObject canvasPuzzle;

    [SerializeField] CinemachineVirtualCamera camBase, camPuzzle;

    [SerializeField] bool camIsZooming, camIsAdjuting;

    [SerializeField] float speedTransitionCam;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //canvasPuzzle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxisRaw("VerticalLeftButtonY"));

        if(canvasPuzzle.activeInHierarchy)
        {
            if(Input.GetButtonDown("CancelButton"))
            {
                canvasPuzzle.SetActive(false);
                playerController.enabled = true;
                targetCam.enabled = true;
                camPuzzle.Priority = 9;
            }
        }

        if(camIsAdjuting)
        {
            SetCamPosition();
        }

        if(camIsZooming)
        {
            camPuzzle.Priority = 10;
            camIsZooming = false;
            StartCoroutine("ActivePuzzle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ActivePuzzle"))
        {
            playerController.enabled = false;
            targetCam.enabled = false;
            camIsAdjuting = true;
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

    IEnumerator ActivePuzzle()
    {
        yield return new WaitForSeconds(2f);
        canvasPuzzle.SetActive(true);
        yield break;
    }
}
