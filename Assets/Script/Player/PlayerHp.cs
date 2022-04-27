using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    static public float hp = 50;

    [SerializeField] Battle battle;

    void Start()
    {
        //hp = 100;
    }

    void Update()
    {
        if (Input.GetButton("HealthButton") && hp < 100 && PlayerBlood.bloodQuantity >0 && !battle.isAttacked)
        {
            ManualHealth();
        }
    }

    public static void TakeDamage(float damage)
    {
        hp -= damage;
        PlayerUI.UpdateSliderHp();
    }

    public static void ManualHealth()
    {
        hp += Time.deltaTime * 5f;
        PlayerUI.UpdateSliderHp();
        PlayerBlood.LooseBlood(Time.deltaTime * 10f);
    }
}