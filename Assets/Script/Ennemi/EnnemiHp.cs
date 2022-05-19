using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiHp : MonoBehaviour
{
    public int maxHp, hp, nbrBlood;
    EnnemiAttack ennemiAttack;

    Transform parentObject;

    // Start is called before the first frame update
    void Start()
    {
        ennemiAttack = GetComponent<EnnemiAttack>();

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

        if(hp <=0)
        {
            transform.parent = null;
            ennemiAttack.enabled = false;
            PlayerBlood.GetBlood(nbrBlood);
            Battle.myAnimator.SetBool("Death", true);

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