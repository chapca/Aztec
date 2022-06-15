using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone2 : MonoBehaviour
{
    [SerializeField] CamFocusObjectif camFocusObjectif;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            camFocusObjectif.playerGetInZone2 = true;
        }
    }
}
