using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFocusObjectif : MonoBehaviour
{
    [SerializeField] GameObject camZone1, camZone2;

    [SerializeField] bool camZone1Active, camZone2Active;

    [SerializeField] PlayerController playerController;

    public bool playerGetInZone2;

    void Update()
    {
        if(!camZone1Active)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                playerController.lookZone = true;
                camZone1.SetActive(true);
                camZone1Active = true;
                StartCoroutine("StopLook");
            }
        }

        if (!camZone2Active)
        {
            if (playerGetInZone2)
            {
                playerController.lookZone = true;
                camZone2.SetActive(true);
                camZone2Active = true;
                StartCoroutine("StopLook");
            }
        }
    }

    IEnumerator StopLook()
    {
        yield return new WaitForSeconds(2f);
        playerController.lookZone = false;
        camZone1.SetActive(false);
        camZone2.SetActive(false);
    }
}