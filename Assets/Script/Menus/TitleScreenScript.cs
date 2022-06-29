using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour
{

    [SerializeField] int mainMenueIndex;
    [SerializeField] int endTimer;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= endTimer)
        {
            SceneManager.LoadScene(mainMenueIndex);
        }
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenueIndex);
        }
    }
}
