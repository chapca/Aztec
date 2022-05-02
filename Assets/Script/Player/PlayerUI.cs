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
    // Start is called before the first frame update
    void Start()
    {
        textHP = transform.Find("TextHp").gameObject.GetComponent<Text>();
        sliderHP = transform.Find("SliderHp").gameObject.GetComponent<Image>();

        textBlood = transform.Find("TextBlood").gameObject.GetComponent<Text>();
        sliderBlood = transform.Find("SliderBlood").gameObject.GetComponent<Image>();

        sliderBloodPuzzle = GameObject.Find("SliderBlood").gameObject.GetComponent<Image>();

        UpdateSliderHp();
        UpdateSliderBlood();
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
}