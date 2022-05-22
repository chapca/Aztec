using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvent : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] float speedEsquive, currentSpeedEsquive, esquiveAngleRot, rotationY, rotSpeedY, speedReturnBaseRot;

    [SerializeField] bool esquiveLeft, esquiveRight, returnBaseRotation;

    public static bool DodgeBlock, attackStandard, attackPerfect;

    public static GameObject ennemi;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (esquiveRight)
        {
            transform.parent.transform.Translate(Vector3.right * currentSpeedEsquive * Time.deltaTime);
            
            if (transform.localRotation.y < esquiveAngleRot)
            {
                rotationY += rotSpeedY * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(0, rotationY, 0);
            }
        }
        else if (esquiveLeft)
        {
            transform.parent.transform.Translate(Vector3.left * currentSpeedEsquive * Time.deltaTime);

            if (transform.localRotation.y > -esquiveAngleRot)
            {
                rotationY -= rotSpeedY * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(0, rotationY, 0);
            }
        }

        if(returnBaseRotation)
        {
            if (transform.localRotation.y != 0)
            {
                rotationY = Mathf.Lerp(rotationY, 0, speedReturnBaseRot);
                transform.localRotation = Quaternion.Euler(0, rotationY, 0);

                if (transform.localRotation.y == 0)
                {
                    rotationY = 0;
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    returnBaseRotation = false;
                }
            }
        }
    }

    void EsquiveDroiteRotation()
    {
        esquiveRight = true;
    }
    void EsquiveDroite()
    {
        currentSpeedEsquive = speedEsquive;
    }
    void EndEsquiveDroite()
    {
        Battle.myAnimator.SetBool("EsquiveDroite", false);
        esquiveRight = false;
        currentSpeedEsquive = 0;

        returnBaseRotation = true;
    }

    void EsquiveGaucheRotation()
    {
        esquiveLeft = true;
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

        returnBaseRotation = true;
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
        Debug.Log("StopAttack");

        if(attackPerfect)
        {
            ShakeCam.ShakeCamBlockNormal(ShakeCam.shakeCamParametersAttackPerfectStatic, ShakeCam.shakeCamParametersAttackPerfectStatic[0].axeShake, ShakeCam.shakeCamParametersAttackPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackPerfectStatic[0].frequence, ShakeCam.shakeCamParametersAttackPerfectStatic[0].duration);
        }
        else
        {
            ShakeCam.ShakeCamBlockNormal(ShakeCam.shakeCamParametersAttackNormalStatic, ShakeCam.shakeCamParametersAttackNormalStatic[0].axeShake, ShakeCam.shakeCamParametersAttackNormalStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackNormalStatic[0].frequence, ShakeCam.shakeCamParametersAttackNormalStatic[0].duration);
        }

        Battle.myAnimator.SetBool("AttackNormal", false);
        Battle.myAnimator.SetBool("AttackPerfect", false);
        Battle.isAttacking = false;
        StartCoroutine("StopFuckingAttack");
    }

    void ParadeReussi()
    {
        Debug.Log("parade anim start");
        DodgeBlock = true;
    }
    void EndParade()
    {
        Battle.myAnimator.SetBool("IsHit", false);
        Battle.myAnimator.SetBool("BlockNormal", false);
        Battle.myAnimator.SetBool("BlockPerfect", false);
        DodgeBlock = false;
    }

    void EndPerfectParade()
    {
        Battle.myAnimator.SetBool("BlockPerfectEnd", false);
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

    void StartDeath()
    {
        DeathText.ActiveText();
        PlayerController.ennemi = null;
        PlayerBlood.deadWastage = true;
    }

    void Death()
    {
        Debug.Log("Launch RestartScene");
        StartCoroutine("RestartScene");
    }

    IEnumerator RestartScene()
    {
        Debug.Log("Launch RestartScene");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("AlphaScene");
        yield break;
    }

    void AnimCounter()
    {
        if (!Battle.isCounter)
        {
            Battle.isCounter = true;
            ennemi.GetComponent<EnnemiHp>().TakeDamage(100);
            ennemi.transform.parent.GetComponent<EnnemiAttack>().myAnimator.SetTrigger("Hit");
            Debug.Log(Battle.isCounter);
        }
    }

    void AnimCounterEnd()
    {
        ShakeCam.ShakeCamBlockNormal(ShakeCam.shakeCamParametersAttackPerfectStatic, ShakeCam.shakeCamParametersAttackPerfectStatic[0].axeShake, ShakeCam.shakeCamParametersAttackPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackPerfectStatic[0].frequence, ShakeCam.shakeCamParametersAttackPerfectStatic[0].duration);

        StartCoroutine("StopCounter");

        Battle.myAnimator.SetBool("Counter", false);
        Battle.canCounter = false;
        Battle.isCounter = false;
        Debug.Log(Battle.isCounter);
    }

    IEnumerator StopCounter()
    {
        yield return new WaitForSeconds(0.2f);

        Battle.isCounter = false;
        Debug.Log(Battle.isCounter);
        yield break;
    }
}