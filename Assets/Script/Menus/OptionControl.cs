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
                if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Caméra X          < Normal >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Caméra X          < Inversé >";
                    PlayerController.CamXInverser = true;
                }
                else if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Caméra X          < Inversé >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Caméra X          < Normal >";
                    PlayerController.CamXInverser = false;
                }
            }
        }

        if (m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex() == 2)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Caméra Y          < Normal >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Caméra Y          < Inversé >";
                    PlayerController.CamYInverser = true;
                }
                else if (m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == "Caméra Y          < Inversé >")
                {
                    m_EventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text = "Caméra Y          < Normal >";
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