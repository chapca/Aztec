using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerBoss : MonoBehaviour
{
    static UIManagerBoss instance;

    static public GameObject UIManette, UIEsquiveRight, UIEsquiveLeft, UIBlock, UICounter, UIAttack, UICombo1, UICombo2, UICombo3, counterAttackParent;

    static public Image sliderEsquiveRight, sliderEsquiveLeft, sliderBlock, sliderCounter, sliderAttack;

    static public Image sliderAttackPerfect, sliderEsquivePerfectRight, sliderEsquivePerfectLeft, sliderCounterPerfect, sliderBlockPerfect, sliderUIPerfectCombo1, sliderUIPerfectCombo2, sliderUIPerfectCombo3;

    static public Image sliderLooseAttack, sliderLooseEsquiveRight, sliderLooseEsquiveLeft, sliderLooseBlock, sliderLooseCounter, sliderUILooseCombo1, sliderUILooseCombo2, sliderUILooseCombo3,
        sliderUILoose2Combo1, sliderUILoose2Combo2, sliderUILoose2Combo3;

    static public Image sliderBoss;

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

        UICombo1 = GameObject.Find("EmptyCombot1UIBoss");
        UICombo2 = GameObject.Find("EmptyCombot2UIBoss");
        UICombo3 = GameObject.Find("EmptyCombot3UIBoss");

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

        sliderUIPerfectCombo1 = GameObject.Find("SliderCombo1PerfectBoss").GetComponent<Image>();
        sliderUIPerfectCombo2 = GameObject.Find("SliderCombo2PerfectBoss").GetComponent<Image>();
        sliderUIPerfectCombo3 = GameObject.Find("SliderCombo3PerfectBoss").GetComponent<Image>();

        sliderLooseAttack = GameObject.Find("SliderAttackLooseBoss").GetComponent<Image>();
        sliderLooseEsquiveRight = GameObject.Find("SliderEsquiveLooseRightBoss").GetComponent<Image>();
        sliderLooseEsquiveLeft = GameObject.Find("SliderEsquiveLooseLeftBoss").GetComponent<Image>();
        sliderLooseBlock = GameObject.Find("SliderBlockLooseBoss").GetComponent<Image>();
        sliderLooseCounter = GameObject.Find("SliderCounterLooseBoss").GetComponent<Image>();

        sliderUILooseCombo1 = GameObject.Find("SliderCombo1LooseBoss").GetComponent<Image>();
        sliderUILooseCombo2 = GameObject.Find("SliderCombo2LooseBoss").GetComponent<Image>();
        sliderUILooseCombo3 = GameObject.Find("SliderCombo3LooseBoss").GetComponent<Image>();

        sliderUILoose2Combo1 = GameObject.Find("SliderCombo1Loose2Boss").GetComponent<Image>();
        sliderUILoose2Combo2 = GameObject.Find("SliderCombo2Loose2Boss").GetComponent<Image>();
        sliderUILoose2Combo3 = GameObject.Find("SliderCombo3Loose2Boss").GetComponent<Image>();

        counterAttackParent = GameObject.Find("EmptyCounterAttackBoss");

        sliderBoss = GameObject.Find("SliderHpBoss").GetComponent<Image>();

        UICombo1.SetActive(false);
        UICombo2.SetActive(false);
        UICombo3.SetActive(false);

        UIManette.SetActive(false);
        UIEsquiveRight.SetActive(false);
        UIEsquiveLeft.SetActive(false);
        UIBlock.SetActive(false);
        UICounter.SetActive(false);
        UIAttack.SetActive(false);

        sliderBoss.gameObject.SetActive(false);

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

    // slider final combo 1 
    static public void UpdateSliderLooseCombo1(float value)
    {
        sliderUILooseCombo1.fillAmount = value;
    }
    static public void UpdateSliderLoose2Combo1(float value)
    {
        sliderUILoose2Combo1.fillAmount = value;
    }
    static public void UpdateSliderPerfectCombo1(float value)
    {
        sliderUIPerfectCombo1.fillAmount = value;
    }

    // slider final combo 2
    static public void UpdateSliderLooseCombo2(float value)
    {
        sliderUILooseCombo2.fillAmount = value;
    }
    static public void UpdateSliderLoose2Combo2(float value)
    {
        sliderUILoose2Combo2.fillAmount = value;
    }
    static public void UpdateSliderPerfectCombo2(float value)
    {
        sliderUIPerfectCombo2.fillAmount = value;
    }

    // slider final combo 3
    static public void UpdateSliderLooseCombo3(float value)
    {
        sliderUILooseCombo3.fillAmount = value;
    }
    static public void UpdateSliderLoose2Combo3(float value)
    {
        sliderUILoose2Combo3.fillAmount = value;
    }
    static public void UpdateSliderPerfectCombo3(float value)
    {
        sliderUIPerfectCombo3.fillAmount = value;
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
                if(UIManette.activeInHierarchy)
                    ActiveManetteUI(false);

                UIBlock.SetActive(false);
            }
        }
    }

    static public void ActiveUIEsquive(bool active, bool right, bool resize)
    {
        if (active)
        {
            UIEsquiveRight.SetActive(right);
            UIEsquiveLeft.SetActive(!right);
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

                if (UIManette.activeInHierarchy)
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
                if (UIManette.activeInHierarchy)
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
                if (UIManette.activeInHierarchy)
                    ActiveManetteUI(false);
            }
        }
    }

    static public void ActiveUINbrCounterAttack(bool active, int nbrCoup)
    {
        Debug.LogError(active);

        if (!active)
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

    static public void ActiveUICombo1(bool active, bool resize)
    {
        if (active)
        {
            UICombo1.SetActive(true);
        }
        else
        {
            if (resize)
            {
                instance.DesableUselessUI(UIBlock, UIBlock, UIBlock, UIBlock, UIBlock);
                instance.CallCouroutineCombo(UICombo1, sliderUILoose2Combo1, sliderUILooseCombo1, sliderUIPerfectCombo1);
            }
            else
            {
                UICombo1.SetActive(false);
            }
        }
    }
    static public void ActiveUICombo2(bool active, bool resize)
    {
        if (active)
        {
            UICombo2.SetActive(true);
        }
        else
        {
            if (resize)
            {
                instance.DesableUselessUI(UIBlock, UIBlock, UIBlock, UIBlock, UIBlock);
                instance.CallCouroutineCombo(UICombo2, sliderUILoose2Combo2, sliderUILooseCombo2, sliderUIPerfectCombo2);
            }
            else
            {
                UICombo2.SetActive(false);
            }
        }
    }
    static public void ActiveUICombo3(bool active, bool resize)
    {
        if (active)
        {
            UICombo3.SetActive(true);
        }
        else
        {
            if (resize)
            {
                instance.DesableUselessUI(UIBlock, UIBlock, UIBlock, UIBlock, UIBlock);
                instance.CallCouroutineCombo(UICombo3, sliderUILoose2Combo3, sliderUILooseCombo3, sliderUIPerfectCombo3);
            }
            else
            {
                UICombo3.SetActive(false);
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

    void CallCouroutineCombo(GameObject uiObj, Image fail, Image perfect, Image fail2)
    {
        StartCoroutine(ResizeSliderCombo(uiObj, fail, perfect, fail2));
    }

    static public void ActiveComboUI(bool active)
    {
        /*UICombo1.SetActive(active);
        UICombo2.SetActive(active);
        UICombo3.SetActive(active);*/
        ActiveManetteUI(active);

        UIEsquiveRight.SetActive(!active);
        UIEsquiveLeft.SetActive(!active);
        UIBlock.SetActive(!active);
        UICounter.SetActive(!active);
        UIAttack.SetActive(!active);
    }

    static public void SliderBoss(float life)
    {
        Debug.LogError(life / 350);
        sliderBoss.gameObject.SetActive(true);
        sliderBoss.fillAmount = life/350;
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

            if (!Boss.esquivePerfect && !HPBoss.finalCombo)
            {
                ActiveManetteUI(false);
            }

            normal.transform.localScale = new Vector2(1f, 1f);
            perfect.transform.localScale = normal.transform.localScale;

            if (uiObj != UICounter)
                fail.transform.localScale = normal.transform.localScale;

            Boss.esquivePerfect = false;
            yield break;
        }
        else
        {
            StartCoroutine(ResizeSlider(uiObj, normal, perfect, fail));
            yield break;
        }
    }

    IEnumerator ResizeSliderCombo(GameObject uiObj, Image normal, Image perfect, Image fail)
    {
        if (normal.transform.localScale.x < 2)
        {
            normal.rectTransform.localScale = new Vector2(normal.rectTransform.localScale.x + 0.1f, normal.rectTransform.localScale.y + 0.1f);
            perfect.rectTransform.localScale = new Vector2(perfect.rectTransform.localScale.x + 0.1f, perfect.rectTransform.localScale.y + 0.1f);
            fail.rectTransform.localScale = new Vector2(fail.rectTransform.localScale.x + 0.1f, fail.rectTransform.localScale.y + 0.1f);
        }
        yield return new WaitForSeconds(0.01f);

        if (normal.transform.localScale.x >= 2)
        {
            uiObj.SetActive(false);

            normal.rectTransform.localScale = new Vector2(1f, 1f);
            perfect.rectTransform.localScale = normal.rectTransform.localScale;
            fail.rectTransform.localScale = normal.rectTransform.localScale;
           
            yield break;
        }
        else
        {
            StartCoroutine(ResizeSliderCombo(uiObj, normal, perfect, fail));
            yield break;
        }
    }
}