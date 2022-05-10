using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuPause : MonoBehaviour
{
    public static bool menuPauseIsActive;

    EventSystem m_EventSystem;

    [SerializeField] GameObject buttonResume, buttonOption, buttonQuit;

    [SerializeField] GameObject optionMenu;
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
            m_EventSystem.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(1).gameObject);
            Debug.Log(m_EventSystem.currentSelectedGameObject);
            menuPauseIsActive = true;
        }
    }

    public void ClickOption()
    {
        optionMenu.SetActive(true);
        EnableButton(buttonResume, buttonQuit, null, false);

    }

    void EnableButton(GameObject button1, GameObject button2, GameObject button3, bool active)
    {
        button1.SetActive(active);
        button2.SetActive(active);
        button3.SetActive(active);
    }
}
