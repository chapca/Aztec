using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectText : MonoBehaviour
{
    static public Text thisText;

    static float r, g, b, a;

    static bool makeTransparent;
    // Start is called before the first frame update
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
        if (makeTransparent)
            MakeTextTRandparent();
    }

    public static void ActiveText()
    {
        a = 1;
        thisText.color = new Color(r, g, b, a);
        makeTransparent = true;
    }

    void MakeTextTRandparent()
    {
        if (a <= 0)
            makeTransparent = false;

        a -= Time.deltaTime;
        thisText.color = new Color(r, g, b, a);
    }
}