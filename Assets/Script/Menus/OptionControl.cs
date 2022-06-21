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

    [SerializeField] Text buttonInverseX, buttonInverseY;

    bool canSwitchOnglet;

    void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    private void OnEnable()
    {
        StartCoroutine("SelectFirstButton");
    }

    IEnumerator SelectFirstButton()
    {
        //m_EventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        //m_EventSystem.SetSelectedGameObject(listButton.GetChild(0).gameObject);
        listButton.GetChild(0).transform.GetComponent<Button>().Select();
        SwitchSprite();
        yield break;
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

        if(Input.GetButtonDown("CancelButton"))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void ChangeInputAxes()
    {
        if (m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex() == 1)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (m_EventSystem.currentSelectedGameObject.transform.Find("Normal").gameObject.activeInHierarchy)
                {
                    m_EventSystem.currentSelectedGameObject.transform.Find("Normal").gameObject.SetActive(false);
                    m_EventSystem.currentSelectedGameObject.transform.Find("Reverse").gameObject.SetActive(true);
                    PlayerController.CamXInverser = true;
                }
                else if (m_EventSystem.currentSelectedGameObject.transform.Find("Reverse").gameObject.activeInHierarchy)
                {
                    m_EventSystem.currentSelectedGameObject.transform.Find("Reverse").gameObject.SetActive(false);
                    m_EventSystem.currentSelectedGameObject.transform.Find("Normal").gameObject.SetActive(true);
                    PlayerController.CamXInverser = false;
                }
            }
        }

        if (m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex() == 2)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (m_EventSystem.currentSelectedGameObject.transform.Find("Normal").gameObject.activeInHierarchy)
                {
                    m_EventSystem.currentSelectedGameObject.transform.Find("Normal").gameObject.SetActive(false);
                    m_EventSystem.currentSelectedGameObject.transform.Find("Reverse").gameObject.SetActive(true);
                    PlayerController.CamYInverser = true;
                }
                else if (m_EventSystem.currentSelectedGameObject.transform.Find("Reverse").gameObject.activeInHierarchy)
                {
                    m_EventSystem.currentSelectedGameObject.transform.Find("Reverse").gameObject.SetActive(false);
                    m_EventSystem.currentSelectedGameObject.transform.Find("Normal").gameObject.SetActive(true);
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
            {
                listSprite.transform.GetChild(i).gameObject.SetActive(true);
                listButton.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                listButton.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
                listButton.GetChild(i).transform.GetComponent<Button>().Select();
            }
            else
            {
                listSprite.transform.GetChild(i).gameObject.SetActive(false);
                listButton.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                listButton.GetChild(i).transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }

    public void GoToSandboxScene()
    {
        SceneManager.LoadScene("AlphaScene");
    }
}