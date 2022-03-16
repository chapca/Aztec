using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    static public float hp = 100;

    void Start()
    {
        //hp = 100;
    }

    void Update()
    {
    }

    public static void TakeDamage(float damage)
    {
        hp -= damage;
        PlayerUI.UpdateSliderHp();
    }
}