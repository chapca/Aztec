using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiAnimEvent : MonoBehaviour
{
    EnnemiAttack ennemiAttack;

    void Start()
    {
        ennemiAttack = transform.parent.transform.parent.GetComponent<EnnemiAttack>();
    }

    void StartAttack()
    {
        //ennemiAttack.SetUpStartActionPlayer();
    }
    void EndAttack()
    {
        ennemiAttack.EndAttack();
    }
    void ApplyDamageToPlayer()
    {
        ennemiAttack.ApplyDamageToPlayer();
    }
}