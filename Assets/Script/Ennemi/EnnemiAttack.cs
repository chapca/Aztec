using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

//[ExecuteInEditMode]
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

    [Header("MANUELLE Defini la dur?e du qte en scd(min:0 / max:1)")]
    [Range(0f, 10f)]
    [SerializeField] float setUpStartActionPlayer;
    [Range(0.0f, 10f)]
    [SerializeField] float setUpEndActionPlayer;

    [Header("MANUELLE Defini la position des frames perfect(min:90? / max:359.99?)")]
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameAttack;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameEsquive;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameBlock;

    [Header("MANUELLE Defini la position des frames Loose(min:90? / max:359.99?)")]
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameAttack;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameCounter;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameEsquive;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameBlock;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameAttack2;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameCounter2;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameEsquive2;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameBlock2;

    [Header("AUTO Valeur ? laquel la frame perfect d?mare par rapport au slider normal")]
    [Range(0.0f, 1f)]
     float setUpStartPerfectFrameAttack;
    [Range(0.0f, 1f)]
     float setUpStartPerfectFrameEsquive;
    [Range(0.0f, 1f)]
     float setUpStartPerfectFrameBlock;

    [Header("AUTO Valeur ? laquel la frame loose d?mare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameAttack;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameCounter;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameEsquive;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameBlock;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameAttack2;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameCounter2;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameEsquive2;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartLooseFrameBlock2;

    [Header("AUTO dur?e au slider normal")]
    [Range(0.0f, 3f)]
     float baseSetUpTimerSliderNormal;
    [Range(0.0f, 3f)]
     float setUpTimerSliderNormal;
    [Range(0.0f, 3f)]
     float setUpSliderPerfect;

    [Header("AUTO dur?e au slider loose")]
    [Range(0.0f, 3f)]
    float setUpSliderLooseAttack;
    [Range(0.0f, 3f)]
     float setUpSliderLooseCounter;
    [Range(0.0f, 3f)]
     float setUpSliderLooseEsquive;
    [Range(0.0f, 3f)]
     float setUpSliderLooseBlock;
    [Range(0.0f, 3f)]
    float setUpSliderLooseAttack2;
    [Range(0.0f, 3f)]
    float setUpSliderLooseCounter2;
    [Range(0.0f, 3f)]
    float setUpSliderLooseEsquive2;
    [Range(0.0f, 3f)]
    float setUpSliderLooseBlock2;

    [Header("Reference slider")]
    [SerializeField] Transform sliderAttackPerfect, sliderAttackNormal, sliderAttackLoose, sliderAttackLoose2, sliderCounterLoose, sliderCounterLoose2;
    [SerializeField] Transform sliderEsquivePerfect, sliderEsquiveNormal, sliderEsquiveLoose, sliderEsquiveLoose2;
    [SerializeField] Transform sliderBlockPerfect, sliderBlockNormal, sliderBlockLoose, sliderBlockLoose2;

    //(fillAmountSliderNormal *360f) +90f = angleSliderPerfect
    //(fillAmountSliderNormal *360f) = angleSliderPerfect - 90f
    //(fillAmountSliderNormal) = (angleSliderPerfect - 90f) /360f

    [Header("MANUAL Taille/dur?e slider")]
    [Range(0.0f, 1f)] [SerializeField] float sliderPerfectSize;
    float sliderPerfectAttacSize;
    float sliderPerfectEsquiveSize;
    float sliderPerfectBlockSize;

    [Header("MANUAL Taille/dur?e slider Loose")]
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseCounterSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseAttackSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseEsquiveSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseBlockSize;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseCounterSize2;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseAttackSize2;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseEsquiveSize2;
    [Range(0.0f, 1f)] [SerializeField] float sliderLooseBlockSize2;

    [Range(0.0f, 1f)]  float baseSliderLooseCounterSize;
    [Range(0.0f, 1f)]  float baseSliderLooseAttackSize;
    [Range(0.0f, 1f)]  float baseSliderLooseEsquiveSize;
    [Range(0.0f, 1f)]  float baseSliderLooseBlockSize;
    [Range(0.0f, 1f)] float baseSliderLooseCounterSize2;
    [Range(0.0f, 1f)] float baseSliderLooseAttackSize2;
    [Range(0.0f, 1f)] float baseSliderLooseEsquiveSize2;
    [Range(0.0f, 1f)] float baseSliderLooseBlockSize2;

    [Header("Execute code en hors Game (DESACTIVER AVANT DE LANCER)")]
    [SerializeField] bool activeThisinEditor, ManetteSpriteIsActive;

    [Header("D?gat du mob")]
    [SerializeField] float damage, blockDamage;

    [Range(0.0f, 2f)]
    static public int countRoundAttack;

    //bool playerCanEsquive;

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

    [SerializeField] static GameObject tutoBlackScreen;

    [Header("Change la taille du slider quand on appuie au bon moment")]
    [SerializeField] Vector3 maxSize;
    [SerializeField] Vector3 baseSize;

    static public bool esquiveRight, esquiveLeft;

    bool coroutineSoundLaunched;

    void Start()
    {
        ennemiHp = GetComponent<EnnemiHp>();
        ennemiManager = GetComponentInParent<EnnemiManager>();
        //myAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        myAudioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        battleScript = GameObject.FindWithTag("Player").GetComponent<Battle>();

        state = 0;

       /* if(!activeThisinEditor)
        {
            setUpStartActionPlayer /= 10;
            setUpEndActionPlayer /= 10;
        }*/

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

        /*sliderLooseAttackSize = setUpSliderLooseAttack / baseSetUpTimerSliderNormal;
        sliderLooseCounterSize = setUpSliderLooseCounter / baseSetUpTimerSliderNormal;
        sliderLooseEsquiveSize = setUpSliderLooseEsquive / baseSetUpTimerSliderNormal;
        sliderLooseBlockSize = setUpSliderLooseBlock / baseSetUpTimerSliderNormal;*/

        baseSliderLooseAttackSize = sliderLooseAttackSize;
        baseSliderLooseCounterSize = sliderLooseCounterSize;
        baseSliderLooseEsquiveSize = sliderLooseEsquiveSize;
        baseSliderLooseBlockSize = sliderLooseBlockSize;

        //
        setUpSliderLooseAttack2 = setUpTimerSliderNormal / (1f / sliderPerfectAttacSize);
        setUpSliderLooseCounter2 = setUpTimerSliderNormal / (1f / sliderLooseCounterSize);
        setUpSliderLooseEsquive2 = setUpTimerSliderNormal / (1f / sliderLooseEsquiveSize);
        setUpSliderLooseBlock2 = setUpTimerSliderNormal / (1f / sliderLooseBlockSize);

        /*sliderLooseAttackSize2 = setUpSliderLooseAttack2 / baseSetUpTimerSliderNormal;
        sliderLooseCounterSize2 = setUpSliderLooseCounter2 / baseSetUpTimerSliderNormal;
        sliderLooseEsquiveSize2 = setUpSliderLooseEsquive2 / baseSetUpTimerSliderNormal;
        sliderLooseBlockSize2 = setUpSliderLooseBlock2 / baseSetUpTimerSliderNormal;*/

        baseSliderLooseAttackSize2 = sliderLooseAttackSize2;
        baseSliderLooseCounterSize2 = sliderLooseCounterSize2;
        baseSliderLooseEsquiveSize2 = sliderLooseEsquiveSize2;
        baseSliderLooseBlockSize2 = sliderLooseBlockSize2;

        bulletTimeAudioSource = GameObject.Find("BattleBulletTimeMusic").GetComponent<AudioSource>();
        qteTimerAudioSource = GameObject.Find("QTETimerMusic").GetComponent<AudioSource>();
        qteValidationAudioSource = GameObject.Find("QTEValidationMusic").GetComponent<AudioSource>();
        playerAudioSource = GameObject.Find("EmptyPlayer").GetComponent<AudioSource>();

        if(tutoBlackScreen == null)
            tutoBlackScreen = GameObject.Find("TutoBlackScreen");

        tutoBlackScreen.GetComponent<Image>().enabled = false;

        basePos = transform.position;

        if (Random.Range(0, 2) == 0)
        {
            esquiveRight = true;
            esquiveLeft = false;
        }
        else
        {
            esquiveLeft = true;
            esquiveRight = false;
        }

        /*UpdateSliderPosition();
        UpdateSliderLoosePosition();

        SetFramePerfectSize();
        SetFrameLooseSize();*/
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

        sliderAttackLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameAttack2);
        sliderCounterLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCounter2);
        sliderEsquiveLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameEsquive2);
        sliderBlockLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameBlock2);
    }

    void Update()
    {
        /*if (activeThisinEditor)
        {
            // UIManager.AjusteSliderEsquive();

            UpdateSliderPosition();
            UpdateSliderLoosePosition();

            SetFramePerfectSize();
            SetFrameLooseSize();
        }*/

        if (thisSelected && !launchQTE)
        {
            UIManager.AjusteSliderEsquive();

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


        if(distPlayer < 10 || ennemiManager.startBattle)
        {
            if(PlayerHp.hp > 0 && !battleScript.isAttacked)
            {
                ennemiManager.ActiveEnnemiFightState();
                ennemiManager.startBattle = true;
            }
        }
        else if(!startBattle)
        {
            state = 0;
        }

        if(startBattle)
        {
            myAnimator.SetBool("Walk", false);
            myAnimator.SetBool("Fight", true);
            startBattle = true;
            ennemiManager.startBattle = true;
        }

        switch (state)
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
                StartCoroutine("CoolDownDone");
            }
        }

        if (startQTE && QTEDone)
        {
            activeThisinEditor = false;
            DelayInput();
        }
        if (startQTECounter)
        {
            activeThisinEditor = false;

            setUpTimerSliderNormal -= Time.unscaledDeltaTime*1.5f;
            TimingCounter();
        }
    }

    IEnumerator CoolDownDone()
    {
        Battle.canEsquive = false;

        if (!tutoBlackScreen.GetComponent<Image>().enabled)
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

        //
        sliderAttackLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameAttack2);
        setUpStartLooseFrameAttack2 = Mathf.Abs((sliderAttackLoose2.eulerAngles.z - (360f * sliderLooseAttackSize2)) / 360f);

        sliderCounterLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCounter2);
        setUpStartLooseFrameCounter2 = Mathf.Abs((sliderCounterLoose2.eulerAngles.z - (360f * sliderLooseCounterSize2)) / 360f);

        sliderEsquiveLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameEsquive2);
        setUpStartLooseFrameEsquive2 = Mathf.Abs((sliderEsquiveLoose2.eulerAngles.z - (360f * sliderLooseEsquiveSize2)) / 360f);

        sliderBlockLoose2.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameBlock2);
        setUpStartLooseFrameBlock2 = Mathf.Abs((sliderBlockLoose2.eulerAngles.z - (360f * sliderLooseBlockSize2)) / 360f);
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

        Debug.LogError("Fix Slider");

        setUpSliderLooseAttack2 = setUpTimerSliderNormal / (1f / sliderLooseAttackSize2);

        setUpSliderLooseCounter2 = setUpTimerSliderNormal / (1f / sliderLooseCounterSize2);

        setUpSliderLooseEsquive2 = setUpTimerSliderNormal / (1f / sliderLooseEsquiveSize2);

        setUpSliderLooseBlock2 = setUpTimerSliderNormal / (1f / sliderLooseBlockSize2);

        sliderLooseAttackSize2 = setUpSliderLooseAttack2 / baseSetUpTimerSliderNormal;
        sliderLooseCounterSize2 = setUpSliderLooseCounter2 / baseSetUpTimerSliderNormal;
        sliderLooseEsquiveSize2 = setUpSliderLooseEsquive2 / baseSetUpTimerSliderNormal;
        sliderLooseBlockSize2 = setUpSliderLooseBlock2 / baseSetUpTimerSliderNormal;

        sliderAttackLoose2.GetComponent<Image>().fillAmount = sliderLooseAttackSize2;
        sliderCounterLoose2.GetComponent<Image>().fillAmount = sliderLooseCounterSize2;
        sliderEsquiveLoose2.GetComponent<Image>().fillAmount = sliderLooseEsquiveSize2;
        sliderBlockLoose2.GetComponent<Image>().fillAmount = sliderLooseBlockSize2;
    }

    void ActiveManetteUI()
    {
        if (!ManetteSpriteIsActive)
            UIManager.ActiveManetteUI(true);
        else
            UIManager.ActiveManetteUI(false);
    }

    [SerializeField] bool launchQTE, coroutineBreakQTE;
    void DelayInput()
    {
        ActiveManetteUI();

        if (!launchQTE && !coroutineBreakQTE)
            StartCoroutine("BreakTimeBeforeLauncheQte");
        else if(launchQTE && coroutineBreakQTE)
        {
            setUpTimerSliderNormal -= Time.unscaledDeltaTime;

            if (countRoundAttack > 0)
            {
                TimingAttack();
            }

            if (!Battle.wallDetectRight || !Battle.wallDetectLeft)
                TimingEsquive();

            TimingBlock();

            if (setUpTimerSliderNormal <= 0)
            {
                countRoundAttack--;
                UIManager.ActiveUINbrCounterAttack(false, countRoundAttack);
                PlayerDoSomething();
            }
        }
    }

    IEnumerator BreakTimeBeforeLauncheQte()
    {
        UpdateSliderPosition();
        UpdateSliderLoosePosition();

        SetFramePerfectSize();
        SetFrameLooseSize();
        UIManager.AjusteSliderEsquive();
        coroutineBreakQTE = true;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0.25f;
        launchQTE = true;
        yield break;
    }

    void TimingAttack()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderAttack(setUpTimerSliderNormal * (1f/ baseSetUpTimerSliderNormal));
            Debug.Log("D?marage Slider Attack" + (1 - setUpStartPerfectFrameAttack));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1-setUpStartPerfectFrameAttack && sliderPerfectAttacSize > 0)
            {
                Battle.canAttack = true;

                sliderPerfectAttacSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackPerfect(sliderPerfectAttacSize);

                if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                {
                    UIManager.ActiveUIAttack(false, true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    AnimationEvent.attackPerfect = true;
                    Battle.myAnimator.SetBool("AttackPerfect", true);
                    countRoundAttack--;
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

                if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                {
                    UIManager.ActiveUIAttack(false, true);
                    canShakeCam = false;
                    countRoundAttack--;
                    PlayerDoSomething();
                    ResetAllSlider();
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameAttack2 && sliderLooseAttackSize2 > 0)
            {
                Battle.canAttack = false;

                sliderLooseAttackSize2 -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackLoose2(sliderLooseAttackSize2);

                if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                {
                    UIManager.ActiveUIAttack(false, true);
                    canShakeCam = false;
                    countRoundAttack--;
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
                if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                {
                    canShakeCam = false;
                    UIManager.ActiveUIAttack(false, true);
                    AnimationEvent.attackStandard = true;
                    Battle.myAnimator.SetBool("AttackNormal", true);
                    countRoundAttack--;
                    NormalTxt.ActiveText();
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
        sliderLooseAttackSize2 = baseSliderLooseAttackSize2;

        UIManager.UpdateSliderAttack(setUpTimerSliderNormal);
        UIManager.UpdateSliderAttackPerfect(sliderPerfectAttacSize);
        UIManager.UpdateSliderAttackLoose(sliderLooseAttackSize);
        UIManager.UpdateSliderAttackLoose2(sliderLooseAttackSize2);
        startQTE = false;
    }

    void TimingEsquive()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderEsquive(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            Debug.Log(Battle.canEsquive);

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1-setUpStartPerfectFrameEsquive && sliderPerfectEsquiveSize > 0)
            {
                Battle.canEsquive = true;

                Debug.Log("Esquive");
                sliderPerfectEsquiveSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquivePerfect(sliderPerfectEsquiveSize);

                if(esquiveRight)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManager.ActiveUIEsquive(false, true, true);

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

                        StartCoroutine("SwitchEsquiveSide");
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                    {
                        UIManager.ActiveUIEsquive(false, true, false);
                        PlayerDoSomething();
                        ResetAllSlider();
                        canShakeCam = true;
                        countRoundAttack--;
                        FailText.ActiveText();
                        Time.timeScale = 1;
                        PlayQTEValidationSound(0);
                    }
                }
                else if(esquiveLeft)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                    {
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

                        StartCoroutine("SwitchEsquiveSide");
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManager.ActiveUIEsquive(false, false, false);

                        PlayerDoSomething();
                        ResetAllSlider();
                        countRoundAttack--;
                        canShakeCam = true;
                        FailText.ActiveText();
                        Time.timeScale = 1;
                        PlayQTEValidationSound(0);
                    }
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameEsquive && sliderLooseEsquiveSize > 0)
            {
                Battle.canEsquive = false;

                sliderLooseEsquiveSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquiveLoose(sliderLooseEsquiveSize);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0 || Input.GetButtonDown("CancelButton") || Input.GetButtonDown("HealthButton"))
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                        UIManager.ActiveUIEsquive(false, true, true);
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                        UIManager.ActiveUIEsquive(false, false, true);

                    PlayerDoSomething();
                    ResetAllSlider();
                    countRoundAttack--;
                    canShakeCam = true;
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameEsquive2 && sliderLooseEsquiveSize2 > 0)
            {
                Battle.canEsquive = false;

                sliderLooseEsquiveSize2 -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquiveLoose2(sliderLooseEsquiveSize2);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0 || Input.GetButtonDown("CancelButton") || Input.GetButtonDown("HealthButton"))
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                        UIManager.ActiveUIEsquive(false, true, true);
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                        UIManager.ActiveUIEsquive(false, false, true);

                    PlayerDoSomething();
                    ResetAllSlider();
                    countRoundAttack--;
                    canShakeCam = true;
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else
            {
                Battle.canEsquive = true;

                if(esquiveRight)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManager.ActiveUIEsquive(false, true, true);

                        canApplyDamage = false;
                        canShakeCam = false;
                        countRoundAttack--;
                        NormalTxt.ActiveText();
                        PlayerDoSomething();
                        ResetAllSlider();
                        PlayQTEValidationSound(1);
                        StartCoroutine("SwitchEsquiveSide");
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                    {
                        UIManager.ActiveUIEsquive(false, true, false);

                        PlayerDoSomething();
                        ResetAllSlider();
                        canShakeCam = true;
                        countRoundAttack--;
                        FailText.ActiveText();
                        Time.timeScale = 1;
                        PlayQTEValidationSound(0);
                    }
                }
                else if (esquiveLeft)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                    {
                        UIManager.ActiveUIEsquive(false, false, true);

                        canApplyDamage = false;
                        canShakeCam = false;
                        countRoundAttack--;
                        NormalTxt.ActiveText();
                        PlayerDoSomething();
                        ResetAllSlider();
                        PlayQTEValidationSound(1);
                        StartCoroutine("SwitchEsquiveSide");
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManager.ActiveUIEsquive(false, false, false);

                        PlayerDoSomething();
                        ResetAllSlider();
                        countRoundAttack--;
                        FailText.ActiveText();
                        Time.timeScale = 1;
                        PlayQTEValidationSound(0);
                    }
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
        sliderLooseEsquiveSize2 = baseSliderLooseEsquiveSize2;

        UIManager.UpdateSliderEsquive(setUpTimerSliderNormal);
        UIManager.UpdateSliderEsquivePerfect(sliderPerfectEsquiveSize);
        UIManager.UpdateSliderEsquiveLoose2(sliderLooseEsquiveSize2);
        startQTE = false;
    }
    IEnumerator SwitchEsquiveSide()
    {
        yield return new WaitForSeconds(1f);
        if (esquiveLeft)
        {
            esquiveRight = true;
            esquiveLeft = false;
            yield break;
        }
        else
        {
            esquiveLeft = true;
            esquiveRight = false;
            yield break;
        }
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

                if (Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
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

                if (Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
                {
                    UIManager.ActiveUIBlock(false, true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    canShakeCam = true;
                    countRoundAttack--;
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameBlock2 && sliderLooseBlockSize2 > 0)
            {
                Battle.canBlock = false;

                sliderLooseBlockSize2 -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderBlockLoose2(sliderLooseBlockSize2);

                if (Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
                {
                    UIManager.ActiveUIBlock(false, true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    canShakeCam = true;
                    countRoundAttack--;
                    FailText.ActiveText();
                    Time.timeScale = 1;
                    PlayQTEValidationSound(0);
                }
            }
            else
            {
                Battle.canBlock = true;

                if (Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
                {
                    UIManager.ActiveUIBlock(false, true);
                    canApplyDamage = false;
                    canShakeCam = true;
                    canApplyDamageBlock = true;
                    PlayerDoSomething();
                    ResetAllSlider();
                    NormalTxt.ActiveText();
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
        sliderLooseBlockSize2 = baseSliderLooseBlockSize2;

        UIManager.UpdateSliderBlock(setUpTimerSliderNormal);
        UIManager.UpdateSliderBlockPerfect(sliderPerfectBlockSize);
        UIManager.UpdateSliderBlockLoose(sliderLooseBlockSize);
        UIManager.UpdateSliderBlockLoose2(sliderLooseBlockSize2);
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

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                UIManager.ActiveUICounter(false, true);

                canShakeCam = false;
                ResetCounterSlider();
                ReturnToStatePatrol();
                countRoundAttack--;
                FailText.ActiveText();
                Time.timeScale = 1;
                PlayQTEValidationSound(0);
            }
        }
        else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameCounter2 && sliderLooseCounterSize2 > 0)
        {
            Battle.canCounter = false;

            sliderLooseCounterSize2 -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal * 1.5f;
            UIManager.UpdateSliderCounterLoose2(sliderLooseCounterSize2);

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                UIManager.ActiveUICounter(false, true);

                canShakeCam = false;
                ResetCounterSlider();
                ReturnToStatePatrol();
                countRoundAttack--;
                FailText.ActiveText();
                Time.timeScale = 1;
                PlayQTEValidationSound(0);
            }
        }
        else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) >= 0 && startQTECounter)
        {
            Battle.canCounter = true;

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                UIManager.ActiveUICounter(false, true);
                PerfectText.ActiveText();
                canShakeCam = false;
                counterReussi = true;
                countRoundAttack--;
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
        sliderLooseCounterSize2 = baseSliderLooseCounterSize2;

        UIManager.UpdateSliderCounter(setUpTimerSliderNormal);
        UIManager.UpdateSliderCounterLoose(sliderLooseCounterSize);
        UIManager.UpdateSliderCounterLoose2(sliderLooseCounterSize2);
        //UIManager.ActiveManetteUI(false);
        startQTECounter = false;
        launchQTE = false;
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
        activeThisinEditor = false;

        agent.enabled = true;
        agent.angularSpeed = 0;

        canApplyDamageBlock = false;
        canApplyDamage = true;
        paradeReussi = false;
        counterReussi = false;
        attackReussiperfect = false;
        esquiveReussiPerfect = false;

        if(PlayerHp.hp <= 0)
        {
            startBattle = false;
            ReturnToStatePatrol();
        }

        SmoothLookAt(player);

        if (distPlayer < 10f)
        {
            Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-30, transform.right)* -transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask); // Si l'angle est plus petit que l'angle de vision du bot, on tire un rayon vers le joueur
            Debug.DrawRay(this.transform.position, Quaternion.AngleAxis(-30, transform.right) * -transform.TransformDirection(Vector3.forward) * 100f, Color.blue);

            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);
                if(transform.position != hit.point)
                {
                    myAnimator.SetBool("Walk", false);
                    myAnimator.SetBool("WalkBack", true);
                }
                else
                {
                    myAnimator.SetBool("WalkBack", false);
                }
            }
        }
        else if (distPlayer > 12f)
        {
            agent.SetDestination(player.position);
            myAnimator.SetBool("WalkBack", false);
            myAnimator.SetBool("Walk", true);
        }

        RandomAttack();

        if (!randomAttack && thisSelected && PlayerHp.hp > 0)
        {
            RandomAttack();
        }
    }
    [Range(0,4)]
    [SerializeField] float delayBeforeAttack;
    void RandomAttack()
    {
        if(!thisSelected)
        {
            randomTimeBeforeAttack = delayBeforeAttack;
        }
        
        if(randomTimeBeforeAttack >0)
        {
            randomTimeBeforeAttack -= Time.deltaTime;
        }
        else
        {
            retrunState1 = false;
            randomTimeBeforeAttack = delayBeforeAttack;
            state = 2;
            canAttack = true;
        }
    }

    void StatePatrol()
    {
        agent.enabled = true;

        if (agent.velocity.magnitude > 0)
        {
            myAnimator.SetBool("Walk", true);
        }
        else
        {
            myAnimator.SetBool("Walk", false);
            Debug.Log("Stop");
        }

        if (startBattle)
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

        if(!coroutineSoundLaunched)
            StartCoroutine("LaunchEnnemiIdleSound");

    }

    IEnumerator LaunchEnnemiIdleSound()
    {
        coroutineSoundLaunched = true;
        SoundManager.PlaySoundEnnemiIdle(myAudioSource, SoundManager.soundAndVolumeEnnemiIdleStatic[Random.Range(0, 2)]);
        yield return new WaitForSeconds(Random.Range(3, 5));

        if(state ==0)
        {
            coroutineSoundLaunched = false;
            yield break;
        }
        else
        {
            coroutineSoundLaunched = false;
            yield break;
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
            yield return new WaitForSeconds((int)Random.Range(10, 15));
        }
    }

    void StateAttack()
    {
        //activeThisinEditor = true;
        //agent.enabled = false;
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

        relativePos.x = player.position.x - transform.position.x;
        relativePos.y = 0;
        relativePos.z = player.position.z - transform.position.z;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 0.5f);
        transform.rotation = rotation;
    }

    void LaunchAttack()
    {
        if (distPlayer <= 6f && !isAttacking)
        {
            battleScript.isAttacked = true;
            if (!tutoDone)
            {
                QTEDone = false;
                coroutineLaunch = false;
            }
            StartCoroutine("StartAttack");
        }

        if(distPlayer > 6f)
        {
            if (!isAttacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), 7 * Time.deltaTime);
                myAnimator.SetBool("Walk", true);
            }
        }
        else
        {
            moveToPlayerBeforeAttack = false;
            myAnimator.SetBool("Walk", false);
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
        agent.enabled = false;
        canAttack = false;
        launchAttack = false;
        isAttacking = true;

        myAnimator.SetBool("Walk", false);
        myAnimator.SetBool("WalkBack", false);
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

        if(!esquiveReussiPerfect)
            launchQTE = false;

        coroutineBreakQTE = false;
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

    void BeginAttack()
    {
        isAttacking = true;
        Invoke("SetUpStartActionPlayer", setUpStartActionPlayer);
    }

    // Animation event

    public void StartHit()
    {
        myAnimator.SetBool("Attack", false);
        isAttacking = false;
    }

    public void EndAttack()
    {
        esquivePerfect = false;
        myAnimator.SetBool("Attack", false);
        isAttacking = false;

        if (!attackReussiperfect && !esquiveReussiPerfect)
            ReturnToStatePatrol();
    }
    
    void SetUpStartActionPlayer()
    {
        Debug.Log("Choix action");

        UIManager.ActiveUINbrCounterAttack(true, countRoundAttack);

        ActiveManetteUI();
        /*if (!playerCanEsquive)
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
        }*/

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

        if(esquiveLeft)
        {
            if (!Battle.wallDetectLeft)
            {
                UIManager.ActiveUIEsquive(true, false, false);
            }
        }
        else
        {
            if (!Battle.wallDetectRight)
            {
                UIManager.ActiveUIEsquive(true, true, false);
            }
        }

        if (QTEDone)
            Time.timeScale = 0.25f;

        startQTE = true;
        playerAction = true;

        PlayBulletTimeSound();
        PlayQTETimerSound();

        Battle.canEsquive = true;
        Battle.canBlock = true;
    }

    void SetUpEndFenetreAttack()
    {
        Time.timeScale = 1f;
    }

    public void EndHit()
    {
        isAttacking = false;
        myAnimator.SetBool("Hit", false);
        ReturnToStatePatrol();
    }

    public void ApplyDamageToPlayer()
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