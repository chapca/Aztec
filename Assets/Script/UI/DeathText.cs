using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathText : MonoBehaviour
{
    static public Text thisText;

    static float r, g, b, a;

    static bool makeTxtVisible;

    void Start()
    {
        thisText = GetComponent<Text>();

        r = thisText.color.r;
        g = thisText.color.g;
        b = thisText.color.b;
        a = 0;

        thisText.color = new Color(r, g, b, a);
    }

    // Update is called once per frame
    void Update()
    {
        if (makeTxtVisible)
            MakeTextVisible();
    }

    public static void ActiveText()
    {
        thisText.color = new Color(r, g, b, a);
        makeTxtVisible = true;
    }

    void MakeTextVisible()
    {
        if (a >= 1)
            makeTxtVisible = false;
        else
        {
            a += Time.deltaTime;
            thisText.color = new Color(r, g, b, a);
        }
    }
}
