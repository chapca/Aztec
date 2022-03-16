using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player, puzzle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (player.activeInHierarchy)
            {
                player.SetActive(false);
                puzzle.SetActive(true);
            }
            else
            {
                puzzle.SetActive(false);
                player.SetActive(true);
            }
        }
    }
}
