using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class AnimationEvent : MonoBehaviour
{
    static AnimationEvent instance;

    PlayerController playerController;

    [SerializeField] float speedEsquive, currentSpeedEsquive, esquiveAngleRot, rotationY, rotSpeedY, speedReturnBaseRot;

    [SerializeField] bool esquiveLeft, esquiveRight, returnBaseRotation;

    public static bool DodgeBlock, attackStandard, attackPerfect;

    public static GameObject ennemi;

    AudioSource audioSource;

    public static bool bossFight;

    [SerializeField] VolumeProfile mVolumeProfile;
    [SerializeField] DepthOfField depthOfField;

    [SerializeField] Vector3 posBeforeEsquive, posAfterEsquive, goodPos;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        audioSource = GetComponent<AudioSource>();

        depthOfField = (DepthOfField)mVolumeProfile.components[8];
        depthOfField.active = false;
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
        posBeforeEsquive = transform.position;
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
        posAfterEsquive = transform.position;
        goodPos = posAfterEsquive - posBeforeEsquive;
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
        posAfterEsquive = transform.position;
        esquiveLeft = true;
        currentSpeedEsquive = speedEsquive;
    }
    void EndEsquiveGauche()
    {
        posAfterEsquive = transform.position;
        goodPos = posAfterEsquive - posBeforeEsquive;
        esquiveLeft = false;
        currentSpeedEsquive = 0;

        returnBaseRotation = true;
    }

    void AttackStart()
    {
        Battle.canAttack = false;

        if (attackPerfect)
        {
            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersAttackPerfectStatic, ShakeCam.shakeCamParametersAttackPerfectStatic[0].axeShake, ShakeCam.shakeCamParametersAttackPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackPerfectStatic[0].frequence, ShakeCam.shakeCamParametersAttackPerfectStatic[0].duration);
        }
        else
        {
            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersAttackNormalStatic, ShakeCam.shakeCamParametersAttackNormalStatic[0].axeShake, ShakeCam.shakeCamParametersAttackNormalStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackNormalStatic[0].frequence, ShakeCam.shakeCamParametersAttackNormalStatic[0].duration);
        }

        if (!Battle.isAttacking)
        {
            Battle.isAttacking = true;

            if(attackStandard)
            {
                if(!bossFight)
                    ennemi.GetComponent<EnnemiHp>().TakeDamage(50);
                else
                    if (!HPBoss.finalCombo)
                        ennemi.GetComponent<HPBoss>().TakeDamage(50);

                SoundManager.PlaySoundPlayerBattle(audioSource, SoundManager.soundAndVolumePlayerBattleStatic[1]);
            }
            if(attackPerfect)
            {
                if (!bossFight)
                {
                    ennemi.GetComponent<EnnemiHp>().TakeDamage(100);
                }
                else
                {
                    if (!HPBoss.finalCombo)
                        ennemi.GetComponent<HPBoss>().TakeDamage(100);
                }

                SoundManager.PlaySoundPlayerBattle(audioSource, SoundManager.soundAndVolumePlayerBattleStatic[2]);
            }
        }
    }
    void AttackEnd()
    {
        Debug.Log("StopAttack");


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
            SoundManager.PlaySoundPlayerBattle(audioSource, SoundManager.soundAndVolumePlayerBattleStatic[3]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersAttackPerfectStatic, ShakeCam.shakeCamParametersAttackPerfectStatic[0].axeShake, ShakeCam.shakeCamParametersAttackPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackPerfectStatic[0].frequence, ShakeCam.shakeCamParametersAttackPerfectStatic[0].duration);

            if (!bossFight)
            {
                ennemi.GetComponent<EnnemiHp>().TakeDamage(100);
                ennemi.GetComponent<EnnemiAttack>().myAnimator.SetTrigger("Hit");
            }
            else
            {
                if (!HPBoss.finalCombo)
                    ennemi.GetComponent<HPBoss>().TakeDamage(100);
                ennemi.GetComponent<Boss>().myAnimator.SetTrigger("Hit");
            }

            Debug.Log(Battle.isCounter);
        }
    }

    void AnimCounterEnd()
    {
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

    static public void Hit()
    {
        instance.LaunchBlurCoroutine();
    }

    void LaunchBlurCoroutine()
    {
        StartCoroutine("BlurCoroutine");
    }

    IEnumerator BlurCoroutine()
    {
        depthOfField.active = true;
        yield return new WaitForSeconds(0.35f);
        depthOfField.active = false;
        yield break;
    }

    void SoundFootStep()
    {
        //Debug.LogError("Sound FootStep");
        
        if(!audioSource.isPlaying)
            SoundManager.PlaySoundStepFoot(audioSource, SoundManager.soundAndVolumePlayerExplorationStatic[Random.Range(0, 2)]);
    }
}