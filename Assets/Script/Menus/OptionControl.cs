using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

        ChangeInputAxes();

        if (Input.GetAxis("HorizontalLeftButtonX") == 0)
            canSwitchOnglet = true;

        if (Input.GetAxis("HorizontalLeftButtonX") != 0 && canSwitchOnglet)
        {
            transform.parent.transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.transform.GetChild(1).gameObject.SetActive(true);

            canSwitchOnglet = false;
        }
    }

    void ChangeInputAxes()
    {
        if (m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex() == 1)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Cam�ra X          < Normal >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Cam�ra X          < Invers� >";
                    PlayerController.CamXInverser = true;
                }
                else if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Cam�ra X          < Invers� >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Cam�ra X          < Normal >";
                    PlayerController.CamXInverser = false;
                }
            }
        }

        if (m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex() == 2)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Cam�ra Y          < Normal >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Cam�ra Y          < Invers� >";
                    PlayerController.CamYInverser = true;
                }
                else if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Cam�ra Y          < Invers� >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Cam�ra Y          < Normal >";
                    PlayerController.CamYInverser = false;
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

    public void GoToSandboxScene()
    {
        SceneManager.LoadScene("SandBox");
    }
}