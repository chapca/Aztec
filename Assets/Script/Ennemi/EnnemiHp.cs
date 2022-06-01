using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiHp : MonoBehaviour
{
    public int maxHp, hp, nbrBlood;
    EnnemiAttack ennemiAttack;
    Animator myAnimator;
    Transform parentObject;

    // Start is called before the first frame update
    void Start()
    {
        ennemiAttack = GetComponent<EnnemiAttack>();

        myAnimator = GetComponent<Animator>();

        hp = maxHp;

        parentObject = transform.parent;

        GetComponentInParent<EnnemiManager>().bloodQuantity += nbrBlood;
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
            EnnemiAttack.QTEDone = true;
            EnnemiAttack.tutoDone = true;
            EnnemiAttack.coroutineLaunch = true;
            transform.parent = null;
            ennemiAttack.enabled = false;
            //PlayerBlood.GetBlood(nbrBlood);
            myAnimator.SetBool("Die", true);
            PlayerController.ennemi = null;
            //StartCoroutine("RespawnEnnemi");
        }
    }

    /*IEnumerator RespawnEnnemi()
    {
        yield return new WaitForSeconds(5f);

        transform.parent = parentObject;
        hp = maxHp;
        ennemiAttack.state = 0;
        ennemiAttack.startBattle = false;
        ennemiAttack.enabled = true;
        myAnimator.SetBool("Die", false);

        yield break;
    }*/
}