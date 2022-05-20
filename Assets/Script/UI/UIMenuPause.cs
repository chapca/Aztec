using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMenuPause : MonoBehaviour
{
    public static bool menuPauseIsActive;

    EventSystem m_EventSystem;

    [SerializeField] GameObject objButtonResume, objButtonOption, objButtonQuit;

    [SerializeField] GameObject canvasSlider, menuPause, optionMenu;

    [SerializeField] Battle battle;

    [SerializeField] Button buttonResume, buttonRestart, buttonOption, buttonQuit;

    GameObject currentButtonSelected;
    // Start is called before the first frame update
    void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause") && !battle.degaine)
        {
            Time.timeScale = 0;
            canvasSlider.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            m_EventSystem.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(2).gameObject);
            Debug.Log(m_EventSystem.currentSelectedGameObject);
            menuPauseIsActive = true;
        }

        if(optionMenu.activeInHierarchy && Input.GetButtonDown("CancelButton"))
        {
            optionMenu.SetActive(false);
            EnableButton(objButtonQuit, objButtonResume, true);
            m_EventSystem.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(2).gameObject);
        }
        else if(!optionMenu.activeInHierarchy && menuPause.activeInHierarchy && Input.GetButtonDown("CancelButton"))
        {
            canvasSlider.SetActive(true);
            menuPause.SetActive(false);
            Time.timeScale = 1;
        }


        if(m_EventSystem.currentSelectedGameObject == buttonResume.gameObject)
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

        if(m_EventSystem.currentSelectedGameObject == buttonRestart.gameObject)
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

    public void ClickOption()
    {
        optionMenu.SetActive(true);
        EnableButton(objButtonResume, objButtonQuit, false);
    }

    void EnableButton(GameObject button1, GameObject button2, bool active)
    {
        button1.SetActive(active);
        button2.SetActive(active);
    }

    public void ResumeButton()
    {
        canvasSlider.SetActive(true);
        menuPause.SetActive(false);

        Time.timeScale = 1;
    }
    
    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("AlphaScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}