using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerController : MonoBehaviour
{
    Battle battle;

    [SerializeField] Animator myAnimator;

    CharacterController controller;
    public Vector3 move;

    [Header("Speed param")]
    [SerializeField] float currentSpeed = 2.0f;
    [SerializeField] float baseSpeed;
    [SerializeField] float backSpeed;
    [SerializeField] float lateralSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float playerVelocity;

    [SerializeField] float turnSmoothTime = 0.7f;
    float turnSmoothVelocity;
    float targetAngle;
    float angle;

    [SerializeField] float gravityValue = -10f;

    [SerializeField] float smoothRun, smoothWalk, smoothRotateToEnnemi;

    public static Transform ennemi;
    public Transform ennemiDebug;

    BoxCollider swordCollider;

    Quaternion lookEnnemi;

    public bool isRunning, isWalking;

    AudioSource myAudioSource;

    [SerializeField]
    CinemachineVirtualCamera camBase;

    CinemachineComposer cinemachineComposer;

    bool camRight;

    public static bool CamXInverser, CamYInverser;

    [SerializeField] float speedCameraSwitchSide;

    CapsuleCollider capsuleCollider;

    Rigidbody rb;

    float horizontalAxis;
    float verticalAxis;

    public bool lookZone;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
        runSpeed = baseSpeed * 2f;

        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        controller = GetComponent<CharacterController>();
        battle = transform.Find("PlayerAnim").GetComponent<Battle>();

        swordCollider = transform.GetChild(0).transform.GetChild(1).GetComponent<BoxCollider>();

        myAudioSource = GetComponent<AudioSource>();

        camRight = true;

        cinemachineComposer = camBase.GetCinemachineComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") >0)
        {
            myAnimator.SetFloat("InputY", new Vector2(0, Input.GetAxis("Vertical")).magnitude);
        }
        else
        {
            myAnimator.SetFloat("InputY", new Vector2(0, Input.GetAxis("Vertical")).magnitude *-1);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            myAnimator.SetFloat("InputX", new Vector2(Input.GetAxis("Horizontal"), 0).magnitude);
        }
        else
        {
            myAnimator.SetFloat("InputX", new Vector2(Input.GetAxis("Horizontal"), 0).magnitude * -1);
        }

        CameraLibreNoMove();
        ChangeCamCenterView();

        //Deplacement();

        if (PlayerHp.hp > 0 && !lookZone)
            Deplacement();

        AnimationDeclancheur();
    }

    void LateUpdate()
    {
        ennemiDebug = ennemi;
        if (battle.degaine)
        {
            if (PlayerHp.hp > 0 && ennemi != null)
            {
                /*Vector3 ennemiPos;

                ennemiPos.x = ennemi.transform.position.x;
                ennemiPos.y = 0f;
                ennemiPos.z = ennemi.transform.position.z;

                SmoothLookAt(ennemiPos);*/

                Vector3 relativePos;

                relativePos.x = ennemi.position.x - transform.position.x;
                relativePos.y = 0;
                relativePos.z = ennemi.position.z - transform.position.z;

                // the second argument, upwards, defaults to Vector3.up
                Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), smoothRotateToEnnemi);
                transform.rotation = rotation;

            }
        }
    }

    void AnimationDeclancheur()
    {
        myAnimator.SetBool("Walk", isWalking);
        myAnimator.SetBool("Run", isRunning);

        if(isWalking)
        {
            if (Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Horizontal"), 2f)) > Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Vertical"), 2f)))
                myAnimator.speed = Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Horizontal"), 2f));
            else
                myAnimator.speed = Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Vertical"), 2f));
        }
        else
        {
            myAnimator.speed = 1;
        }
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

        if(camRight && cinemachineComposer.m_TrackedObjectOffset.x < 1.5f)
        {
            cinemachineComposer.m_TrackedObjectOffset.x += Time.deltaTime * speedCameraSwitchSide;

            if (cinemachineComposer.m_TrackedObjectOffset.x > 1.5f)
            {
                cinemachineComposer.m_TrackedObjectOffset.x = 1.5f;
            }
        }
        else if(!camRight && cinemachineComposer.m_TrackedObjectOffset.x > -1.5f)
        {
            cinemachineComposer.m_TrackedObjectOffset.x -= Time.deltaTime * speedCameraSwitchSide;
            if(cinemachineComposer.m_TrackedObjectOffset.x < -1.5f)
            {
                cinemachineComposer.m_TrackedObjectOffset.x = -1.5f;
            }
        }
    }

    void SmoothLookAt(Vector3 target)
    {
        lookEnnemi = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookEnnemi, 10 * Time.deltaTime);
    }

    void Deplacement()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            isRunning = false;
            isWalking = false;
        }

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;

        Vector3 inputValues = new Vector3(horizontalAxis, 0f, verticalAxis);

        //movement
        if (!battle.degaine)
        {
            if (capsuleCollider.enabled)
            {
                controller.enabled = true;
                capsuleCollider.enabled = false;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            //Run
            RunMovement();

            if (controller.isGrounded)
            {
                if (direction.magnitude >= 0f)
                {
                    if (direction.z < 0 || direction.x !=0)
                    {
                        targetAngle = Mathf.Atan2(direction.x, direction.z) / Mathf.Rad2Deg + camBase.transform.eulerAngles.y;
                        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                        transform.rotation = Quaternion.Euler(0, angle, 0);
                        move = Quaternion.Euler(0, targetAngle, 0) * direction;
                    }
                    else if(direction.z > 0)
                    {
                        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camBase.transform.eulerAngles.y;
                        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                        transform.rotation = Quaternion.Euler(0, angle, 0);
                        move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                    }
                }
            }
            if(Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") != 0)
            {
                if (!isRunning)
                    isWalking = true;

                move.y += gravityValue * Time.deltaTime;
                controller.Move(inputValues.magnitude * move.normalized * currentSpeed * Time.deltaTime);

                if (!myAudioSource.isPlaying && controller.isGrounded)
                    ActiveSound();
            }
            else
            {
                move.y += gravityValue * Time.deltaTime;
                move.x =0;
                move.z = 0;
                controller.Move(move.normalized);
            }
        }
        else
        {
            isWalking = false;
            isRunning = false;

            if(!capsuleCollider.enabled)
            {
                controller.enabled = false;
                capsuleCollider.enabled = true;
                rb.useGravity = true;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    void RunMovement()
    {
        if (Input.GetButtonDown("RunButton"))
        {
            if(!isRunning && Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
            {
                isRunning = true;
                isWalking = false;
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
            if(Input.GetAxis("Vertical") <0)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, backSpeed, smoothWalk);
            }
            else
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Abs(Input.GetAxis("Vertical")))
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, lateralSpeed, smoothWalk);
                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, smoothWalk);
                }
            }
        }

    }
    //Sound 
    void ActiveSound()
    {
        //SoundManager.PlaySoundStepFoot(myAudioSource, SoundManager.soundAndVolumePlayerExplorationStatic[Random.Range(0,2)]);
    }

    //Animation Event : 

    /*void AnimCounter()
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
        ShakeCam.ShakeCamBlockNormal(ShakeCam.shakeCamParametersAttackPerfectStatic, ShakeCam.shakeCamParametersAttackPerfectStatic[0].axeShake, ShakeCam.shakeCamParametersAttackPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamParametersAttackPerfectStatic[0].frequence, ShakeCam.shakeCamParametersAttackPerfectStatic[0].duration);

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
    }*/
}