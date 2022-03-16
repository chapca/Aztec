using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public GameObject UIEsquive, UIBlock, UICounter, UIAttack;

    static public Image sliderEsquive, sliderBlock, sliderCounter, sliderAttack;

    static public Image sliderAttackPerfect, sliderEsquivePerfect, sliderCounterPerfect, sliderBlockPerfect;

    // Start is called before the first frame update
    void Start()
    {
        UIEsquive = GameObject.Find("EmptyEsquiveUI");
        UIBlock = GameObject.Find("EmptyBlockUI");
        UICounter = GameObject.Find("EmptyCounterUI");
        UIAttack = GameObject.Find("EmptyAttackUI");

        sliderEsquive = GameObject.Find("SliderEsquive").GetComponent<Image>();
        sliderBlock = GameObject.Find("SliderBlock").GetComponent<Image>();
        sliderCounter = GameObject.Find("SliderCounter").GetComponent<Image>();
        sliderAttack = GameObject.Find("SliderAttack").GetComponent<Image>();

        sliderAttackPerfect = GameObject.Find("SliderAttackPerfect").GetComponent<Image>();
        sliderEsquivePerfect = GameObject.Find("SliderEsquivePerfect").GetComponent<Image>();
        sliderCounterPerfect = GameObject.Find("SliderCounterPerfect").GetComponent<Image>();
        sliderBlockPerfect = GameObject.Find("SliderBlockPerfect").GetComponent<Image>();

        UIEsquive.SetActive(false);
        UIBlock.SetActive(false);
        UICounter.SetActive(false);
        UIAttack.SetActive(false);
    }

    static public void UpdateSliderEsquive(float value)
    {
        sliderEsquive.fillAmount = value;
    } static public void UpdateSliderEsquivePerfect(float value)
    {
        sliderEsquivePerfect.fillAmount = value;
    }

    static public void UpdateSliderBlock(float value)
    {
        sliderBlock.fillAmount = value;
    }
    static public void UpdateSliderBlockPerfect(float value)
    {
        sliderBlockPerfect.fillAmount = value;
    }

    static public void UpdateSliderCounter(float value)
    {
        sliderCounter.fillAmount = value;
    }static public void UpdateSliderCounterPerfect(float value)
    {
        sliderCounterPerfect.fillAmount = value;
    }

    static public void UpdateSliderAttack(float value)
    {
        sliderAttack.fillAmount = value;
    } static public void UpdateSliderAttackPerfect(float value)
    {
        sliderAttackPerfect.fillAmount = value;
    }



    static public void ActiveUIBlock(bool active)
    {
        if (active)
        {
            UIBlock.SetActive(true);
        }
        else
        {
            UIBlock.SetActive(false);
        }
    }

    static public void ActiveUIEsquive(bool active)
    {
        if(active)
        {
            UIEsquive.SetActive(true);
        }
        else
        {
            UIEsquive.SetActive(false);
        }
    }

    static public void ActiveUICounter(bool active)
    {
        if (active)
        {
            UICounter.SetActive(true);
        }
        else
        {
            UICounter.SetActive(false);
        }
    }

    static public void ActiveUIAttack(bool active)
    {
        if (active)
        {
            UIAttack.SetActive(true);
        }
        else
        {
            UIAttack.SetActive(false);
        }
    }
}