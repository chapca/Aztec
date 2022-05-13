using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] public static Animator myAnimator;

    [SerializeField]
    CinemachineVirtualCamera camBase, camBattle;

    public bool degaine, isAttacked;
 
    static public bool canEsquive, canBlock, canCounter, canAttack;
    static public bool isCounter, isAttacking, isEsquiving, isBlocking;

    void Start()
    {
        myAnimator = transform.parent.GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(canBlock);

        if(isAttacked)
        {
            myAnimator.SetBool("Degaine", true);
            degaine = true;
            camBattle.Priority = 11;

            camBase.transform.position = Vector3.zero;
        }
        else
        {
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
                myAnimator.SetBool("Block", true);

                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }
        
        if(canCounter)
        {
            if (Input.GetButtonDown("InteractButton"))
            {
                myAnimator.SetBool("Estoc", true);
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
                myAnimator.SetBool("Taille", true);
                canEsquive = false;
                canBlock = false;
                canAttack = false;
            }
        }
    }
}