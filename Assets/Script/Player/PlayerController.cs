using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerController : MonoBehaviour
{
    Battle battle;

    CharacterController controller;
    public Vector3 move;

    [Header("Speed param")]
    [SerializeField] float currentSpeed = 2.0f;
    [SerializeField] float baseSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float speedRot = 100;
    [SerializeField] float playerVelocity;

    [SerializeField] float rotY;

    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float gravityValue = -10f;

    [SerializeField] float smoothRun, smoothWalk;

    public static Transform ennemi;

    BoxCollider swordCollider;

    Quaternion lookEnnemi;

    public bool isRunning;

    AudioSource myAudioSource;

    [SerializeField]
    CinemachineVirtualCamera camBase;

    Cinemachine3rdPersonFollow camBaseThirdPersonFollow;

    bool camRight;

    public static bool CamXInverser, CamYInverser;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
        runSpeed = baseSpeed * 2f;

        controller = GetComponent<CharacterController>();
        battle = transform.GetChild(0).GetComponent<Battle>();

        swordCollider = transform.GetChild(0).transform.GetChild(1).GetComponent<BoxCollider>();

        myAudioSource = GetComponent<AudioSource>();

        camRight = true;

        camBaseThirdPersonFollow = camBase.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraLibreNoMove();

        ChangeCamCenterView();

        //Deplacement();

        if (!battle.degaine)
            RotationPlayer();
        else
        {
            if(PlayerHp.hp >0)
            {
                Vector3 ennemiPos;

                ennemiPos.x = ennemi.transform.position.x;
                ennemiPos.y = 0f;
                ennemiPos.z = ennemi.transform.position.z;

                SmoothLookAt(ennemiPos);
            }
        }
    }
    void LateUpdate()
    {
        Deplacement();
    }

    void CameraLibreNoMove() // change le déplacement de camera quand le player ne bouge pas
    {
        playerVelocity = controller.velocity.magnitude;
        if (playerVelocity == 0)
        {
            TargetCam.canTurnCamAroundPlayer = true;
        }
        else
        {
            TargetCam.canTurnCamAroundPlayer = false;
        }
    }

    void ChangeCamCenterView()
    {
        if(Input.GetButtonDown("ChangeViewCenter"))
        {
            if (camRight)
                camRight = false;
            else
                camRight = true;
        }

        if(camRight && camBaseThirdPersonFollow.CameraSide < 0.85f)
            camBaseThirdPersonFollow.CameraSide += Time.deltaTime;
        else if(!camRight && camBaseThirdPersonFollow.CameraSide > 0.15f)
            camBaseThirdPersonFollow.CameraSide -= Time.deltaTime;
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

                move = transform.TransformDirection(move);
                controller.Move(move * Time.deltaTime * currentSpeed);

                if(Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0)
                {
                    if(!myAudioSource.isPlaying)
                        ActiveSound();
                }
            }
        }

        move.y += gravityValue * Time.deltaTime;
        controller.Move(move * Time.deltaTime);


        //Run
        RunMovement();
    }

    void RunMovement()
    {
        if (Input.GetButtonDown("RunButton"))
        {
            if(!isRunning)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }

        if(isRunning)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, smoothRun);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, smoothWalk);
        }

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            isRunning = false;
        }
    }

    void RotationPlayer()
    {
        if(CamXInverser)
        {
            if (playerVelocity > 0)
            {
                rotY -= Input.GetAxis("RightJoystickX") * speedRot * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(0, rotY, 0);
            }
        }
        else
        {
            if (playerVelocity > 0)
            {
                rotY += Input.GetAxis("RightJoystickX") * speedRot * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(0, rotY, 0);
            }
        }
    }

    //Sound 
    void ActiveSound()
    {
        SoundManager.PlaySoundStepFoot(myAudioSource, SoundManager.audioClipsListStatic[Random.Range(0,2)]);
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