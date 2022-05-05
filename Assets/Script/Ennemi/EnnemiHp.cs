using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiHp : MonoBehaviour
{
    public int hp, nbrBlood;
    EnnemiAttack ennemiAttack;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        ennemiAttack = GetComponent<EnnemiAttack>();
        myAnimator = GetComponent<Animator>();
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
            transform.parent = null;
            ennemiAttack.enabled = false;
            PlayerBlood.GetBlood(nbrBlood);
            myAnimator.SetBool("Die", true);
        }
    }
}