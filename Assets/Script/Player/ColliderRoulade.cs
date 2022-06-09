using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRoulade : MonoBehaviour
{
    [SerializeField] bool right, left;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (right)
            {
                Battle.wallDetectRight = true;
            }
            if (left)
            {
                Battle.wallDetectLeft = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (right)
            {
                Battle.wallDetectRight = false;
            }
            if (left)
            {
                Battle.wallDetectLeft = false;
            }
        }
    }
}
