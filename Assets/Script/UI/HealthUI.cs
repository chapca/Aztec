using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    static Animator animator;
    static GameObject thisGameObject;

    void Start()
    {
        animator = GetComponent<Animator>();

        thisGameObject = gameObject;

        thisGameObject.SetActive(false);
    }

    public static void ActiveUIAnim(bool active)
    {
        if(active)
        {
            thisGameObject.SetActive(true);
            animator.enabled = true;
        }
        else
        {
            thisGameObject.SetActive(false);
            animator.enabled = false;
        }
    }
}