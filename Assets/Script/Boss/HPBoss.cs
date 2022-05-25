using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBoss : MonoBehaviour
{
    public int maxHp, hp, nbrBlood;
    Boss boss;
    Animator myAnimator;
    Transform parentObject;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();

        myAnimator = GetComponent<Animator>();

        hp = maxHp;

        parentObject = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            transform.parent = null;
            boss.enabled = false;
            PlayerBlood.GetBlood(nbrBlood);
            myAnimator.SetBool("Die", true);
            PlayerController.ennemi = null;
        }
    }
}