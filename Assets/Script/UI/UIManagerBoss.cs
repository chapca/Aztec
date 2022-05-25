using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerBoss : MonoBehaviour
{
    static UIManagerBoss instance;

    static public GameObject UIManette, UIEsquiveRight, UIEsquiveLeft, UIBlock, UICounter, UIAttack;

    static public Image sliderEsquiveRight, sliderEsquiveLeft, sliderBlock, sliderCounter, sliderAttack;

    static public Image sliderAttackPerfect, sliderEsquivePerfectRight, sliderEsquivePerfectLeft, sliderCounterPerfect, sliderBlockPerfect;

    static public Image sliderLooseAttack, sliderLooseEsquiveRight, sliderLooseEsquiveLeft, sliderLooseBlock, sliderLooseCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManette = GameObject.Find("ManetteSpriteBoss");

        UIEsquiveRight = GameObject.Find("EmptyEsquiveUIRightBoss");
        UIEsquiveLeft = GameObject.Find("EmptyEsquiveUILeftBoss");

        UIBlock = GameObject.Find("EmptyBlockUIBoss");
        UICounter = GameObject.Find("EmptyCounterUIBoss");
        UIAttack = GameObject.Find("EmptyAttackUIBoss");

        sliderEsquiveRight = GameObject.Find("SliderEsquiveRightBoss").GetComponent<Image>();
        sliderEsquiveLeft = GameObject.Find("SliderEsquiveLeftBoss").GetComponent<Image>();

        sliderBlock = GameObject.Find("SliderBlockBoss").GetComponent<Image>();
        sliderCounter = GameObject.Find("SliderCounterBoss").GetComponent<Image>();
        sliderAttack = GameObject.Find("SliderAttackBoss").GetComponent<Image>();

        sliderAttackPerfect = GameObject.Find("SliderAttackPerfectBoss").GetComponent<Image>();
        sliderEsquivePerfectRight = GameObject.Find("SliderEsquivePerfectRightBoss").GetComponent<Image>();
        sliderEsquivePerfectLeft = GameObject.Find("SliderEsquivePerfectLeftBoss").GetComponent<Image>();
        sliderCounterPerfect = GameObject.Find("SliderCounterPerfectBoss").GetComponent<Image>();
        sliderBlockPerfect = GameObject.Find("SliderBlockPerfectBoss").GetComponent<Image>();

        sliderLooseAttack = GameObject.Find("SliderAttackLooseBoss").GetComponent<Image>();
        sliderLooseEsquiveRight = GameObject.Find("SliderEsquiveLooseRightBoss").GetComponent<Image>();
        sliderLooseEsquiveLeft = GameObject.Find("SliderEsquiveLooseLeftBoss").GetComponent<Image>();
        sliderLooseBlock = GameObject.Find("SliderBlockLooseBoss").GetComponent<Image>();
        sliderLooseCounter = GameObject.Find("SliderCounterLooseBoss").GetComponent<Image>();

        UIManette.SetActive(false);
        UIEsquiveRight.SetActive(false);
        UIEsquiveLeft.SetActive(false);
        UIBlock.SetActive(false);
        UICounter.SetActive(false);
        UIAttack.SetActive(false);

        sliderLooseEsquiveRight.transform.localRotation = sliderLooseEsquiveLeft.transform.localRotation;

    }

    //slider :

    static public void UpdateSliderEsquive(float value)
    {
        sliderEsquiveRight.fillAmount = value;
        sliderEsquiveLeft.fillAmount = value;
    }
    static public void UpdateSliderEsquivePerfect(float value)
    {
        sliderEsquivePerfectRight.fillAmount = value;
        sliderEsquivePerfectLeft.fillAmount = value;
    }
    static public void UpdateSliderEsquiveLoose(float value)
    {
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
    }
    static public void UpdateSliderCounterLoose(float value)
    {
        sliderLooseCounter.fillAmount = value;
    }

    static public void UpdateSliderAttack(float value)
    {
        sliderAttack.fillAmount = value;
    }
    static public void UpdateSliderAttackPerfect(float value)
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
            if (resize)
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
        if (active)
        {
            UIEsquiveRight.SetActive(true);
            UIEsquiveLeft.SetActive(true);
        }
        else
        {
            if (resize)
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
            if (resize)
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
            if (resize)
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
            if (uiObj != UICounter)
                fail.transform.localScale = normal.transform.localScale;
        }
        yield return new WaitForSeconds(0.01f);

        if (normal.transform.localScale.x >= 2)
        {
            uiObj.SetActive(false);

            if (!EnnemiAttack.esquivePerfect)
                ActiveManetteUI(false);

            normal.transform.localScale = new Vector2(1f, 1f);
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
}