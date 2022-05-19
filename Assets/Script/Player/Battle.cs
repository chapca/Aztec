using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] public static Animator myAnimator;

    AudioSource parentAudioSource, battleAudioSource, explorationAudioSource;

    [SerializeField]
    CinemachineVirtualCamera camBase, camBattle;

    public bool degaine, isAttacked;
 
    static public bool canEsquive, canBlock, canCounter, canAttack;
    static public bool isCounter, isAttacking, isEsquiving, isBlocking;

    void Start()
    {
        myAnimator = GetComponent<Animator>();

        parentAudioSource = transform.parent.GetComponent<AudioSource>();
        battleAudioSource = GameObject.Find("BattleMusic").GetComponent<AudioSource>();
        explorationAudioSource = GameObject.Find("ExplorationMusic").GetComponent<AudioSource>();

        SoundManager.PlaySound2DContinue(explorationAudioSource, SoundManager.soundAndVolume2DStatic[5], true);
    }

    void Update()
    {
        Debug.Log(canBlock);

        if(isAttacked)
        {
            if(!degaine)
            {
               SoundManager.PlaySound2DContinue(explorationAudioSource, SoundManager.soundAndVolume2DStatic[5], false);
               SoundManager.PlaySoundPlayerBattle(parentAudioSource, SoundManager.soundAndVolume2DStatic[7]);
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

            myAnimator.SetBool("Degaine", false);
            degaine = false;
            camBattle.Priority = 9;
        }

       
        if(canEsquive)
        {
            if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
            {
                myAnimator.SetBool("EsquiveDroite", true);

                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
            else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0)
            {
                myAnimator.SetBool("EsquiveGauche", true);
                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }

        if(canBlock)
        {
            if(Input.GetButtonDown("BlockButton"))
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
            if (Input.GetButtonDown("InteractButton"))
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
            if (Input.GetButtonDown("InteractButton"))
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
}