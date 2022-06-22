using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    static public GameObject UIManette, UIEsquiveRight, UIEsquiveLeft, UIBlock, UICounter, UIAttack;

    static public Image sliderEsquiveRight, sliderEsquiveLeft, sliderBlock, sliderCounter, sliderAttack;

    static public Image sliderAttackPerfect, sliderEsquivePerfectRight, sliderEsquivePerfectLeft, sliderCounterPerfect, sliderBlockPerfect;

    static public Image sliderLooseAttack, sliderLooseEsquiveRight, sliderLooseEsquiveLeft, sliderLooseBlock, sliderLooseCounter;

    static public Text textCantinteract;

    static public GameObject counterAttackParent, cursor;

    private void Awake()
    {
        instance = this;
    }

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

        sliderLooseAttack = GameObject.Find("SliderAttackLoose").GetComponent<Image>();
        sliderLooseEsquiveRight = GameObject.Find("SliderEsquiveLooseRight").GetComponent<Image>();
        sliderLooseEsquiveLeft = GameObject.Find("SliderEsquiveLooseLeft").GetComponent<Image>();
        sliderLooseBlock = GameObject.Find("SliderBlockLoose").GetComponent<Image>();
        sliderLooseCounter = GameObject.Find("SliderCounterLoose").GetComponent<Image>();

        textCantinteract = GameObject.Find("TextCantInteract").GetComponent<Text>();

        counterAttackParent = GameObject.Find("EmptyCounterAttack");

        cursor = GameObject.Find("Cursor");

        UIManette.SetActive(false);
        UIEsquiveRight.SetActive(false);
        UIEsquiveLeft.SetActive(false);
        UIBlock.SetActive(false);
        UICounter.SetActive(false);
        UIAttack.SetActive(false);

        textCantinteract.gameObject.SetActive(false);
        cursor.SetActive(false);

        sliderLooseEsquiveRight.transform.localRotation = sliderLooseEsquiveLeft.transform.localRotation;
        sliderEsquivePerfectRight.transform.localRotation = sliderEsquivePerfectLeft.transform.localRotation;

    }

    //slider :

    static public void AjusteSliderEsquive()
    {
        sliderLooseEsquiveRight.transform.localRotation = sliderLooseEsquiveLeft.transform.localRotation;
        sliderEsquivePerfectRight.transform.localRotation = sliderEsquivePerfectLeft.transform.localRotation;
    }

    static public void UpdateSliderEsquive(float value)
    {
        sliderLooseEsquiveRight.transform.localRotation = sliderLooseEsquiveLeft.transform.localRotation;
        sliderEsquivePerfectRight.transform.localRotation = sliderEsquivePerfectLeft.transform.localRotation;

        sliderEsquiveRight.fillAmount = value;
        sliderEsquiveLeft.fillAmount = value;
    } static public void UpdateSliderEsquivePerfect(float value)
    {
        sliderLooseEsquiveRight.transform.localRotation = sliderLooseEsquiveLeft.transform.localRotation;
        sliderEsquivePerfectRight.transform.localRotation = sliderEsquivePerfectLeft.transform.localRotation;

        sliderEsquivePerfectRight.fillAmount = value;
        sliderEsquivePerfectLeft.fillAmount = value;
    }
    static public void UpdateSliderEsquiveLoose(float value)
    {
        sliderLooseEsquiveRight.transform.localRotation = sliderLooseEsquiveLeft.transform.localRotation;
        sliderEsquivePerfectRight.transform.localRotation = sliderEsquivePerfectLeft.transform.localRotation;

        sliderLooseEsquiveRight.fillAmount = value;
        sliderLooseEsquiveLeft.fillAmount = value;
    }

    static public void UpdateSliderBlock(float value)
    {
        sliderBlock.fillAmount = value;
    }
    static public void UpdateSliderBlockPerfect(float value)
    {
        sliderBlockPerfect.fillAmount = value;
    }
    static public void UpdateSliderBlockLoose(float value)
    {
        sliderLooseBlock.fillAmount = value;
    }

    static public void UpdateSliderCounter(float value)
    {
        sliderCounter.fillAmount = value;
    }static public void UpdateSliderCounterLoose(float value)
    {
        sliderLooseCounter.fillAmount = value;
    }

    static public void UpdateSliderAttack(float value)
    {
        sliderAttack.fillAmount = value;
    } static public void UpdateSliderAttackPerfect(float value)
    {
        sliderAttackPerfect.fillAmount = value;
    }
    static public void UpdateSliderAttackLoose(float value)
    {
        Debug.Log(sliderLooseAttack.fillAmount);
        sliderLooseAttack.fillAmount = value;
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

    static public void ActiveUIBlock(bool active, bool resize)
    {
        if (active)
        {
            UIBlock.SetActive(true);
        }
        else
        {
            if(resize)
            {
                instance.DesableUselessUI(UIAttack, UIAttack, UIEsquiveRight, UIEsquiveLeft, UICounter);
                instance.CallCoroutine(UIBlock, sliderBlock, sliderBlockPerfect, sliderLooseBlock);
            }
            else
            {
                ActiveManetteUI(false);
                UIBlock.SetActive(false);
            }
        }
    }

    static public void ActiveUIEsquive(bool active, bool right, bool resize)
    {
        if(active)
        {
            UIEsquiveRight.SetActive(right);
            UIEsquiveLeft.SetActive(!right);
        }
        else
        {
            if(resize)
            {
                instance.DesableUselessUI(UIAttack, UIBlock, UIAttack, UIAttack, UICounter);

                if (right)
                {
                    UIEsquiveLeft.SetActive(false);
                    instance.CallCoroutine(UIEsquiveRight, sliderEsquiveRight, sliderEsquivePerfectRight, sliderLooseEsquiveRight);
                }
                if (!right)
                {
                    UIEsquiveRight.SetActive(false);
                    instance.CallCoroutine(UIEsquiveLeft, sliderEsquiveLeft, sliderEsquivePerfectLeft, sliderLooseEsquiveLeft);
                }
            }
            else
            {
                UIEsquiveLeft.SetActive(false);
                UIEsquiveRight.SetActive(false);
                ActiveManetteUI(false);
            }
        }
    }

    static public void ActiveUICounter(bool active, bool resize)
    {
        if (active)
        {
            UICounter.SetActive(true);
        }
        else
        {
            if(resize)
            {
                instance.DesableUselessUI(UIBlock, UIBlock, UIEsquiveRight, UIEsquiveLeft, UIBlock);
                instance.CallCoroutine(UICounter, sliderCounter, sliderCounterPerfect, null);
            }
            else
            {
                UICounter.SetActive(false);
                ActiveManetteUI(false);
            }
        }
    }

    static public void ActiveUIAttack(bool active, bool resize)
    {
        if (active)
        {
            UIAttack.SetActive(true);
        }
        else
        {
            if(resize)
            {
                instance.DesableUselessUI(UIBlock, UIBlock, UIEsquiveRight, UIEsquiveLeft, UICounter);
                instance.CallCoroutine(UIAttack, sliderAttack, sliderAttackPerfect, sliderLooseAttack);
            }
            else
            {
                UIAttack.SetActive(false);
                ActiveManetteUI(false);
            }
        }
    }

    static public void ActiveUINbrCounterAttack(bool active, int nbrCoup)
    {
        Debug.LogError(active);

        if(!active)
        {
            counterAttackParent.transform.GetChild(0).gameObject.SetActive(false);
            counterAttackParent.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            switch (nbrCoup)
            {
                case 0:
                    counterAttackParent.transform.GetChild(0).gameObject.SetActive(false);
                    counterAttackParent.transform.GetChild(1).gameObject.SetActive(false);
                    break;
                case 1:
                    counterAttackParent.transform.GetChild(0).gameObject.SetActive(true);
                    counterAttackParent.transform.GetChild(1).gameObject.SetActive(false);
                    break;
                case 2:
                    counterAttackParent.transform.GetChild(0).gameObject.SetActive(false);
                    counterAttackParent.transform.GetChild(1).gameObject.SetActive(true);
                    break;
            }
        }
    }

    void DesableUselessUI(GameObject uiAttack, GameObject uIBlock, GameObject uIEsquiveRight, GameObject uIEsquiveLeft, GameObject uiCounter)
    {
        uiAttack.SetActive(false);
        uIBlock.SetActive(false);
        uIEsquiveRight.SetActive(false);
        uIEsquiveLeft.SetActive(false);
        uiCounter.SetActive(false);

    }

    void CallCoroutine(GameObject uiObj, Image normal, Image perfect, Image fail)
    {
        StartCoroutine(ResizeSlider(uiObj, normal, perfect, fail));
    }

    IEnumerator ResizeSlider(GameObject uiObj, Image normal, Image perfect, Image fail)
    {
        if (normal.transform.localScale.x < 2)
        {
            normal.transform.localScale = new Vector2(normal.transform.localScale.x + 0.1f, normal.transform.localScale.y + 0.1f);
            perfect.transform.localScale = normal.transform.localScale;
            if(uiObj != UICounter)
                fail.transform.localScale = normal.transform.localScale;
        }
        yield return new WaitForSeconds(0.01f);

        if (normal.transform.localScale.x >= 2)
        {
            uiObj.SetActive(false);

            if(!EnnemiAttack.esquivePerfect)
                ActiveManetteUI(false);

            normal.transform.localScale = new Vector2(1f,1f);
            perfect.transform.localScale = normal.transform.localScale;
            if (uiObj != UICounter)
                fail.transform.localScale = normal.transform.localScale;

            EnnemiAttack.esquivePerfect = false;
            yield break;
        }
        else
        {
            StartCoroutine(ResizeSlider(uiObj, normal, perfect, fail));
            yield break;
        }
    }

    // text info 

    static public void ActiveTextCantInteract(bool active)
    {
        textCantinteract.gameObject.SetActive(active);
    }
    static public void ActiveManetteInputInteract(bool active)
    {
       /* if (!active)
            cursor.SetActive(false);
        else
            cursor.SetActive(true);*/
    }
    
    static public void ActiveManetteInputInteractLeaveGame(bool active)
    {
       /* if (!active)
            cursor.SetActive(false);
        else
            cursor.SetActive(true);*/
    }
}