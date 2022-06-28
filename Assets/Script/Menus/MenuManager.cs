using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Index")]
    [SerializeField] int gameSceneIndex;
    [SerializeField] int optionSceneIndex;
    [SerializeField] int archivesSceneIndex;

    [Header("ControllerTool")]
    [SerializeField] List<GameObject> ButonsList = new List<GameObject>();

    EventSystem m_EventSystem;
    [SerializeField] Transform btnParent;

    // Start is called before the first frame update
    void Start()
    {
        m_EventSystem = EventSystem.current;
        StartCoroutine("SelectFirstButton");
    }

    IEnumerator SelectFirstButton()
    {
        yield return new WaitForEndOfFrame();
        btnParent.GetChild(0).transform.GetComponent<Button>().Select();
        btnParent.parent.transform.Find("Image").gameObject.SetActive(true);
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
}
