using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public GameObject UIManette, UIEsquiveRight, UIEsquiveLeft, UIBlock, UICounter, UIAttack;

    static public Image sliderEsquiveRight, sliderEsquiveLeft, sliderBlock, sliderCounter, sliderAttack;

    static public Image sliderAttackPerfect, sliderEsquivePerfectRight, sliderEsquivePerfectLeft, sliderCounterPerfect, sliderBlockPerfect;

    static public Text textCantinteract;

    static public GameObject manetteValidateInteraction;

    // Start is called before the first frame update
    void Start()
    {
        UIManette = GameObject.Find("ManetteSprite");

        UIEsquiveRight = GameObject.Find("EmptyEsquiveUIRight");
        UIEsquiveLeft = GameObject.Find("EmptyEsquiveUILeft");

        UIBlock = GameObject.Find("EmptyBlockUI");
        UICounter = GameObject.Find("EmptyCounterUI");
        UIAttack = GameObject.Find("EmptyAttackUI");

        sliderEsquiveRight = GameObject.Find("SliderEsquiveRight").GetComponent<Image>();
        sliderEsquiveLeft = GameObject.Find("SliderEsquiveLeft").GetComponent<Image>();

        sliderBlock = GameObject.Find("SliderBlock").GetComponent<Image>();
        sliderCounter = GameObject.Find("SliderCounter").GetComponent<Image>();
        sliderAttack = GameObject.Find("SliderAttack").GetComponent<Image>();

        sliderAttackPerfect = GameObject.Find("SliderAttackPerfect").GetComponent<Image>();
        sliderEsquivePerfectRight = GameObject.Find("SliderEsquivePerfectRight").GetComponent<Image>();
        sliderEsquivePerfectLeft = GameObject.Find("SliderEsquivePerfectLeft").GetComponent<Image>();
        sliderCounterPerfect = GameObject.Find("SliderCounterPerfect").GetComponent<Image>();
        sliderBlockPerfect = GameObject.Find("SliderBlockPerfect").GetComponent<Image>();


        textCantinteract = GameObject.Find("TextCantInteract").GetComponent<Text>();

        manetteValidateInteraction = GameObject.Find("ManetteValidateInteraction");

        UIManette.SetActive(false);
        UIEsquiveRight.SetActive(false);
        UIEsquiveLeft.SetActive(false);
        UIBlock.SetActive(false);
        UICounter.SetActive(false);
        UIAttack.SetActive(false);

        textCantinteract.gameObject.SetActive(false);
        manetteValidateInteraction.gameObject.SetActive(false);

    }

    //slider :

    static public void UpdateSliderEsquive(float value)
    {
        sliderEsquiveRight.fillAmount = value;
        sliderEsquiveLeft.fillAmount = value;
    } static public void UpdateSliderEsquivePerfect(float value)
    {
        sliderEsquivePerfectRight.fillAmount = value;
        sliderEsquivePerfectLeft.fillAmount = value;
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



    static public void ActiveManetteUI(bool active)
    {
        if (active)
        {
            UIManette.SetActive(true);
        }
        else
        {
            UIManette.SetActive(false);
        }
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
            UIEsquiveRight.SetActive(true);
            UIEsquiveLeft.SetActive(true);
        }
        else
        {
            UIEsquiveRight.SetActive(false);
            UIEsquiveLeft.SetActive(false);
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



    // text info 

    static public void ActiveTextCantInteract(bool active)
    {
        textCantinteract.gameObject.SetActive(active);
    }
    static public void ActiveManetteInputInteract(bool active)
    {
        manetteValidateInteraction.gameObject.SetActive(active);
    }
}