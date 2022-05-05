using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlood : MonoBehaviour
{
    [Range(0,100)]
    static public float bloodQuantity, halfBloodQuantity;

    public static bool deadWastage;
    void Start()
    {
    }

    private void Update()
    {
        if(deadWastage)
        {
            if(bloodQuantity > halfBloodQuantity)
            {
                bloodQuantity -= Time.deltaTime*20f;
                PlayerUI.UpdateSliderBlood();
            }
            else
            {
                deadWastage = false;
            }
        }
        else
        {
            halfBloodQuantity = (int)bloodQuantity / 2;
        }
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