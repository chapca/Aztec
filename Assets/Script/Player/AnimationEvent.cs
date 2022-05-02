using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] float speedEsquive, currentSpeedEsquive;

    Transform player;

    [SerializeField] bool esquiveLeft, esquiveRight;

    public static bool DodgeBlock, attackStandard, attackPerfect;

    public static GameObject ennemi;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.GetChild(0).GetComponent<PlayerController>();

        player = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (esquiveRight)
            transform.Translate(Vector3.right * currentSpeedEsquive * Time.deltaTime);

        if (esquiveLeft)
            transform.Translate(Vector3.right * -currentSpeedEsquive * Time.deltaTime);
    }

    void EsquiveDroite()
    {
        esquiveRight = true;
        currentSpeedEsquive = speedEsquive;
    }
    void EndEsquiveDroite()
    {
        Battle.myAnimator.SetBool("EsquiveDroite", false);
        esquiveRight = false;
        currentSpeedEsquive = 0;
    }

    void EsquiveGauche()
    {
        Battle.myAnimator.SetBool("EsquiveGauche", false);
        esquiveLeft = true;
        currentSpeedEsquive = speedEsquive;
    }
    void EndEsquiveGauche()
    {
        esquiveLeft = false;
        currentSpeedEsquive = 0;
    }

    void AttackStart()
    {
        Battle.canAttack = false;
        if (!Battle.isAttacking)
        {
            Battle.isAttacking = true;

            if(attackStandard)
                ennemi.GetComponent<EnnemiHp>().TakeDamage(50);
            if(attackPerfect)
                ennemi.GetComponent<EnnemiHp>().TakeDamage(100);
        }
    }
    void AttackEnd()
    {
        Battle.myAnimator.SetBool("Taille", false);
        Battle.isAttacking = false;
        StartCoroutine("StopFuckingAttack");
    }

    void ParadeReussi()
    {
        DodgeBlock = true;
    }
    void EndParade()
    {
        Battle.myAnimator.SetBool("Block", false);
        DodgeBlock = false;
    }

    IEnumerator StopFuckingAttack()
    {
        attackStandard = false;
        attackPerfect = false;
        Battle.isAttacking = false;
        yield return new WaitForSecondsRealtime(0.1f);

        if (Battle.isAttacking)
        {
            Battle.isAttacking = false;
            yield break;
        }
        else
            yield break;
    }
}