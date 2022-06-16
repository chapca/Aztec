using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailText : MonoBehaviour
{
    static Animator animator;

    static GameObject thisGameObject;

    static Image image;

    static Color color;

    [Range(0,1)]
    static float alpha;

    void Start()
    {
        animator = GetComponent<Animator>();
        thisGameObject = gameObject;

        thisGameObject.SetActive(false);

        image = GetComponent<Image>();

        color = Color.white;

        alpha = 1;

        color.a = alpha;

        image.color = color; 
    }

    public static void ActiveText()
    {
        alpha = 1f;
        color.a = alpha;
        image.color = color;

        thisGameObject.SetActive(true);
        animator.enabled = true;
    }

    void DesableText()
    {
        StartCoroutine("MakeTransparence");
    }

    IEnumerator MakeTransparence()
    {
        alpha -= 0.1f;
        color.a = alpha;
        image.color = color;
        yield return new WaitForSeconds(0.01f);

        if(alpha <=0)
        {
            animator.enabled = false;
            thisGameObject.SetActive(false);
            yield break;
        }
        else
        {
            StartCoroutine("MakeTransparence");
        }
    }
}