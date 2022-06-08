using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlood : MonoBehaviour
{
    static PlayerBlood instance;

    static Animator myAnimator;

    [Range(0,100)]
    static public float bloodQuantity, halfBloodQuantity;

    public static bool deadWastage, recoveringBlood;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        myAnimator = GameObject.Find("PlayerAnim").GetComponent<Animator>();
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
        myAnimator.SetTrigger("RecoverBlood");
    }

    IEnumerator GetBloodSmooth(float blood)
    {
        bloodQuantity += 0.2f;
        recoveringBlood = true;

        if (bloodQuantity > 100)
            bloodQuantity = 100;

        PlayerUI.UpdateSliderBlood();
        yield return new WaitForSeconds(0.001f);

        if (bloodQuantity >= 100)
        {
            recoveringBlood = false;
            bloodQuantity = 100;
            yield break;
        }
        else
        {
            StartCoroutine(GetBloodSmooth(blood));
        }
    }
}