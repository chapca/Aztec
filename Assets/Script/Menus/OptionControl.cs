using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionControl : MonoBehaviour
{
    EventSystem m_EventSystem;

    [SerializeField] Transform listButton, listSprite;

    bool canSwitchOnglet;
    void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    private void OnEnable()
    {
        m_EventSystem.SetSelectedGameObject(listButton.GetChild(0).gameObject);
    }

    void Update()
    {
        if(Input.GetAxisRaw("Vertical") !=0)
        {
            SwitchSprite();
        }

        if(m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex() ==1)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if(m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Cam�ra           < Normal >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Cam�ra           < Invers� >";
                }
                else if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Cam�ra           < Invers� >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Cam�ra           < Normal >";
                }
            }
        }

        if (Input.GetAxis("HorizontalLeftButtonX") == 0)
            canSwitchOnglet = true;

        if (Input.GetAxis("HorizontalLeftButtonX") != 0 && canSwitchOnglet)
        {
            transform.parent.transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.transform.GetChild(1).gameObject.SetActive(true);

            canSwitchOnglet = false;
        }
    }

    void SwitchSprite()
    {
        for(int i=0; i < listSprite.transform.childCount; i++)
        {
            if(i == m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex())
                listSprite.transform.GetChild(i).gameObject.SetActive(true);
            else
            {
                listSprite.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}