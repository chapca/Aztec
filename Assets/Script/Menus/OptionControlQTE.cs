using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionControlQTE : MonoBehaviour
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
        Debug.Log(listButton.GetChild(0).gameObject);
        m_EventSystem.SetSelectedGameObject(listButton.GetChild(0).gameObject);
    }

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            SwitchSprite();
        }

        if (Input.GetAxis("HorizontalLeftButtonX") == 0)
            canSwitchOnglet = true;

        if (Input.GetAxis("HorizontalLeftButtonX") != 0 && canSwitchOnglet)
        {
            transform.parent.transform.GetChild(1).gameObject.SetActive(false);
            transform.parent.transform.GetChild(0).gameObject.SetActive(true);
            canSwitchOnglet = false;
        }

        if (Input.GetButtonDown("CancelButton"))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void SwitchSprite()
    {
        for (int i = 0; i < listSprite.transform.childCount; i++)
        {
            if (i == m_EventSystem.currentSelectedGameObject.transform.GetSiblingIndex())
                listSprite.transform.GetChild(i).gameObject.SetActive(true);
            else
            {
                listSprite.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}