using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

//[ExecuteInEditMode]
public class Boss : MonoBehaviour
{
    HPBoss hpBoss;

    AudioSource myAudioSource;

    SliderBoss sliderBoss;
    SliderComboBoss sliderComboBoss;

    Battle battle;

    public AudioSource bulletTimeAudioSource, qteTimerAudioSource, qteValidationAudioSource, playerAudioSource;

    public Animator myAnimator;

    [SerializeField] float distPlayer;

    Transform player;

    Battle battleScript;

    [SerializeField] bool lookat, isAttacking, goBack, moveToPlayerBeforeAttack, launchAttack, canAttack;

    Quaternion lookPlayer;

    public int state;

    [SerializeField] float randomTimeBeforeAttack;

    [SerializeField] bool randomAttack, paradeReussi, counterReussi, attackReussiperfect, esquiveReussiPerfect;

    static public bool esquivePerfect;

    [SerializeField] bool playerAction, startQTE, startQTECounter, canApplyDamage, canApplyDamageBlock, canShakeCam;

    public bool startBattle;

    [Header("D�gat du mob")]
    [SerializeField] float damage, blockDamage;

    [Range(0.0f, 2f)]
    static public int countRoundAttack;

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

    [Header("Change la taille du slider quand on appuie au bon moment")]
    [SerializeField] Vector3 maxSize;
    [SerializeField] Vector3 baseSize;

    [SerializeField] bool activeCombo1, activeCombo2, activeCombo3, combo1Done, combo2Done, combo3Done;

    bool isDead, isHealthing, activeSliderBarreHP;

    static public bool esquiveRight, esquiveLeft;

    void Start()
    {
        sliderBoss = GetComponent<SliderBoss>();
        sliderComboBoss = GetComponent<SliderComboBoss>();

        hpBoss = GetComponent<HPBoss>();
       
        player = GameObject.FindWithTag("Player").transform;
        myAudioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        battleScript = GameObject.FindWithTag("Player").GetComponent<Battle>();

        state = 0;

        bulletTimeAudioSource = GameObject.Find("BattleBulletTimeMusic").GetComponent<AudioSource>();
        qteTimerAudioSource = GameObject.Find("QTETimerMusic").GetComponent<AudioSource>();
        qteValidationAudioSource = GameObject.Find("QTEValidationMusic").GetComponent<AudioSource>();
        playerAudioSource = GameObject.Find("EmptyPlayer").GetComponent<AudioSource>();

        battle = GameObject.FindWithTag("Player").GetComponent<Battle>();

        basePos = transform.position;

        randomTimeBeforeAttack = Random.Range(2, 5);

        UIManagerBoss.ActiveManetteUI(false);
    }

    void Update()
    {
        distPlayer = Vector3.Distance(transform.position, player.position);

        SmoothLookAt(player);

        /*if (retrunState1 && state != 0)
        {
            ReturnToStatePatrol();
        }*/

        if (distPlayer < 15 && !combo1Done && !combo2Done && !combo3Done)
        {
            startBattle = true;
            battle.isAttacked = true;
            AnimationEvent.bossFight = true;
        }
        else if (!startBattle && !combo1Done && !combo2Done && !combo3Done)
        {
            state = 0;
        }

        if(startBattle)
        {
           UIManagerBoss.AjusteSliderEsquive();

           if(!activeSliderBarreHP)
           {
            UIManagerBoss.SliderBoss(hpBoss.hp);
            activeSliderBarreHP = true;
           }
        }
        

        switch (state)
        {
            case 0:
                StatePatrol();
                break;
            case 1:
                if (startBattle && !canAttack && state != 4 && state != 3 && state != 2 && hpBoss.hp > 0 && !isHealthing)
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
            case 5:
                StateWaitFinalCombo();
                break;
            case 6:
                StateHealth();
                break;
            case 7:
                StateDeath();
                break;
        }

        /*if (startBattle && !canAttack && state !=4 && state !=3 && state != 2 && hpBoss.hp >0 && !isHealthing)
            StateWaitingPlayer();*/

        if (startQTE && hpBoss.hp >0)
        {
            DelayInput();
        }
        if (startQTECounter)
        {
            sliderBoss.setUpTimerSliderNormal -= Time.unscaledDeltaTime * 1.5f;
            TimingCounter();
        }

        if(HPBoss.startFinalCombo)
        {
            if(!combo1Done || !combo2Done || !combo3Done)
            {
                state = 5;

                if (!combo1Done)
                    activeCombo1 = true;
            }
        }

        if(activeCombo1)
        {
            UIManagerBoss.ActiveComboUI(true);
            UIManagerBoss.ActiveUICombo1(true, false);
            DelayInputCombo1();
        }
        if (activeCombo2)
        {
            UIManagerBoss.ActiveComboUI(true);
            UIManagerBoss.ActiveUICombo2(true, false);
            DelayInputCombo2();
        }
        if (activeCombo3)
        {
            UIManagerBoss.ActiveComboUI(true);
            UIManagerBoss.ActiveUICombo3(true, false);
            DelayInputCombo3();
        }

        if (combo1Done && combo2Done && combo3Done)
        {
            state = 7;
        }
    }

    void ActiveManetteUI()
    {
       /* if (!ManetteSpriteIsActive)
            UIManager.ActiveManetteUI(true);
        else
            UIManager.ActiveManetteUI(false);*/
    }

    void DelayInput()
    {
        if (sliderBoss.setUpTimerSliderNormal <= 0)
        {
            Debug.LogError("End timer action");

            canApplyDamage = true;
            canApplyDamageBlock = false;
            canShakeCam = true;
            countRoundAttack--;
            UIManagerBoss.ActiveUINbrCounterAttack(false, countRoundAttack);
            PlayerDoSomething();
        }
        else
        {
            sliderBoss.setUpTimerSliderNormal -= Time.unscaledDeltaTime;

            ActiveManetteUI();
            if (countRoundAttack > 0)
            {
                TimingAttack();
            }
            if (!Battle.wallDetectRight || !Battle.wallDetectLeft)
                TimingEsquive();

            TimingBlock();
        }
    }

    void DelayInputCombo1()
    {
        ActiveManetteUI();

        TimingCombo1();

        if (sliderComboBoss.sliderLoose2Combo1Size <= 0)
        {
            FailCombo();
        }
    }

    void DelayInputCombo2()
    {
        TimingCombo2();

        if (sliderComboBoss.sliderLooseCombo2Size <= 0)
        {
            FailCombo();
        }
    }

    void DelayInputCombo3()
    {
        TimingCombo3();

        if (sliderComboBoss.sliderLoose2Combo3Size <= 0)
        {
            FailCombo();
        }
    }

    // timing combat normal
    void TimingAttack()
    {
        if (sliderBoss.setUpTimerSliderNormal > 0)
        {
            UIManagerBoss.UpdateSliderAttack(sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal));
            Debug.Log("D�marage Slider Attack" + (1 - sliderBoss.setUpStartPerfectFrameAttack));

            if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartPerfectFrameAttack && sliderBoss.sliderPerfectAttacSize > 0)
            {
                Battle.canAttack = true;

                sliderBoss.sliderPerfectAttacSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderAttackPerfect(sliderBoss.sliderPerfectAttacSize);

                if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                {
                    UIManagerBoss.ActiveUIAttack(false, true);
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
            else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartLooseFrameAttack && sliderBoss.sliderLooseAttackSize > 0)
            {
                Battle.canAttack = false;

                sliderBoss.sliderLooseAttackSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderAttackLoose(sliderBoss.sliderLooseAttackSize);

                if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                {
                    UIManagerBoss.ActiveUIAttack(false, true);
                    canShakeCam = false;
                    PlayerDoSomething();
                    ResetAllSlider();
                    countRoundAttack--;
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
                    UIManagerBoss.ActiveUIAttack(false, true);
                    AnimationEvent.attackStandard = true;
                    Battle.myAnimator.SetBool("AttackNormal", true);
                    countRoundAttack--;
                    PlayerDoSomething();
                    ResetAllSlider();
                    PlayQTEValidationSound(1);
                }
            }
        }
        else
        {
            Debug.Log("Reset UI");
            PlayerDoSomething();
            ResetAllSlider();

            UIManagerBoss.ActiveUIEsquive(false, true, false);
            UIManagerBoss.ActiveUIEsquive(false, false, false);
            UIManagerBoss.ActiveUIAttack(false, false);
            UIManagerBoss.ActiveUIBlock(false, false);
            UIManagerBoss.ActiveUICounter(false, false);
        }
    }
    void ResetAttackSlider()
    {
        sliderBoss.setUpTimerSliderNormal = ((sliderBoss.setUpEndActionPlayer - sliderBoss.setUpStartActionPlayer) * 10f) / sliderBoss.convertion;

        sliderBoss.sliderPerfectAttacSize = sliderBoss.setUpSliderPerfect / sliderBoss.baseSetUpTimerSliderNormal;

        sliderBoss.sliderLooseAttackSize = sliderBoss.baseSliderLooseAttackSize;

        UIManagerBoss.UpdateSliderAttack(sliderBoss.setUpTimerSliderNormal);
        UIManagerBoss.UpdateSliderAttackPerfect(sliderBoss.sliderPerfectAttacSize);
        UIManagerBoss.UpdateSliderAttackLoose(sliderBoss.sliderLooseAttackSize);
        startQTE = false;
    }

    void TimingEsquive()
    {
        if (sliderBoss.setUpTimerSliderNormal > 0)
        {
            UIManagerBoss.UpdateSliderEsquive(sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal));

            if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartPerfectFrameEsquive && sliderBoss.sliderPerfectEsquiveSize > 0)
            {
                Battle.canEsquive = true;

                Debug.Log("Esquive");
                sliderBoss.sliderPerfectEsquiveSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderEsquivePerfect(sliderBoss.sliderPerfectEsquiveSize);

                if(esquiveRight)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManagerBoss.ActiveUIEsquive(false, true, true);

                        Debug.Log("Esquive Perfect");
                        esquivePerfect = true;
                        canApplyDamage = false;
                        canShakeCam = false;
                        esquiveReussiPerfect = true;
                        UIManagerBoss.ActiveUICounter(true, false);
                        Battle.canCounter = true;
                        startQTECounter = true;
                        state = 4;
                        PlayerDoSomething();
                        ResetAllSlider();
                        PerfectText.ActiveText();
                        PlayQTEValidationSound(2);
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                    {
                        UIManagerBoss.ActiveUIEsquive(false, true, false);

                        canApplyDamage = true;
                        canApplyDamageBlock = false;
                        canShakeCam = true;
                        PlayerDoSomething();
                        ResetAllSlider();
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
                        UIManagerBoss.ActiveUIEsquive(false, false, true);

                        Debug.Log("Esquive Perfect");
                        esquivePerfect = true;
                        canApplyDamage = false;
                        canShakeCam = false;
                        esquiveReussiPerfect = true;
                        UIManagerBoss.ActiveUICounter(true, false);
                        Battle.canCounter = true;
                        startQTECounter = true;
                        state = 4;
                        PlayerDoSomething();
                        ResetAllSlider();
                        PerfectText.ActiveText();
                        PlayQTEValidationSound(2);
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManagerBoss.ActiveUIEsquive(false, false, false);

                        canApplyDamage = true;
                        canApplyDamageBlock = false;
                        canShakeCam = true;
                        PlayerDoSomething();
                        ResetAllSlider();
                        countRoundAttack--;
                        FailText.ActiveText();
                        Time.timeScale = 1;
                        PlayQTEValidationSound(0);
                    }
                }
            }
            else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartLooseFrameEsquive && sliderBoss.sliderLooseEsquiveSize > 0)
            {
                Battle.canEsquive = false;

                sliderBoss.sliderLooseEsquiveSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderEsquiveLoose(sliderBoss.sliderLooseEsquiveSize);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0 || Input.GetButtonDown("HealthButton") || Input.GetButtonDown("CancelButton"))
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
                        UIManagerBoss.ActiveUIEsquive(false, true, true);
                    else
                        UIManagerBoss.ActiveUIEsquive(false, false, true);

                    canApplyDamage = true;
                    canApplyDamageBlock = false;
                    canShakeCam = true;

                    PlayerDoSomething();
                    ResetAllSlider();
                    countRoundAttack--;
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
                        UIManagerBoss.ActiveUIEsquive(false, true, true);

                        canApplyDamage = false;
                        canShakeCam = false;
                        countRoundAttack--;
                        PlayerDoSomething();
                        ResetAllSlider();
                        PlayQTEValidationSound(1);
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") < 0 || Input.GetButtonDown("HealthButton"))
                    {
                        UIManagerBoss.ActiveUIEsquive(false, true, false);
                        canApplyDamage = true;
                        canApplyDamageBlock = false;
                        canShakeCam = true;
                        PlayerDoSomething();
                        ResetAllSlider();
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
                        UIManagerBoss.ActiveUIEsquive(false, false, true);

                        canApplyDamage = false;
                        canShakeCam = false;
                        countRoundAttack--;
                        PlayerDoSomething();
                        ResetAllSlider();
                        PlayQTEValidationSound(1);
                    }
                    else if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0 || Input.GetButtonDown("CancelButton"))
                    {
                        UIManagerBoss.ActiveUIEsquive(false, false, false);
                        canApplyDamage = true;
                        canApplyDamageBlock = false;
                        canShakeCam = true;
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

            UIManagerBoss.ActiveUIEsquive(false, true, false);
            UIManagerBoss.ActiveUIEsquive(false, false, false);
            UIManagerBoss.ActiveUIAttack(false, false);
            UIManagerBoss.ActiveUIBlock(false, false);
            UIManagerBoss.ActiveUICounter(false, false);
        }
    }
    void ResetEsquiveSlider()
    {
        sliderBoss.setUpTimerSliderNormal = ((sliderBoss.setUpEndActionPlayer - sliderBoss.setUpStartActionPlayer) * 10f) / sliderBoss.convertion;

        sliderBoss.sliderPerfectEsquiveSize = sliderBoss.setUpSliderPerfect / sliderBoss.baseSetUpTimerSliderNormal;

        sliderBoss.sliderLooseEsquiveSize = sliderBoss.baseSliderLooseEsquiveSize;

        UIManagerBoss.UpdateSliderEsquive(sliderBoss.setUpTimerSliderNormal);
        UIManagerBoss.UpdateSliderEsquivePerfect(sliderBoss.sliderPerfectEsquiveSize);
        UIManagerBoss.UpdateSliderEsquiveLoose(sliderBoss.sliderLooseEsquiveSize);
        startQTE = false;
    }

    void TimingBlock()
    {
        if (sliderBoss.setUpTimerSliderNormal > 0)
        {
            UIManagerBoss.UpdateSliderBlock(sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal));

            if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartPerfectFrameBlock && sliderBoss.sliderPerfectBlockSize > 0)
            {
                Battle.canBlock = true;

                sliderBoss.sliderPerfectBlockSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderBlockPerfect(sliderBoss.sliderPerfectBlockSize);

                if (Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
                {
                    UIManagerBoss.ActiveUIBlock(false, true);
                    canApplyDamage = false;
                    canApplyDamageBlock = false;
                    canShakeCam = true;
                    Battle.myAnimator.SetBool("BlockPerfect", true);
                    Debug.LogError("Block");
                    PlayerDoSomething();
                    ResetAllSlider();
                    PerfectText.ActiveText();
                    countRoundAttack = 2;
                    PlayQTEValidationSound(2);
                }
            }
            else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartLooseFrameBlock && sliderBoss.sliderLooseBlockSize > 0)
            {
                Battle.canBlock = false;

                sliderBoss.sliderLooseBlockSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderBlockLoose(sliderBoss.sliderLooseBlockSize);

                if (Input.GetButtonDown("BlockButton") || Input.GetAxisRaw("VerticalLeftButtonY") < 0)
                {
                    UIManagerBoss.ActiveUIBlock(false, true);
                    PlayerDoSomething();
                    ResetAllSlider();
                    canApplyDamage = true;
                    canApplyDamageBlock = false;
                    canShakeCam = true;
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
                    UIManagerBoss.ActiveUIBlock(false, true);
                    canApplyDamage = false;
                    canApplyDamageBlock = true;
                    canShakeCam = true;
                    countRoundAttack--;
                    PlayerDoSomething();
                    ResetAllSlider();
                    PlayQTEValidationSound(1);
                }
            }
        }
        else
        {
            Battle.canBlock = false;
            canApplyDamageBlock = false;
            canApplyDamage = true;
            canShakeCam = true;
            PlayerDoSomething();
            ResetAllSlider();

            UIManagerBoss.ActiveUIEsquive(false, true, false);
            UIManagerBoss.ActiveUIEsquive(false, false, false);
            UIManagerBoss.ActiveUIAttack(false, false);
            UIManagerBoss.ActiveUIBlock(false, false);
            UIManagerBoss.ActiveUICounter(false, false);
        }
    }
    void ResetBlockSlider()
    {
        sliderBoss.setUpTimerSliderNormal = ((sliderBoss.setUpEndActionPlayer - sliderBoss.setUpStartActionPlayer) * 10f) / sliderBoss.convertion;

        sliderBoss.sliderPerfectBlockSize = sliderBoss.setUpSliderPerfect / sliderBoss.baseSetUpTimerSliderNormal;

        sliderBoss.sliderLooseBlockSize = sliderBoss.baseSliderLooseBlockSize;

        UIManagerBoss.UpdateSliderBlock(sliderBoss.setUpTimerSliderNormal);
        UIManagerBoss.UpdateSliderBlockPerfect(sliderBoss.sliderPerfectBlockSize);
        UIManagerBoss.UpdateSliderBlockLoose(sliderBoss.sliderLooseBlockSize);
        startQTE = false;
    }

    void TimingCounter()
    {
        Debug.Log("Counter");
        UIManagerBoss.UpdateSliderCounter(sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal));

        if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartLooseFrameCounter && sliderBoss.sliderLooseCounterSize > 0)
        {
            Battle.canCounter = false;

            sliderBoss.sliderLooseCounterSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal * 1.5f;
            UIManagerBoss.UpdateSliderCounterLoose(sliderBoss.sliderLooseCounterSize);

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                UIManagerBoss.ActiveUICounter(false, true);

                canShakeCam = false;
                esquiveReussiPerfect = false;
                ResetCounterSlider();
                ReturnToStatePatrol();
                countRoundAttack--;
                FailText.ActiveText();
                Time.timeScale = 1;
                PlayQTEValidationSound(0);
            }
        }
        else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) >= 0 && startQTECounter)
        {
            Battle.canCounter = true;

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                UIManagerBoss.ActiveUICounter(false, true);
                esquiveReussiPerfect = false;
                canShakeCam = false;
                counterReussi = true;
                countRoundAttack--;
                ResetCounterSlider();
                PlayQTEValidationSound(1);
            }
        }
        else
        {
            esquiveReussiPerfect = false;
            Battle.canCounter = false;
            PlayerDoSomething();
            ResetCounterSlider();
            ReturnToStatePatrol();
            canShakeCam = false;

            countRoundAttack--;

            UIManagerBoss.ActiveUIEsquive(false, true, false);
            UIManagerBoss.ActiveUIEsquive(false, false, false);
            UIManagerBoss.ActiveUIAttack(false, false);
            UIManagerBoss.ActiveUIBlock(false, false);
            UIManagerBoss.ActiveUICounter(false, false);
        }
    }
    void ResetCounterSlider()
    {
        StopPlayQTETimerSound();
        StopPlayBulletTimeSound();

        sliderBoss.setUpTimerSliderNormal = ((sliderBoss.setUpEndActionPlayer - sliderBoss.setUpStartActionPlayer) * 10f) / sliderBoss.convertion;
        sliderBoss.setUpSliderPerfect = sliderBoss.setUpTimerSliderNormal / 4f;

        sliderBoss.sliderLooseCounterSize = sliderBoss.baseSliderLooseCounterSize;

        UIManagerBoss.UpdateSliderCounter(sliderBoss.setUpTimerSliderNormal);
        UIManagerBoss.UpdateSliderCounterLoose(sliderBoss.sliderLooseCounterSize);
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

    // timing combo
    void TimingCombo1()
    {
        if(sliderComboBoss.sliderLooseCombo1Size>0) // frame loose
        {
            sliderComboBoss.sliderLooseCombo1Size -= Time.unscaledDeltaTime * 0.75f;
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                FailCombo();
        }
        if (sliderComboBoss.sliderLooseCombo1Size <=0 && sliderComboBoss.sliderPerfectSize > 0) // frame Perfect
        {
            sliderComboBoss.sliderPerfectSize -= Time.unscaledDeltaTime * 0.75f;

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                combo1Done = true;
                activeCombo1 = false;
                UIManagerBoss.ActiveUICombo1(false, true);
                StartCoroutine(LaunchNextCombo(true, false));
                sliderComboBoss.sliderPerfectSize = 0.15f;
                Battle.myAnimator.SetBool("AttackNormal", true);
            }
        }
        if (sliderComboBoss.sliderPerfectSize <= 0 && sliderComboBoss.sliderLoose2Combo1Size > 0)// frame loose
        {
            sliderComboBoss.sliderLoose2Combo1Size -= Time.unscaledDeltaTime * 0.75f;
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                FailCombo();
        }
        if (sliderComboBoss.sliderLoose2Combo1Size <= 0)// loose combo
        {
            FailCombo();
        }
    }
    void TimingCombo2()
    {
        if (sliderComboBoss.sliderLoose2Combo2Size > 0) // frame loose
        {
            sliderComboBoss.sliderLoose2Combo2Size -= Time.unscaledDeltaTime * 0.75f;
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                FailCombo();
        }
        if (sliderComboBoss.sliderLoose2Combo2Size <= 0 && sliderComboBoss.sliderPerfectSize > 0) // frame Perfect
        {
            sliderComboBoss.sliderPerfectSize -= Time.unscaledDeltaTime * 0.75f;

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                Time.timeScale = 0.3f;
                combo2Done = true;
                activeCombo2 = false;
                UIManagerBoss.ActiveUICombo2(false, true);
                StartCoroutine(LaunchNextCombo(false, true));
                sliderComboBoss.sliderPerfectSize = 0.15f;
                Battle.myAnimator.SetBool("AttackPerfect", true); 
                Battle.myAnimator.SetBool("AttackNormal", false);
            }
        }
        if (sliderComboBoss.sliderPerfectSize <= 0 && sliderComboBoss.sliderLooseCombo2Size > 0)// frame loose
        {
            sliderComboBoss.sliderLooseCombo2Size -= Time.unscaledDeltaTime * 0.75f;
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                FailCombo();
        }
        if (sliderComboBoss.sliderLooseCombo2Size <= 0)// loose combo
        {
            FailCombo();
        }
    }
    void TimingCombo3()
    {
        if (sliderComboBoss.sliderLooseCombo3Size > 0) // frame loose
        {
            Time.timeScale = 1;
            sliderComboBoss.sliderLooseCombo3Size -= Time.unscaledDeltaTime * 0.75f;
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                FailCombo();
        }
        if (sliderComboBoss.sliderLooseCombo3Size <= 0 && sliderComboBoss.sliderPerfectSize > 0) // frame Perfect
        {
            sliderComboBoss.sliderPerfectSize -= Time.unscaledDeltaTime * 0.75f;

            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
            {
                Time.timeScale = 0.3f;
                startBattle = false;
                combo3Done = true;
                activeCombo3 = false;
                UIManagerBoss.ActiveUICombo3(false, true); 
                Battle.myAnimator.SetBool("Counter", true);
                Battle.myAnimator.SetBool("AttackPerfect", false);
                state = 7;
            }
        }
        if (sliderComboBoss.sliderPerfectSize <= 0 && sliderComboBoss.sliderLoose2Combo3Size > 0)// frame loose
        {
            sliderComboBoss.sliderLoose2Combo3Size -= Time.unscaledDeltaTime * 0.75f;
            if (Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("VerticalLeftButtonY") > 0)
                FailCombo();
        }
        if (sliderComboBoss.sliderLoose2Combo3Size <= 0)// loose combo
        {
            FailCombo();
        }
    }

    IEnumerator LaunchNextCombo(bool active2, bool active3)
    {
        yield return new WaitForSeconds(0.5f);
        activeCombo2 = active2;
        activeCombo3 = active3;
        yield break;
    }

    void StateWaitingPlayer()
    {
        Debug.LogError("State 1");

        agent.enabled = true;
        agent.angularSpeed = 0;

        paradeReussi = false;
        counterReussi = false;
        attackReussiperfect = false;

        RandomAttack();

        if (!randomAttack && PlayerHp.hp > 0)
        {
            RandomAttack();
        }

        if (PlayerHp.hp <= 0)
        {
            startBattle = false;
            ReturnToStatePatrol();
        }

        SmoothLookAt(player);

        if (distPlayer < 18f)
        {
            Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-30, transform.right) * -transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask); // Si l'angle est plus petit que l'angle de vision du bot, on tire un rayon vers le joueur
            Debug.DrawRay(this.transform.position, Quaternion.AngleAxis(-30, transform.right) * -transform.TransformDirection(Vector3.forward) * 100f, Color.blue);

            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);
                if (transform.position != hit.point)
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
        else if (distPlayer > 20)
        {
            agent.SetDestination(player.position);
            myAnimator.SetBool("WalkBack", false);
            myAnimator.SetBool("Walk", true);
        }
    }

    void RandomAttack()
    {
        if (randomTimeBeforeAttack > 0)
        {
            randomTimeBeforeAttack -= Time.deltaTime;
        }
        else
        {
            //retrunState1 = false;
            canAttack = true;
            randomTimeBeforeAttack = Random.Range(4, 7);
            state = 2;
        }
    }

    void StatePatrol()
    {
        Debug.LogError("State 0");

        if (startBattle)
        {
            state = 1;
            patrolIsActive = false;
        }
        else
        {
            if (!patrolIsActive)
            {
                //StartCoroutine("Walk");
                patrolIsActive = true;
                agent.angularSpeed = 120f;
            }
        }
    }

    IEnumerator Walk()
    {
        while (gameObject.activeSelf && state == 0)
        {
            Debug.Log("ennemi patrolling");

            Vector2 pos2D = Random.insideUnitCircle * circleRadius;
            Vector3 pos3D = basePos + new Vector3(pos2D.x, 0, pos2D.y);

            if (agent.isActiveAndEnabled)
                agent.SetDestination(pos3D);

            if(agent.isActiveAndEnabled)
            {
                if (!agent.isStopped)
                {
                    Debug.Log("!is stopped");
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    Debug.Log("is stopped");
                }
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
       /* myAnimator.SetBool("Hit", true);*/
        myAnimator.SetBool("Attack", false);
    }

    void StateWaitCounter()
    {
        if (Battle.isCounter)
        {
            state = 3;
        }
    }

    void StateWaitFinalCombo()
    {
        canAttack = false;
    }

    void StateHealth()
    {
        if(hpBoss.hp < hpBoss.maxHp/2)
        {
            myAnimator.SetBool("Hit", false);

            myAnimator.SetBool("Health", true);

            isHealthing = true;
            hpBoss.hp += Time.deltaTime * 70;
            UIManagerBoss.SliderBoss(hpBoss.hp);
            Debug.LogError("Health");
        }
    }

    public void StartHealth()
    {
        SoundManager.PlaySoundBoss(myAudioSource, SoundManager.soundAndVolumeBossHealthStatic[0]);
        /*animHeakthLaunched = true;
        myAnimator.SetBool("Health", false);*/
    }

    public void EndHealth()
    {
        isHealthing = false;
        myAnimator.SetBool("Health", false);
        hpBoss.hp = hpBoss.maxHp / 2;
        state = 1;
    }

    void StateDeath()
    {
        Debug.LogError("Death");

        if(!isDead)
            StartCoroutine("DelayBeforeDeath");
    }

    IEnumerator DelayBeforeDeath()
    {
        isDead = true;
        UIManagerBoss.ActiveManetteUI(false);
        yield return new WaitForSeconds(0.65f);
        hpBoss.BossDeath();
        Time.timeScale = 1f;
        yield break;
    }

    void SmoothLookAt(Transform target)
    {
        Vector3 relativePos = player.position - transform.position;

        relativePos.x = player.position.x - transform.position.x;
        relativePos.y = 0;
        relativePos.z = player.position.z - transform.position.z;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 0.5f);
        transform.rotation = rotation;
/*
        Vector3 relativePos = player.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 0.5f);
        transform.rotation = rotation;*/
    }

    void LaunchAttack()
    {
        if (distPlayer <= 9f && !isAttacking)
        {
            battleScript.isAttacked = true;
          
            StartCoroutine("StartAttack");
        }

        if (distPlayer > 5f)
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
        yield break;
    }

    void FailCombo()
    {
        UIManagerBoss.ActiveManetteUI(false);

        UIManagerBoss.ActiveUICombo1(false, false);
        UIManagerBoss.ActiveUICombo2(false, false);
        UIManagerBoss.ActiveUICombo3(false, false);

        HPBoss.startFinalCombo = false;

        combo1Done = false;
        combo2Done = false;
        combo3Done = false;

        activeCombo1 = false;
        activeCombo2 = false;
        activeCombo3 = false;

        hpBoss.hp = 1;

        sliderComboBoss.sliderPerfectSize = 0.15f;

        //combo 1
        sliderComboBoss.sliderLooseCombo1Size = sliderComboBoss.baseSliderLooseCombo1Size;
        sliderComboBoss.sliderLoose2Combo1Size = sliderComboBoss.baseSliderLoose2Combo1Size;
        sliderComboBoss.sliderPerfectCombo1Size = sliderComboBoss.setUpSliderPerfect / sliderComboBoss.baseSetUpTimerSliderNormal;


        //combo 2
        sliderComboBoss.sliderLooseCombo2Size = sliderComboBoss.baseSliderLooseCombo2Size;
        sliderComboBoss.sliderLoose2Combo2Size = sliderComboBoss.baseSliderLoose2Combo2Size;
        sliderComboBoss.sliderPerfectCombo2Size = sliderComboBoss.setUpSliderPerfect / sliderComboBoss.baseSetUpTimerSliderNormal;


        //combo 3
        sliderComboBoss.sliderLooseCombo3Size = sliderComboBoss.baseSliderLooseCombo3Size;
        sliderComboBoss.sliderLoose2Combo3Size = sliderComboBoss.baseSliderLoose2Combo3Size;
        sliderComboBoss.sliderPerfectCombo3Size = sliderComboBoss.setUpSliderPerfect / sliderComboBoss.baseSetUpTimerSliderNormal;

        state = 6;


    }

    // fonction en lien avec les action du joueur
    void ReturnToStatePatrol()
    {
        Debug.LogError("ReturnToStatePatrol");
        state = 0;
    }

    void TimeScaleNormal()
    {
        Time.timeScale = 1f;
    }

    // Animation event
    public void StartHit()
    {
        myAnimator.SetBool("Attack", false);
        isAttacking = false;
    }

    void BeginAttack()
    {
        isAttacking = true;
        Invoke("SetUpStartActionPlayer", sliderBoss.setUpStartActionPlayer);
    }

    public void EndAttack()
    {
        if (!attackReussiperfect && !esquiveReussiPerfect && !HPBoss.finalCombo)
        {
            ReturnToStatePatrol();
        }

        myAnimator.SetBool("Attack", false);
        isAttacking = false;
    }

    void SetUpStartActionPlayer()
    {
        Debug.Log("Choix action");

        UIManagerBoss.ActiveUINbrCounterAttack(true, countRoundAttack);

        if (!Battle.wallDetectLeft && !Battle.wallDetectRight)
        {
            if (Random.Range(0, 2) == 0)
            {
                UIManagerBoss.ActiveUIEsquive(true, true, false);
                esquiveRight = true;
                esquiveLeft = false;
            }
            else
            {
                UIManagerBoss.ActiveUIEsquive(true, false, false);
                esquiveLeft = true;
                esquiveRight = false;
            }
        }

        if (countRoundAttack > 0)
        {
            UIManagerBoss.ActiveUIAttack(true, false);
            Battle.canAttack = true;
        }
        else
        {
            UIManagerBoss.ActiveUIAttack(false, false);
            Battle.canAttack = false;
        }

        UIManagerBoss.ActiveManetteUI(true);
        UIManagerBoss.ActiveUIBlock(true, false);

        Battle.canBlock = true;

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

    public void EndHit()
    {
        esquiveReussiPerfect = false;
        myAnimator.SetBool("Hit", false);
        isAttacking = false;

        if(!HPBoss.finalCombo)
            ReturnToStatePatrol();
    }

    public void ApplyDamageToPlayer()
    {
        if (canApplyDamage && canShakeCam && !canApplyDamageBlock)
        {
            Debug.LogError("No Block");
            Battle.myAnimator.SetBool("IsHit", true);

            PlayerHp.TakeDamage(damage);
            Debug.LogError(SoundManager.soundAndVolumePlayerBattleStatic[0]);
            SoundManager.PlaySoundPlayerBattle(playerAudioSource, SoundManager.soundAndVolumePlayerBattleStatic[0]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersFailStatic, ShakeCam.shakeCamParametersFailStatic[0].axeShake, ShakeCam.shakeCamParametersFailStatic[0].amplitude,
                       ShakeCam.shakeCamParametersFailStatic[0].frequence, ShakeCam.shakeCamParametersFailStatic[0].duration);

            AnimationEvent.Hit();
        }

        if (!canApplyDamage && canApplyDamageBlock && canShakeCam)
        {
            Debug.LogError("launch Block Normal");
            Battle.myAnimator.SetBool("BlockNormal", true);

            PlayerHp.TakeDamage(blockDamage);
            SoundManager.PlaySoundPlayerBattle(playerAudioSource, SoundManager.soundAndVolumePlayerBattleStatic[4]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamParametersBlockNormalStatic, ShakeCam.shakeCamParametersBlockNormalStatic[0].axeShake, ShakeCam.shakeCamParametersBlockNormalStatic[0].amplitude,
                        ShakeCam.shakeCamParametersBlockNormalStatic[0].frequence, ShakeCam.shakeCamParametersBlockNormalStatic[0].duration);
        }

        if (!canApplyDamageBlock && !canApplyDamage && canShakeCam)
        {
            Debug.LogError("launch Block Perfect");
            Battle.myAnimator.SetBool("BlockPerfectEnd", true);

            SoundManager.PlaySoundPlayerBattle(playerAudioSource, SoundManager.soundAndVolumePlayerBattleStatic[5]);

            ShakeCam.ShakerCam(ShakeCam.shakeCamparametersBlockPerfectStatic, ShakeCam.shakeCamparametersBlockPerfectStatic[0].axeShake, ShakeCam.shakeCamparametersBlockPerfectStatic[0].amplitude,
                        ShakeCam.shakeCamparametersBlockPerfectStatic[0].frequence, ShakeCam.shakeCamparametersBlockPerfectStatic[0].duration);
        }

        //esquiveReussiPerfect = false;
        attackReussiperfect = false;
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