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
        
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
