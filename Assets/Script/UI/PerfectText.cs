using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectText : MonoBehaviour
{
    static Animator animator;

    static GameObject thisGameObject;

    void Start()
    {
        animator = GetComponent<Animator>();
        thisGameObject = gameObject;

        thisGameObject.SetActive(false);
    }

    public static void ActiveText()
    {
        thisGameObject.SetActive(true);
        animator.enabled = true;
    }

    void DesableText()
    {
        animator.enabled = false;
        thisGameObject.SetActive(false);
    }
}