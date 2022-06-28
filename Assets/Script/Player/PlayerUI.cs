using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    static Text textHP;
    static Image sliderHP;

    static Text textBlood;
    static Image sliderBlood;

    static Image sliderBloodPuzzle;

    static public GameObject healthButton, tutoBlood;

    static public Image healthInput;

    // Start is called before the first frame update
    void Start()
    {
        textHP = transform.Find("TextHp").gameObject.GetComponent<Text>();
        sliderHP = GameObject.Find("SliderHp").gameObject.GetComponent<Image>();

        textBlood = transform.Find("TextBlood").gameObject.GetComponent<Text>();
        sliderBlood = GameObject.Find("SliderBlood").gameObject.GetComponent<Image>();

        sliderBloodPuzzle = GameObject.Find("SliderBlood").gameObject.GetComponent<Image>();

        healthInput = GameObject.Find("HealthInput").GetComponent<Image>();

        healthButton = transform.Find("HealthButton").gameObject;

        tutoBlood = transform.Find("TutoBlood").gameObject;

        ActiveTutoBlood(false);
        UpdateSliderHp();
        UpdateSliderBlood();
    }

    private void Update()
    {
        if(tutoBlood.activeInHierarchy)
        {
            if(Input.anyKeyDown)
            {
                ActiveTutoBlood(false);
                Time.timeScale = 1;
            }
        }
    }

    static public void UpdateSliderHp()
    {
        textHP.text = PlayerHp.hp.ToString();
        sliderHP.fillAmount = PlayerHp.hp / 100;
    }

    static public void UpdateSliderBlood()
    {
        textBlood.text = PlayerBlood.bloodQuantity.ToString();
        sliderBlood.fillAmount = PlayerBlood.bloodQuantity/100;
        sliderBloodPuzzle.fillAmount = PlayerBlood.bloodQuantity/100; 
    }

    static public void ActiveUIHealthButton(bool active)
    {
        healthButton.SetActive(active);
    }

    static public void ActiveTutoBlood(bool active)
    {
        tutoBlood.SetActive(active);
    }

    static public void ActiveHealthinteraction(bool active)
    {
        if(active)
            healthInput.color = Color.red;
        else
            healthInput.color = Color.white;
    }
}