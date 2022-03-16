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
        if(isAttacked)
        {
            myAnimator.SetBool("Degaine", true);
            degaine = true;
            camBattle.Priority = 11;
        }
        else
        {
            myAnimator.SetBool("Degaine", false);
            degaine = false;
            camBattle.Priority = 9;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            if(!degaine)
            {
                myAnimator.SetBool("Degaine", true);
                degaine = true;
                camBattle.Priority = 11;
            }
            else
            {
                myAnimator.SetBool("Degaine", false);
                degaine = false;
                camBattle.Priority = 9;
            }
        }

        if(canEsquive)
        {
            if (Input.GetAxisRaw("HorizontalLeftButtonX") >0)
            {
                myAnimator.SetBool("EsquiveDroite", true);
            }
            else if (Input.GetAxisRaw("HorizontalLeftButtonX") <0)
            {
                myAnimator.SetBool("EsquiveGauche", true);
            }
        }

        if(canBlock)
        {
            if(Input.GetAxisRaw("VerticalLeftButtonY") < 0)
                myAnimator.SetBool("Block", true);
        }
        
        if(canCounter)
        {
            if (Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                myAnimator.SetBool("Estoc", true);
        }

        if (canAttack)
        {
            if (Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                myAnimator.SetBool("Taille", true);
        }
    }
}