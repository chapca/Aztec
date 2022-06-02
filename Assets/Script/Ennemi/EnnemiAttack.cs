using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnnemiAttack : MonoBehaviour
{
    EnnemiHp ennemiHp;

    EnnemiManager ennemiManager;

    AudioSource myAudioSource;

    public AudioSource bulletTimeAudioSource, qteTimerAudioSource, qteValidationAudioSource, playerAudioSource;

    public Animator myAnimator;

    [SerializeField] float distPlayer;

    Transform player;

    Battle battleScript;

    [SerializeField] bool lookat, isAttacking, goBack, moveToPlayerBeforeAttack, launchAttack, canAttack;

    Quaternion lookPlayer;

    public int state;

    [SerializeField] bool moveRight;

    float time = 2f;

    [SerializeField] float randomTimeBeforeAttack;

    [SerializeField] bool randomAttack, retrunState1, paradeReussi, counterReussi, attackReussiperfect, esquiveReussiPerfect;

    static public bool esquivePerfect;

    [SerializeField] bool playerAction, startQTE, startQTECounter, canApplyDamage, canApplyDamageBlock, canShakeCam;

    public bool thisSelected, resetEnnemiSelected;

    public bool startBattle;

    float convertion;

    [Header("MANUELLE Defini la durée du qte en scd(min:0 / max:1)")]
    [Range(0f, 10f)]
    [SerializeField] float setUpStartActionPlayer;
    [Range(0.0f, 10f)]
    [SerializeField] float setUpEndActionPlayer;

    [Header("MANUELLE Defini la position des frames perfect(min:90° / max:359.99°)")]
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameAttack;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameEsquive;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameBlock;

    [Header("MANUELLE Defini la position des frames Loose(min:90° / max:359.99°)")]
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameAttack;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameCounter;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameEsquive;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameBlock;

    [Header("AUTO Valeur à laquel la frame perfect démare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartPerfectFrameAttack;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartPerfectFrameEsquive;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartPerfectFrameBlock;

    [Header("AUTO Valeur à laquel la frame loose démare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameAttack;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameCounter;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameEsquive;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameBlock;

    [Header("AUTO durée au slider normal")]
    [Range(0.0f, 3f)]
    [SerializeField] float baseSetUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpSliderPerfect;

    [Header("AUTO durée au slider loose")]
    [Range(0.0f, 3f)]
    [SerializeField] float setUpSliderLooseAttack;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpSliderLooseCounter;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpSliderLooseEsquive;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpSliderLooseBlock;

    [Header("Reference slider")]
    [SerializeField] Transform sliderAttackPerfect, sliderAttackNormal, sliderAttackLoose, sliderCounterLoose;
    [SerializeField] Transform sliderEsquivePerfect, sliderEsquiveNormal, sliderEsquiveLoose;
    [SerializeField] Transform sliderBlockPerfect, sliderBlockNormal, sliderBlockLoose;

    //(fillAmountSliderNormal *360f) +90f = angleSliderPerfect
    //(fillAmountSliderNormal *360f) = angleSliderPerfect - 90f
    //(fillAmountSliderNormal) = (angleSliderPerfect - 90f) /360f

    [Header("MANUAL Taille/durée slider")]
    [Range(0.0f, 1f)] [SerializeField] float sliderPerfectSize;
    float sliderPerfectAttacSize;
    float sliderPerfectEsquiveSize;
    float sliderPerfectBlockSize;

    [Header("MANUAL Taille/durée slider Loose")]
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseCounterSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseAttackSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseEsquiveSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseBlockSize;

    [Range(0.0f, 1f)]  float baseSliderLooseCounterSize;
    [Range(0.0f, 1f)]  float baseSliderLooseAttackSize;
    [Range(0.0f, 1f)]  float baseSliderLooseEsquiveSize;
    [Range(0.0f, 1f)]  float baseSliderLooseBlockSize;

    [Header("Execute code en hors Game (DESACTIVER AVANT DE LANCER)")]
    [SerializeField] bool activeThisinEditor, ManetteSpriteIsActive;

    [Header("Dégat du mob")]
    [SerializeField] float damage, blockDamage;

    int countRoundAttack;

    bool playerCanEsquive;

    [Header("Patrouille NavMesh")]
    [SerializeField] float circleRadius;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] bool findPosition = false;
    bool patrolIsActive;
    Vector3 basePos;
    Vector2 pos2D;
    Vector3 pos3D;

    RaycastHit hit;
    [SerializeField] LayerMask layerMask;

    public static bool QTEDone, coroutineLaunch, tutoDone;

    [SerializeField] GameObject tutoBlackScreen;

    [Header("Change la taille du slider quand on appuie au bon moment")]
    [SerializeField] Vector3 maxSize;
    [SerializeField] Vector3 baseSize;

    void Start()
    {
        /*sliderAttackPerfect = UIManager.sliderAttackPerfect.transform;
        sliderAttackNormal = UIManager.sliderAttack.transform;
        sliderEsquivePerfect = UIManager.sliderEsquivePerfectLeft.transform;
        sliderEsquiveNormal = UIManager.sliderEsquiveLeft.transform;
        sliderBlockPerfect = UIManager.sliderBlockPerfect.transform;
        sliderBlockNormal = UIManager.sliderBlock.transform;*/

        ennemiHp = GetComponent<EnnemiHp>();
        ennemiManager = GetComponentInParent<EnnemiManager>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        myAudioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        battleScript = GameObject.FindWithTag("Player").GetComponent<Battle>();

        state = 0;

        if(!activeThisinEditor)
        {
            setUpStartActionPlayer /= 10;
            setUpEndActionPlayer /= 10;
        }

        SetPositionFramePerfect();
        SetPositionFrameLoose();

        convertion = 10f / 6f;

        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal /(1f/sliderPerfectSize);

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) *10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        sliderPerfectAttacSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectBlockSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectEsquiveSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;


        setUpSliderLooseAttack = setUpTimerSliderNormal / (1f / sliderPerfectAttacSize);
        setUpSliderLooseCounter = setUpTimerSliderNormal / (1f / sliderLooseCounterSize);
        setUpSliderLooseEsquive = setUpTimerSliderNormal / (1f / sliderLooseEsquiveSize);
        setUpSliderLooseBlock = setUpTimerSliderNormal / (1f / sliderLooseBlockSize);

        sliderLooseAttackSize = setUpSliderLooseAttack / baseSetUpTimerSliderNormal;
        sliderLooseCounterSize = setUpSliderLooseCounter / baseSetUpTimerSliderNormal;
        sliderLooseEsquiveSize = setUpSliderLooseEsquive / baseSetUpTimerSliderNormal;
        sliderLooseBlockSize = setUpSliderLooseBlock / baseSetUpTimerSliderNormal;

        baseSliderLooseAttackSize = sliderLooseAttackSize;
        baseSliderLooseCounterSize = sliderLooseCounterSize;
        baseSliderLooseEsquiveSize = sliderLooseEsquiveSize;
        baseSliderLooseBlockSize = sliderLooseBlockSize;

        bulletTimeAudioSource = GameObject.Find("BattleBulletTimeMusic").GetComponent<AudioSource>();
        qteTimerAudioSource = GameObject.Find("QTETimerMusic").GetComponent<AudioSource>();
        qteValidationAudioSource = GameObject.Find("QTEValidationMusic").GetComponent<AudioSource>();
        playerAudioSource = GameObject.Find("EmptyPlayer").GetComponent<AudioSource>();

        tutoBlackScreen = GameObject.Find("TutoBlackScreen");
        tutoBlackScreen.GetComponent<Image>().enabled = false;

        basePos = transform.position;
    }

    void SetPositionFramePerfect()
    {
        sliderAttackPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameAttack);
        sliderEsquivePerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameEsquive);
        sliderBlockPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameBlock);
    }

    void SetPositionFrameLoose()
    {
        sliderAttackLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameAttack);
        sliderCounterLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCounter);
        sliderEsquiveLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameEsquive); 
        sliderBlockLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameBlock);
    }

    void Update()
    {
        if(activeThisinEditor)
        {
            UpdateSliderPosition();
            UpdateSliderLoosePosition();

            SetFramePerfectSize();
            SetFrameLooseSize();
        }

        distPlayer = Vector3.Distance(transform.position, player.position);

        if(retrunState1 && state !=0)
        {
            ReturnToStatePatrol();
        }

        if(distPlayer < 10 || ennemiManager.startBattle && PlayerHp.hp >0)
        {
            startBattle = true;
            ennemiManager.startBattle = true;
        }
        else if(!startBattle)
        {
            state = 0;
        }

        switch(state)
        {
            case 0:
                StatePatrol();
                break;
            case 1:
                StateWaitingPlayer();
                break;
            case 2:
                StateAttack();
                break;
            case 3:
                StateHurt();
                break;
            case 4:
                StateWaitCounter();
                break;
        }

        /*if (!thisSelected)
            ReturnToStatePatrol(); */
        
        if (!thisSelected && startBattle)
            StateWaitingPlayer();

        /*if (playerAction)
        {
            if (Input.GetButtonDown("BlockButton") || Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
            {
                //PlayerDoSomething();
                TimeScaleNormal();
                playerAction = false;
            }
        }*/

        if (startQTE && !QTEDone)
        {
            if (!coroutineLaunch)
            {
                ActiveManetteUI();
                StartCoroutine("CoolDownDone");
            }
        }

        if (startQTE && QTEDone)
        {
            DelayInput();
        }
        if (startQTECounter)
        {
            setUpTimerSliderNormal -= Time.unscaledDeltaTime*1.5f;
            TimingCounter();
        }
    }

    IEnumerator CoolDownDone()
    {
        if(!tutoBlackScreen.GetComponent<Image>().enabled)
            tutoBlackScreen.GetComponent<Image>().enabled = true;

        tutoBlackScreen.SetActive(true);
        Time.timeScale = 0;
        coroutineLaunch = true;
        yield return new WaitForSecondsRealtime(2f);
        tutoBlackScreen.SetActive(false);
        Time.timeScale = 0.25f;
        QTEDone = true;
        yield break;
    }

    void UpdateSliderPosition()
    {
        sliderAttackPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameAttack);
        setUpStartPerfectFrameAttack = Mathf.Abs((sliderAttackPerfect.eulerAngles.z - (360f* sliderPerfectSize)) / 360f);

        sliderEsquivePerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameEsquive);
        setUpStartPerfectFrameEsquive = Mathf.Abs((sliderEsquivePerfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);

        sliderBlockPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameBlock);
        setUpStartPerfectFrameBlock = Mathf.Abs((sliderBlockPerfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);
    }

     void UpdateSliderLoosePosition()
     {
        sliderAttackLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameAttack);
        setUpStartLooseFrameAttack = Mathf.Abs((sliderAttackLoose.eulerAngles.z - (360f* sliderLooseAttackSize)) / 360f);

        sliderCounterLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCounter);
        setUpStartLooseFrameCounter = Mathf.Abs((sliderCounterLoose.eulerAngles.z - (360f * sliderLooseCounterSize)) / 360f);

        sliderEsquiveLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameEsquive);
        setUpStartLooseFrameEsquive = Mathf.Abs((sliderEsquiveLoose.eulerAngles.z - (360f * sliderLooseEsquiveSize)) / 360f);

        sliderBlockLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameBlock);
        setUpStartLooseFrameBlock = Mathf.Abs((sliderBlockLoose.eulerAngles.z - (360f * sliderLooseBlockSize)) / 360f);
     }

    void SetFramePerfectSize()
    {
        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        sliderPerfectAttacSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectBlockSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectEsquiveSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        sliderAttackPerfect.GetComponent<Image>().fillAmount = sliderPerfectAttacSize;
        sliderEsquivePerfect.GetComponent<Image>().fillAmount = sliderPerfectEsquiveSize;
        sliderBlockPerfect.GetComponent<Image>().fillAmount = sliderPerfectBlockSize;
    }

    void SetFrameLooseSize()
    {
        setUpSliderLooseAttack = setUpTimerSliderNormal / (1f / sliderLooseAttackSize);

        setUpSliderLooseCounter = setUpTimerSliderNormal / (1f / sliderLooseCounterSize);

        setUpSliderLooseEsquive = setUpTimerSliderNormal / (1f / sliderLooseEsquiveSize);

        setUpSliderLooseBlock = setUpTimerSliderNormal / (1f / sliderLooseBlockSize);

        sliderLooseAttackSize = setUpSliderLooseAttack / baseSetUpTimerSliderNormal;
        sliderLooseCounterSize = setUpSliderLooseCounter / baseSetUpTimerSliderNormal;
        sliderLooseEsquiveSize = setUpSliderLooseEsquive / baseSetUpTimerSliderNormal;
        sliderLooseBlockSize = setUpSliderLooseBlock / baseSetUpTimerSliderNormal;

        sliderAttackLoose.GetComponent<Image>().fillAmount = sliderLooseAttackSize;
        sliderCounterLoose.GetComponent<Image>().fillAmount = sliderLooseCounterSize;
        sliderEsquiveLoose.GetComponent<Image>().fillAmount = sliderLooseEsquiveSize;
        sliderBlockLoose.GetComponent<Image>().fillAmount = sliderLooseBlockSize;
    }

    void ActiveManetteUI()
    {
        if (!ManetteSpriteIsActive)
            UIManager.ActiveManetteUI(true);
        else
            UIManager.ActiveManetteUI(false);
    }

    void DelayInput()
    {
        setUpTimerSliderNormal -= Time.unscaledDeltaTime;

        ActiveManetteUI();
        if(countRoundAttack>0)
        {
            TimingAttack();
        }
        if(playerCanEsquive)
        {
            TimingEsquive();
        }
        TimingBlock();

        if(setUpTimerSliderNormal <=0)
        {
            PlayerDoSomething();
        }
    }

    void TimingAttack()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderAttack(setUpTimerSliderNormal * (1f/ baseSetUpTimerSliderNormal));
            Debug.Log("Démarage Slider Attack" + (1 - setUpStartPerfectFrameAttack));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1-setUpStartPerfectFrameAttack && sliderPerfectAttacSize > 0)
            {
                Battle.canAttack = true;

                sliderPerfectAttacSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackPerfect(sliderPerfectAttacSize);

                if (Input.GetButtonDown("InteractButton"))
                {
                    UIManager.ActiveUIAttack(false, true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    AnimationEvent.attackPerfect = true;
                    Battle.myAnimator.SetBool("AttackPerfect", true);
                    canApplyDamage = false;
                    canShakeCam = false;
                    attackReussiperfect = true;
                    state = 4;
                    PerfectText.ActiveText();

                    PlayQTEValidationSound(2);
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameAttack && sliderLooseAttackSize > 0)
            {
                Battle.canAttack = false;

                sliderLooseAttackSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackLoose(sliderLooseAttackSize);

                if (Input.GetButtonDown("InteractButton"))
                {
                    UIManager.ActiveUIAttack(false, true);
                    canShakeCam = false;
                    PlayerDoSomething();
                    ResetAllSlider();
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else
            {
                Battle.canAttack = true;
                if (Input.GetButtonDown("InteractButton"))
                {
                    canShakeCam = false;
                    UIManager.ActiveUIAttack(false, true);
                    AnimationEvent.attackStandard = true;
                    Battle.myAnimator.SetBool("AttackNormal", true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    PlayQTEValidationSound(1);
                }
            }
        }
        else
        {
            Debug.Log("REset UI");
            PlayerDoSomething();
            ResetAllSlider();

            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIAttack(false, false);
            UIManager.ActiveUIBlock(false, false);
            UIManager.ActiveUICounter(false, false);
        }
    }
    void ResetAttackSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;

        sliderPerfectAttacSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        sliderLooseAttackSize = baseSliderLooseAttackSize;

        UIManager.UpdateSliderAttack(setUpTimerSliderNormal);
        UIManager.UpdateSliderAttackPerfect(sliderPerfectAttacSize);
        UIManager.UpdateSliderAttackLoose(sliderLooseAttackSize);
        startQTE = false;
    }

    void TimingEsquive()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderEsquive(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1-setUpStartPerfectFrameEsquive && sliderPerfectEsquiveSize > 0)
            {
                Battle.canEsquive = true;

                Debug.Log("Esquive");
                sliderPerfectEsquiveSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquivePerfect(sliderPerfectEsquiveSize);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") !=0)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
                        UIManager.ActiveUIEsquive(false, true, true);
                    else
                        UIManager.ActiveUIEsquive(false, false, true);

                    Debug.Log("Esquive Perfect");
                    esquivePerfect = true;
                    canApplyDamage = false;
                    canShakeCam = false;
                    esquiveReussiPerfect = true;
                    UIManager.ActiveUICounter(true, false);
                    Battle.canCounter = true;
                    startQTECounter = true;
                    state = 4;
                    PlayerDoSomething();
                    ResetAllSlider();
                    PerfectText.ActiveText();
                    PlayQTEValidationSound(2);
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameEsquive && sliderLooseEsquiveSize > 0)
            {
                Battle.canEsquive = false;

                sliderLooseEsquiveSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquiveLoose(sliderLooseEsquiveSize);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
                        UIManager.ActiveUIEsquive(false, true, true);
                    else
                        UIManager.ActiveUIEsquive(false, false, true);

                    PlayerDoSomething();
                    ResetAllSlider();
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else
            {
                Battle.canEsquive = true;

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
                {
                    if(Input.GetAxisRaw("HorizontalLeftButtonX") >0 )
                        UIManager.ActiveUIEsquive(false, true, true);
                    else
                        UIManager.ActiveUIEsquive(false, false, true);

                    canApplyDamage = false;
                    canShakeCam = false;
                    PlayerDoSomething();
                    ResetAllSlider();
                    PlayQTEValidationSound(1);
                }
            }
        }
        else
        {
            PlayerDoSomething();
            ResetAllSlider();

            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIAttack(false, false);
            UIManager.ActiveUIBlock(false, false);
            UIManager.ActiveUICounter(false, false);
        }
    }
    void ResetEsquiveSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;

        sliderPerfectEsquiveSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        sliderLooseEsquiveSize = baseSliderLooseEsquiveSize;

        UIManager.UpdateSliderEsquive(setUpTimerSliderNormal);
        UIManager.UpdateSliderEsquivePerfect(sliderPerfectEsquiveSize);
        UIManager.UpdateSliderEsquiveLoose(sliderLooseEsquiveSize);
        startQTE = false;
    }

    void TimingBlock()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderBlock(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartPerfectFrameBlock && sliderPerfectBlockSize > 0)
            {
                Battle.canBlock = true;

                sliderPerfectBlockSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderBlockPerfect(sliderPerfectBlockSize);

                if (Input.GetButtonDown("BlockButton"))
                {
                    UIManager.ActiveUIBlock(false, true);
                    Battle.myAnimator.SetBool("BlockPerfect", true);
                    Debug.Log("Block");
                    PlayerDoSomething();
                    ResetAllSlider();
                    canApplyDamage = false;
                    canShakeCam = true;
                    PerfectText.ActiveText();
                    countRoundAttack = 2;
                    PlayQTEValidationSound(2);
                }
            }
            else if(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameBlock && sliderLooseBlockSize > 0)
            {
                Battle.canBlock = false;

                sliderLooseBlockSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderBlockLoose(sliderLooseBlockSize);

                if (Input.GetButtonDown("BlockButton"))
                {
                    UIManager.ActiveUIBlock(false, true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    canShakeCam = true;
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else
            {
                Battle.canBlock = true;

                if (Input.GetButtonDown("BlockButton"))
                {
                    UIManager.ActiveUIBlock(false, true);
                    canApplyDamage = false;
                    canShakeCam = true;
                    canApplyDamageBlock = true;
                    PlayerDoSomething();
                    ResetAllSlider();
                    countRoundAttack = 1;
                    PlayQTEValidationSound(1);
                }
            }
        }
        else
        {
            Battle.canBlock = false;
            canShakeCam = true;
            PlayerDoSomething();
            ResetAllSlider();

            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIAttack(false, false);
            UIManager.ActiveUIBlock(false, false);
            UIManager.ActiveUICounter(false, false);
        }
    }
    void ResetBlockSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;

        sliderPerfectBlockSize = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        sliderLooseBlockSize = baseSliderLooseBlockSize;

        UIManager.UpdateSliderBlock(setUpTimerSliderNormal);
        UIManager.UpdateSliderBlockPerfect(sliderPerfectBlockSize);
        UIManager.UpdateSliderBlockLoose(sliderLooseBlockSize);
        startQTE = false;

        if(countRoundAttack>0)
            countRoundAttack--;
    }

    void TimingCounter()
    {
        Debug.Log("Counter");
        UIManager.UpdateSliderCounter(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

        if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameCounter && sliderLooseCounterSize > 0)
        {
            Battle.canCounter = false;

            sliderLooseCounterSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal*1.5f;
            UIManager.UpdateSliderCounterLoose(sliderLooseCounterSize);

            if (Input.GetButtonDown("InteractButton"))
            {
                UIManager.ActiveUICounter(false, true);

                canShakeCam = false;
                ResetCounterSlider();
                ReturnToStatePatrol();
                FailText.ActiveText();
                Time.timeScale = 1;
                PlayQTEValidationSound(0);
            }
        }
        else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) >= 0 && startQTECounter)
        {
            Battle.canCounter = true;

            if (Input.GetButtonDown("InteractButton"))
            {
                UIManager.ActiveUICounter(false, true);

                canShakeCam = false;
                counterReussi = true;
                ResetCounterSlider();
                PlayQTEValidationSound(1);
            }
        }
        else
        {
            Battle.canCounter = false;
            PlayerDoSomething();
            ResetCounterSlider();
            ReturnToStatePatrol();
            thisSelected = false;
            canShakeCam = false;

            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIAttack(false, false);
            UIManager.ActiveUIBlock(false, false);
            UIManager.ActiveUICounter(false, false);
        }
    }
    void ResetCounterSlider()
    {
        StopPlayQTETimerSound();
        StopPlayBulletTimeSound();

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / 4f;

        sliderLooseCounterSize = baseSliderLooseCounterSize;

        UIManager.UpdateSliderCounter(setUpTimerSliderNormal);
        UIManager.UpdateSliderCounterLoose(sliderLooseCounterSize);
        //UIManager.ActiveManetteUI(false);
        startQTECounter = false;
    }

    void ResetAllSlider()
    {
        StopPlayQTETimerSound();
        StopPlayBulletTimeSound();

        ResetBlockSlider();
        ResetEsquiveSlider();
        ResetAttackSlider();

        SetUpEndFenetreAttack();
    }

    void StateWaitingPlayer()
    {
        agent.enabled = true;
        agent.angularSpeed = 0;

        canApplyDamageBlock = false;
        canApplyDamage = true;
        paradeReussi = false;
        counterReussi = false;
        attackReussiperfect = false;
        esquiveReussiPerfect = false;

        RandomAttack();

        if(!randomAttack && thisSelected && PlayerHp.hp >0)
        {
            RandomAttack();
        }

        if(PlayerHp.hp <= 0)
        {
            startBattle = false;
            ReturnToStatePatrol();
        }

        SmoothLookAt(player);

        /* if (distPlayer < 10f)
             transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - player.position.x, transform.position.y, transform.position.z - player.position.z), 1f * Time.deltaTime);
         else if(distPlayer > 12f)
             transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), 1 * Time.deltaTime);*/

        if (distPlayer < 10f)
        {
            Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-30, transform.right)* -transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask); // Si l'angle est plus petit que l'angle de vision du bot, on tire un rayon vers le joueur
            Debug.DrawRay(this.transform.position, Quaternion.AngleAxis(-30, transform.right) * -transform.TransformDirection(Vector3.forward) * 100f, Color.blue);

            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);
                //Debug.LogWarning(hit.transform.gameObject + "  /  " + transform.position + "  /  " + hit.point);
            }
        }
        else if (distPlayer > 12f)
        {
            agent.SetDestination(player.position);
        }

        if (time >=0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if (!moveRight)
            {
                moveRight = true;
            }
            else
            {
                moveRight = false;
            }

            time = 2;
        }
    }

    void RandomAttack()
    {
        if(!thisSelected)
        {
            randomTimeBeforeAttack = Random.Range(2, 5);
        }
        
        if(randomTimeBeforeAttack >0)
        {
            randomTimeBeforeAttack -= Time.deltaTime;
        }
        else
        {
            retrunState1 = false;
            randomTimeBeforeAttack = Random.Range(2, 5);
            state = 2;
            canAttack = true;

            if(Random.Range(0, 2) == 0)
            {
                playerCanEsquive = true;
            }
            else
            {
                playerCanEsquive = false;
            }
        }
    }

    void StatePatrol()
    {
        if(startBattle)
        {
            state = 1;
            patrolIsActive = false;
        }
        else
        {
            if(!patrolIsActive)
            {
                StartCoroutine("Walk");
                patrolIsActive = true;
                agent.angularSpeed = 120f;
            }
        }
    }

    IEnumerator Walk()
    {
        while (gameObject.activeSelf && state ==0)
        {
            Debug.Log("ennemi patrolling");

            Vector2 pos2D = Random.insideUnitCircle * circleRadius;
            Vector3 pos3D = basePos + new Vector3(pos2D.x, 0, pos2D.y);

            agent.SetDestination(pos3D);

            if (!agent.isStopped)
            {
                Debug.Log("!is stopped");
                yield return new WaitForEndOfFrame();
            }
            else
            {
                Debug.Log("is stopped");
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void StateAttack()
    {
        agent.enabled = false;
        findPosition = false;
        PlayerController.ennemi = this.transform;
        AnimationEvent.ennemi = this.gameObject;

        if (canAttack)
            LaunchAttack();
    }
    void StateHurt()
    {
        isAttacking = false;
        myAnimator.SetBool("Hit", true);
        myAnimator.SetBool("Attack", false);
    }

    void StateWaitCounter()
    {
        if(Battle.isCounter)
        {
            state = 3;
        }
        else if(Battle.isAttacking)
        {
            state = 3;
        }
    }

    void SmoothLookAt(Transform target)
    {/*
        lookPlayer = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookPlayer, 10 * Time.deltaTime);*/

        Vector3 relativePos = player.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 0.5f);
        transform.rotation = rotation;
    }

    void LaunchAttack()
    {
        if (distPlayer <= 5f && !isAttacking)
        {
            battleScript.isAttacked = true;
            if (!tutoDone)
            {
                QTEDone = false;
                coroutineLaunch = false;
            }
            StartCoroutine("StartAttack");
        }

        if(distPlayer > 5f)
        {
            if (!isAttacking)
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), 7 * Time.deltaTime);
        }
        else
        {
            moveToPlayerBeforeAttack = false;
        }
    }

    IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
        StopCoroutine("DelayBeforeAttack");
        yield break;
    }

    IEnumerator StartAttack()
    {
        canAttack = false;
        launchAttack = false;
        isAttacking = true;
        yield return new WaitForSeconds(0.1f);
        myAnimator.SetBool("Attack", true);
        BeginAttack();
        StopLookAt();
        attackReussiperfect = false;
        StopCoroutine("StartAttack");
        yield break;
    }

    void StopLookAt()
    {
       // lookat = false;
    }

    void LookAtPlayer()
    {
        StartCoroutine("ReLookAtPlayer");
        StartCoroutine("DelayBeforeNextAttack");
        goBack = false;
    }

    void PlayerDoSomething()
    {
        StartCoroutine("BlockPlayerAction");
    }

    IEnumerator BlockPlayerAction()
    {
        if (!esquiveReussiPerfect)
        {
            //UIManager.ActiveManetteUI(false);
            PlayQTETimerSound();
        }

        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log("Desable UI");

        Battle.canEsquive = false;
        Battle.canAttack = false;
        Battle.canBlock = false;
        yield break;
    }

    // fonction en lien avec les action du joueur
    void ReturnToStatePatrol()
    {
        state = 0;
        thisSelected = false;
    }

    void TimeScaleNormal()
    {
        Time.timeScale = 1f;
    }

    // Animation event
    void BeginAttack()
    {
        isAttacking = true;
        Invoke("SetUpStartActionPlayer", setUpStartActionPlayer);
    }

    void EndAttack()
    {
        if (!attackReussiperfect && !esquiveReussiPerfect)
            ReturnToStatePatrol();
        myAnimator.SetBool("Attack", false);
        isAttacking = false;
    }

    void SetUpStartActionPlayer()
    {
        Debug.Log("Choix action");

        if(!playerCanEsquive)
        {
            UIManager.ActiveUIEsquive(false, false, false);
            UIManager.ActiveUIEsquive(false, true, false);
            Battle.canEsquive = false;
        }
        else
        {
            UIManager.ActiveUIEsquive(true, true, false);
            UIManager.ActiveUIEsquive(true, false, false);
            Battle.canEsquive = true;
        }

        if (countRoundAttack>0)
        {
            UIManager.ActiveUIAttack(true, false);
            Battle.canAttack = true;
        }
        else
        {
            UIManager.ActiveUIAttack(false, false);
            Battle.canAttack = false;
        }

        UIManager.ActiveManetteUI(true);
        UIManager.ActiveUIBlock(true, false);

        Battle.canBlock = true;

        if(QTEDone)
            Time.timeScale = 0.25f;

        startQTE = true;
        playerAction = true;

        PlayBulletTimeSound();
        PlayQTETimerSound();
    }

    void SetUpEndFenetreAttack()
    {
        Time.timeScale = 1f;
    }

    void EndHit()
    {
        myAnimator.SetBool("Hit", false);
        ReturnToStatePatrol();
    }

    void ApplyDamageToPlayer()
    {
        if(canApplyDamage && canShakeCam)
        {
            Battle.myAnimator.SetBool("IsHit", true);

            PlayerHp.TakeDamage(damage);
            Debug.LogError(SoundManager.soundAndVolumePlayerBattleStatic[0]);
            SoundManager.PlaySoundPlayerBattle(playerAudioSource, SoundManager.soundAndVolumePlayerBattleStatic[0]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersFailStatic, ShakeCam.shakeCamParametersFailStatic[0].axeShake, ShakeCam.shakeCamParametersFailStatic[0].amplitude,
                       ShakeCam.shakeCamParametersFailStatic[0].frequence, ShakeCam.shakeCamParametersFailStatic[0].duration);

            AnimationEvent.Hit();
        }

        if(canApplyDamageBlock && canShakeCam)
        {
            Battle.myAnimator.SetBool("BlockNormal", true);
            
            PlayerHp.TakeDamage(blockDamage);
            SoundManager.PlaySoundPlayerBattle(playerAudioSource, SoundManager.soundAndVolumePlayerBattleStatic[4]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersBlockNormalStatic, ShakeCam.shakeCamParametersBlockNormalStatic[0].axeShake, ShakeCam.shakeCamParametersBlockNormalStatic[0].amplitude,
                        ShakeCam.shakeCamParametersBlockNormalStatic[0].frequence, ShakeCam.shakeCamParametersBlockNormalStatic[0].duration);
        }
        
        if(!canApplyDamageBlock && !canApplyDamage && canShakeCam)
        {
            Battle.myAnimator.SetBool("BlockPerfectEnd", true);

            SoundManager.PlaySoundPlayerBattle(playerAudioSource, SoundManager.soundAndVolumePlayerBattleStatic[5]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamparametersBlockPerfectStatic, ShakeCam.shakeCamparametersBlockPerfectStatic[0].axeShake, ShakeCam.shakeCamparametersBlockPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamparametersBlockPerfectStatic[0].frequence, ShakeCam.shakeCamparametersBlockPerfectStatic[0].duration);
        }
    }


    // Launch Sound
    void PlayBulletTimeSound()
    {
        SoundManager.PlaySound2DContinue(bulletTimeAudioSource, SoundManager.soundAndVolume2DStatic[3], true);
    }
    void PlayQTETimerSound()
    {
        SoundManager.PlaySound2DContinue(qteTimerAudioSource, SoundManager.soundAndVolume2DStatic[4], true);
    }
    void PlayQTEValidationSound(int reussite)
    {
        SoundManager.PlaySoundPlayerInteraction(qteValidationAudioSource, SoundManager.soundAndVolume2DStatic[reussite]);
    }

    // Stop sound
    void StopPlayBulletTimeSound()
    {
        SoundManager.PlaySound2DContinue(bulletTimeAudioSource, SoundManager.soundAndVolume2DStatic[3], false);
    }
    void StopPlayQTETimerSound()
    {
        SoundManager.PlaySound2DContinue(qteTimerAudioSource, SoundManager.soundAndVolume2DStatic[4], false);
    }
}