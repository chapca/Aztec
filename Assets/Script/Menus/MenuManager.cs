using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Index")]
    [SerializeField] int gameSceneIndex;
    //[SerializeField] int optionSceneIndex;
    //[SerializeField] int archivesSceneIndex;

    [Header("ControllerTool")]
    [SerializeField] List<GameObject> ButonsList = new List<GameObject>();

    EventSystem m_EventSystem;
    [SerializeField] Transform btnParent;

    [Header("Canvas")]

    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject archivesCanvas;
    [SerializeField] GameObject optionsCanvas;
    //[SerializeField] EventSystem eventSystem;

    [Header("Vcams")]
    
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;
    [SerializeField] CinemachineVirtualCamera vcam3;

    [SerializeField] GameObject firstSelected;

    [SerializeField] Light dL;
    // Start is called before the first frame update
    void Start()
    {
        dL.colorTemperature = 2588;
        mainCanvas.SetActive(true);
        archivesCanvas.SetActive(false);
        //optionsCanvas.SetActive(false);
        vcam1.Priority = 10;
        vcam2.Priority = 0;
        vcam3.Priority = 0;
        m_EventSystem = EventSystem.current;
        StartCoroutine("SelectFirstButton");
    }

    IEnumerator SelectFirstButton()
    {
        yield return new WaitForEndOfFrame();
        btnParent.GetChild(0).transform.GetComponent<Button>().Select();
        //btnParent.parent.transform.Find("Image").gameObject.SetActive(true);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ButonsList.Count; i++)
        {
            if (ButonsList[i] != m_EventSystem.currentSelectedGameObject)
            {
                ButonsList[i].transform.Find("Image").gameObject.SetActive(false);
            }
            else
            {
                ButonsList[i].transform.Find("Image").gameObject.SetActive(true);

            }
        }

        if(Input.GetButtonDown("CancelButton"))
        {
            GoMain();
        }

        if(m_EventSystem.currentSelectedGameObject == null)
        {
            Debug.Log("vbilvjnqblnb kn");
            m_EventSystem.SetSelectedGameObject(firstSelected);
        }
    }

    #region ButonsFonctions
    public void Play()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }
    public void Options()
    {
        SceneManager.LoadScene("TestOptionController");
    }
    public void Archives()
    {
        SceneManager.LoadScene("PropsMenue");
    }
    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region ChangeCanvas

    public void GoArchives()
    {
        mainCanvas.SetActive(false);
        archivesCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
        //optionsCanvas.enabled = false;
        vcam1.Priority = 0;
        vcam2.Priority = 10;
        vcam3.Priority = 0;
        dL.colorTemperature = 7400;
    }
    public void GoOptions()
    {
        mainCanvas.SetActive(false);
        archivesCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        vcam1.Priority = 0;
        vcam2.Priority = 0;
        vcam3.Priority = 10;
    }
    public void GoMain()
    {   
        m_EventSystem.SetSelectedGameObject(firstSelected);
        mainCanvas.SetActive(true);
        archivesCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        dL.colorTemperature = 2588;
        vcam1.Priority = 10;
        vcam2.Priority = 0;
        vcam3.Priority = 0;
    }
    #endregion
}
