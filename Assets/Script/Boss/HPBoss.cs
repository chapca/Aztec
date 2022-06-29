using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HPBoss : MonoBehaviour
{
    public float maxHp, hp, nbrBlood;
    Boss boss;
    Animator myAnimator;
    Transform parentObject;

    Battle battle;

    public static bool startFinalCombo, finalCombo;

    public bool bossIsDead;

    void Start()
    {
        boss = GetComponent<Boss>();

        myAnimator = GetComponent<Animator>();

        hp = maxHp;

        parentObject = transform.parent;

        battle = GameObject.FindWithTag("Player").GetComponent<Battle>();

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        UIManagerBoss.SliderBoss(hp);

        if (hp <=0)
        {
            hp = 0;
            startFinalCombo = true;
        }
    }

    public void BossDeath()
    {
        AnimationEvent.bossFight = false;
        transform.parent = null;
        boss.enabled = false;
        PlayerBlood.GetBlood(nbrBlood);
        myAnimator.SetBool("Die", true);
        PlayerController.ennemi = null;
        battle.isAttacked = false;
        bossIsDead = true;

        StartCoroutine("LoadMenu");
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(3);
        yield break;
    }
}