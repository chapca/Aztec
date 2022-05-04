using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionControl : MonoBehaviour
{
    EventSystem m_EventSystem;

    [SerializeField] Transform listButton, listSprite;

    void Start()
    {
        m_EventSystem = EventSystem.current;

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
                if(m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Caméra           < Normal >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Caméra           < Inversé >";
                }
                else if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Caméra           < Inversé >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Caméra           < Normal >";
                }
            }
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