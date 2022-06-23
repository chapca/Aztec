using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] public static Animator myAnimator;

    AudioSource parentAudioSource, battleAudioSource, explorationAudioSource, battleStartAudioSource;

    [SerializeField]
    CinemachineVirtualCamera camBase, camBattle;

    public bool degaine, isAttacked;
 
    static public bool canEsquive, canBlock, canCounter, canAttack;
    static public bool isCounter, isAttacking, isEsquiving, isBlocking;

    static public bool wallDetectLeft, wallDetectRight;

    [SerializeField] BoxCollider boxRight, boxLeft;

    void Start()
    {
        myAnimator = GetComponent<Animator>();

        parentAudioSource = transform.parent.GetComponent<AudioSource>();
        battleAudioSource = GameObject.Find("BattleMusic").GetComponent<AudioSource>();
        explorationAudioSource = GameObject.Find("ExplorationMusic").GetComponent<AudioSource>();
        battleStartAudioSource = GameObject.Find("BattleStartSFX").GetComponent<AudioSource>();

        SoundManager.PlaySound2DContinue(explorationAudioSource, SoundManager.soundAndVolume2DStatic[5], true);
    }

    void Update()
    {
        if(isAttacked)
        {
            PlayerUI.ActiveUIHealthButton(false);

            if (!degaine)
            {
               SoundManager.PlaySound2DContinue(explorationAudioSource, SoundManager.soundAndVolume2DStatic[5], false);
               SoundManager.PlaySound2DOneShot(battleStartAudioSource, SoundManager.soundAndVolume2DStatic[7]);
               SoundManager.PlaySound2DContinue(battleAudioSource, SoundManager.soundAndVolume2DStatic[6], true);


                myAnimator.SetBool("Degaine", true);
                degaine = true;
                camBattle.Priority = 11;

                camBase.transform.position = Vector3.zero;
            }
        }
        else
        {
            if (degaine)
            {
                SoundManager.PlaySound2DContinue(battleAudioSource, SoundManager.soundAndVolume2DStatic[6], false);
                SoundManager.PlaySound2DContinue(explorationAudioSource, SoundManager.soundAndVolume2DStatic[5], true);
            }
            PlayerUI.ActiveUIHealthButton(true);
            myAnimator.SetBool("Degaine", false);
            degaine = false;
            camBattle.Priority = 9;
        }

       
        if(canEsquive)
        {
            Debug.LogError("Launch esquive anim");

            if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 && EnnemiAttack.esquiveRight || Input.GetAxisRaw("HorizontalLeftButtonX") > 0 && Boss.esquiveRight ||
                Input.GetButtonDown("CancelButton") && EnnemiAttack.esquiveRight || Input.GetButtonDown("CancelButton") && Boss.esquiveRight)
            {
                myAnimator.SetBool("EsquiveDroite", true);

                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
            else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 && EnnemiAttack.esquiveLeft || Input.GetAxisRaw("HorizontalLeftButtonX") < 0 && Boss.esquiveLeft ||
                Input.GetButtonDown("HealthButton") && EnnemiAttack.esquiveLeft || Input.GetButtonDown("HealthButton") && Boss.esquiveLeft)
            {
                myAnimator.SetBool("EsquiveGauche", true);
                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }

        if (canBlock)
        {
            if(Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
            {
                Debug.Log("Launch Block");
                //myAnimator.SetBool("Block", true);

                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }
        
        if(canCounter)
        {
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                myAnimator.SetBool("Counter", true);
                canCounter = false;
                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }

        if (canAttack)
        {
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
               /* if(AnimationEvent.attackPerfect)
                {
                    myAnimator.SetBool("AttackPerfect", true);
                    Debug.LogWarning("AttackPerfect");
                }
                else
                {
                    myAnimator.SetBool("AttackNormal", true);
                    Debug.LogWarning("AttackNormal");
                }*/

                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            Debug.LogError("WAll Contact");
            wallDetectRight = true;
            wallDetectLeft = true;
            /*if (boxRight.CompareTag("Wall"))
            {
                wallDetectRight = true;

                Debug.LogError("wallDetectRight " + wallDetectRight);
            }
            if (boxLeft.CompareTag("Wall"))
            {
                wallDetectLeft = true;
                Debug.LogError("wallDetectLeft " + wallDetectLeft);
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            wallDetectRight = false;
            wallDetectLeft = false;
            /*if (boxRight.CompareTag("Wall"))
            {
                wallDetectRight = false;

                Debug.LogError("wallDetectRight " + wallDetectRight);
            }
            if (boxLeft.CompareTag("Wall"))
            {
                wallDetectLeft = false;
                Debug.LogError("wallDetectLeft " + wallDetectLeft);
            }*/
        }
    }
}