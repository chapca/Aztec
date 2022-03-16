using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Battle battle;

    CharacterController controller;
    public Vector3 move;

    [SerializeField] float currentSpeed = 2.0f;
    [SerializeField] float baseSpeed;
    [SerializeField] float runSpeed;

    [SerializeField] float speedRot = 100;
    [SerializeField] float rotY;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float gravityValue = -10f;

    public static Transform ennemi;

    BoxCollider swordCollider;

    Quaternion lookEnnemi;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
        runSpeed = baseSpeed * 1.5f;

        controller = GetComponent<CharacterController>();
        battle = transform.GetChild(0).GetComponent<Battle>();

        swordCollider = transform.GetChild(0).transform.GetChild(1).GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
    }
    void LateUpdate()
    {
        if (!battle.degaine)
            RotationPlayer();
        else
        {
            Vector3 ennemiPos;

            ennemiPos.x = ennemi.transform.position.x;
            ennemiPos.y = 0;
            ennemiPos.z = ennemi.transform.position.z;

            SmoothLookAt(ennemiPos);
            //transform.LookAt(ennemiPos);
        }
    }

    void SmoothLookAt(Vector3 target)
    {
        lookEnnemi = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookEnnemi, 10 * Time.deltaTime);
    }

    void Deplacement()
    {
        //movement
        if (!battle.degaine)
        {
            if (controller.isGrounded)
            {
                // move basic
                move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                //Jump
                if (Input.GetButtonDown("Jump"))
                {
                    move.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                }

                move = transform.TransformDirection(move);
                controller.Move(move * Time.deltaTime * currentSpeed);
            }
        }

        move.y += gravityValue * Time.deltaTime;
        controller.Move(move * Time.deltaTime);


        //Run
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = baseSpeed;
        }
    }

    void RotationPlayer()
    {
        rotY += Input.GetAxis("RightJoystickX") * speedRot * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, rotY, 0);
    }


    //Animation Event : 

    void AnimCounter()
    {
        if(!Battle.isCounter)
        {
            Battle.isCounter = true;
            ennemi.GetComponent<EnnemiHp>().TakeDamage(100);
            ennemi.transform.parent.GetComponent<EnnemiAttack>().myAnimator.SetTrigger("Hit");
            Debug.Log(Battle.isCounter);
        }
    }

    void AnimCounterEnd()
    {
        StartCoroutine("StopCounter");

        Battle.myAnimator.SetBool("Estoc", false);
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