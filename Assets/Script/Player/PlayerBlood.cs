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

    static bool firstRecoverBlood;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine("GetBloodSmooth",100);
        myAnimator = GameObject.Find("PlayerAnim").GetComponent<Animator>();
        firstRecoverBlood = false;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
            // GetBlood(100);
        // }

        if (deadWastage)
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
        if(!firstRecoverBlood)
        {
            PlayerUI.ActiveTutoBlood(true);
            Time.timeScale = 0;
            firstRecoverBlood = true;
        }
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
        Debug.LogError("Launche coroutine get blood");
        StartCoroutine(GetBloodSmooth(blood));
        myAnimator.SetTrigger("RecoverBlood");
    }

    IEnumerator GetBloodSmooth(float blood)
    {
        bloodQuantity += 0.2f;
        Debug.LogError("coroutine blood " + bloodQuantity);
        recoveringBlood = true;

        if (bloodQuantity > 100)
            bloodQuantity = 100;

        PlayerUI.UpdateSliderBlood();
        yield return new WaitForSeconds(0.0005f);

        if (bloodQuantity >= blood)
        {
            recoveringBlood = false;
            StopCoroutine("GetBloodSmooth");
            // bloodQuantity = 100;
            //ForceGetBlood(bloodQuantity + blood);
            yield break;
            
        }
        else
        {
            StartCoroutine(GetBloodSmooth(blood));
        }
    }
}