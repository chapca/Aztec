using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlood : MonoBehaviour
{
    static PlayerBlood instance;

    [Range(0,100)]
    static public float bloodQuantity, halfBloodQuantity;

    public static bool deadWastage;

    private void Awake()
    {
        instance = this;
    }

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
        instance.LaunchGetBloodCoroutine(blood);
    }

    static public void ForceGetBlood(float blood)
    {
        bloodQuantity = blood;
    }

    static public void LooseBlood(float blood)
    {
        bloodQuantity -= blood;
        PlayerUI.UpdateSliderBlood();
    }

    void LaunchGetBloodCoroutine(float blood)
    {
        StartCoroutine(GetBloodSmooth(blood));
    }

    IEnumerator GetBloodSmooth(float blood)
    {
        Debug.LogError("GetBlood");

        bloodQuantity += 0.1f;

        if (bloodQuantity > 100)
            bloodQuantity = 100;

        PlayerUI.UpdateSliderBlood();
        yield return new WaitForSeconds(0.001f);

        if (bloodQuantity >= 100)
        {
            bloodQuantity = 100;
            yield break;
        }
        else
        {
            StartCoroutine(GetBloodSmooth(blood));
        }
    }
}