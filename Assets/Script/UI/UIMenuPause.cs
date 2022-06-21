using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class UIMenuPause : MonoBehaviour
{
    public static bool menuPauseIsActive;

    EventSystem m_EventSystem;

    [SerializeField] GameObject objButtonResume, objButtonOption, objButtonQuit, objButtonRestart, backGround;

    [SerializeField] GameObject canvasSlider, menuPause, optionMenu;

    [SerializeField] Battle battle;

    [SerializeField] Button buttonResume, buttonRestart, buttonOption, buttonQuit;

    GameObject currentButtonSelected;

    [SerializeField] VolumeProfile mVolumeProfile;
    [SerializeField] DepthOfField depthOfField;

    Animator playerAnimator;
     
    void Start()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();

        m_EventSystem = EventSystem.current;

        depthOfField = (DepthOfField)mVolumeProfile.components[8];
        depthOfField.active = false;
    }


    void Update()
    {
        if(Input.GetButtonDown("Pause") && !battle.degaine)
        {
            playerAnimator.enabled = false;
            Time.timeScale = 0;
            canvasSlider.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            m_EventSystem.SetSelectedGameObject(objButtonResume);
            depthOfField.active = true;
            Debug.Log(m_EventSystem.currentSelectedGameObject);
            menuPauseIsActive = true;
        }

        if(optionMenu.activeInHierarchy && Input.GetButtonDown("CancelButton"))
        {
            depthOfField.active = false;
            optionMenu.SetActive(false);
            EnableButton(objButtonQuit, objButtonResume, objButtonRestart, backGround, true);
            m_EventSystem.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(2).gameObject);
        }
        else if(!optionMenu.activeInHierarchy && menuPause.activeInHierarchy && Input.GetButtonDown("CancelButton"))
        {
            depthOfField.active = false;
            canvasSlider.SetActive(true);
            menuPause.SetActive(false);
            Time.timeScale = 1;
            playerAnimator.enabled = true;
        }

        if (m_EventSystem.currentSelectedGameObject != null)
        {
            if (m_EventSystem.currentSelectedGameObject == buttonResume.gameObject)
            {
                buttonResume.transform.Find("TxtSelect").gameObject.SetActive(true);
                buttonResume.transform.Find("Selection").gameObject.SetActive(true);
                buttonResume.transform.Find("TxtUnselect").gameObject.SetActive(false);
            }
            else
            {
                buttonResume.transform.Find("TxtUnselect").gameObject.SetActive(true);
                buttonResume.transform.Find("TxtSelect").gameObject.SetActive(false);
                buttonResume.transform.Find("Selection").gameObject.SetActive(false);
            }

            if (m_EventSystem.currentSelectedGameObject == buttonRestart.gameObject)
            {
                buttonRestart.transform.Find("TxtSelect").gameObject.SetActive(true);
                buttonRestart.transform.Find("Selection").gameObject.SetActive(true);
                buttonRestart.transform.Find("TxtUnselect").gameObject.SetActive(false);
            }
            else
            {
                buttonRestart.transform.Find("TxtUnselect").gameObject.SetActive(true);
                buttonRestart.transform.Find("TxtSelect").gameObject.SetActive(false);
                buttonRestart.transform.Find("Selection").gameObject.SetActive(false);
            }

            if (m_EventSystem.currentSelectedGameObject == buttonOption.gameObject)
            {
                buttonOption.transform.Find("TxtSelect").gameObject.SetActive(true);
                buttonOption.transform.Find("Selection").gameObject.SetActive(true);
                buttonOption.transform.Find("TxtUnselect").gameObject.SetActive(false);
            }
            else
            {
                buttonOption.transform.Find("TxtUnselect").gameObject.SetActive(true);
                buttonOption.transform.Find("TxtSelect").gameObject.SetActive(false);
                buttonOption.transform.Find("Selection").gameObject.SetActive(false);
            }

            if (m_EventSystem.currentSelectedGameObject == buttonQuit.gameObject)
            {
                buttonQuit.transform.Find("TxtSelect").gameObject.SetActive(true);
                buttonQuit.transform.Find("Selection").gameObject.SetActive(true);
                buttonQuit.transform.Find("TxtUnselect").gameObject.SetActive(false);
            }
            else
            {
                buttonQuit.transform.Find("TxtUnselect").gameObject.SetActive(true);
                buttonQuit.transform.Find("Selection").gameObject.SetActive(false);
                buttonQuit.transform.Find("TxtSelect").gameObject.SetActive(false);
            }
        }
        else
        {
            m_EventSystem.SetSelectedGameObject(objButtonResume);
        }
    }

    public void ClickOption()
    {
        optionMenu.SetActive(true);
        EnableButton(objButtonResume, objButtonQuit, objButtonRestart, backGround, false);
    }

    void EnableButton(GameObject button1, GameObject button2, GameObject button3, GameObject backGround, bool active)
    {
        button1.SetActive(active);
        button2.SetActive(active);
        button3.SetActive(active);
        backGround.SetActive(active);
    }

    public void ResumeButton()
    {
        depthOfField.active = false;
        canvasSlider.SetActive(true);
        menuPause.SetActive(false);

        Time.timeScale = 1;
        playerAnimator.enabled = false;
    }
    
    public void RestartButton()
    {
        Time.timeScale = 1;
        PlayerBlood.ForceGetBlood(0);
        UIManagerBoss.ActiveManetteUI(false);
        SceneManager.LoadScene("BetaScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}