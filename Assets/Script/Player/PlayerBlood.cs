using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlood : MonoBehaviour
{
    static public float bloodQuantity; 

    void Start()
    {
        GetBlood(100);
    }

    static public void GetBlood(float blood)
    {
        bloodQuantity += blood;
        PlayerUI.UpdateSliderBlood();
    }

    static public void LooseBlood(float blood)
    {
        bloodQuantity -= blood;
        PlayerUI.UpdateSliderBlood();
    }
}