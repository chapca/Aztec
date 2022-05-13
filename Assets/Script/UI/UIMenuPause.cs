using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UIMenuPause : MonoBehaviour
{
    public static bool menuPauseIsActive;

    EventSystem m_EventSystem;

    [SerializeField] GameObject buttonResume, buttonOption, buttonQuit;

    [SerializeField] GameObject menuPause, optionMenu;
    // Start is called before the first frame update
    void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            Time.timeScale = 0;
            transform.GetChild(0).gameObject.SetActive(true);
            m_EventSystem.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(2).gameObject);
            Debug.Log(m_EventSystem.currentSelectedGameObject);
            menuPauseIsActive = true;
        }

        if(optionMenu.activeInHierarchy && Input.GetButtonDown("CancelButton"))
        {
            optionMenu.SetActive(false);
            EnableButton(buttonQuit, buttonResume, true);
            m_EventSystem.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(2).gameObject);
        }
        else if(!optionMenu.activeInHierarchy && menuPause.activeInHierarchy && Input.GetButtonDown("CancelButton"))
        {
            menuPause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ClickOption()
    {
        optionMenu.SetActive(true);
        EnableButton(buttonResume, buttonQuit, false);

    }

    void EnableButton(GameObject button1, GameObject button2, bool active)
    {
        button1.SetActive(active);
        button2.SetActive(active);
    }

    public void ResumeButton()
    {
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