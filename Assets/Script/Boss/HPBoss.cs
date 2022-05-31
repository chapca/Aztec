using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBoss : MonoBehaviour
{
    public float maxHp, hp, nbrBlood;
    Boss boss;
    Animator myAnimator;
    Transform parentObject;

    Battle battle;

    public static bool startFinalCombo, finalCombo;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();

        myAnimator = GetComponent<Animator>();

        hp = maxHp;

        parentObject = transform.parent;

        battle = GameObject.FindWithTag("Player").GetComponent<Battle>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if(hp <=0)
        {
            hp = 0;
            startFinalCombo = true;
        }

        if (hp <= 0 && finalCombo)
        {
            transform.parent = null;
            boss.enabled = false;
            PlayerBlood.GetBlood(nbrBlood);
            myAnimator.SetBool("Die", true);
            PlayerController.ennemi = null;
            battle.isAttacked = false;
        }
    }
}