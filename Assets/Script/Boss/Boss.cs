using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


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

    [SerializeField] bool randomAttack, retrunState1, paradeReussi, counterReussi, attackReussiperfect, esquiveReussiPerfect;

    static public bool esquivePerfect;

    [SerializeField] bool playerAction, startQTE, startQTECounter, canApplyDamage, canApplyDamageBlock, canShakeCam;

    public bool startBattle;

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

    [Header("Change la taille du slider quand on appuie au bon moment")]
    [SerializeField] Vector3 maxSize;
    [SerializeField] Vector3 baseSize;

    [SerializeField] bool activeCombo1, activeCombo2, activeCombo3, combo1Done, combo2Done, combo3Done;

    void Start()
    {
        sliderBoss = GetComponent<SliderBoss>();
        sliderComboBoss = GetComponent<SliderComboBoss>();

        hpBoss = GetComponent<HPBoss>();
        myAnimator = GetComponent<Animator>();
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
    }

    void Update()
    {
        distPlayer = Vector3.Distance(transform.position, player.position);

        SmoothLookAt(player);

        if (retrunState1 && state != 0)
        {
            ReturnToStatePatrol();
        }

        if (distPlayer < 10)
        {
            startBattle = true;
            battle.isAttacked = true;
        }
        else if (!startBattle)
        {
            state = 0;
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
            case 5:
                StateWaitFinalCombo();
                break;
            case 6:
                StateHealth();
                break;
        }

        if (startBattle && !canAttack && state !=4 && state !=3)
            StateWaitingPlayer();

        if (startQTE)
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
            state = 5;

            if(!combo1Done)
                activeCombo1 = true;
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
        sliderBoss.setUpTimerSliderNormal -= Time.unscaledDeltaTime;

        ActiveManetteUI();
        if (countRoundAttack > 0)
        {
            TimingAttack();
        }
        if (playerCanEsquive)
        {
            TimingEsquive();
        }
        TimingBlock();

        if (sliderBoss.setUpTimerSliderNormal <= 0)
        {
            PlayerDoSomething();
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

        if (sliderComboBoss.sliderLooseCombo3Size <= 0)
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
            Debug.Log("Démarage Slider Attack" + (1 - sliderBoss.setUpStartPerfectFrameAttack));

            if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartPerfectFrameAttack && sliderBoss.sliderPerfectAttacSize > 0)
            {
                Battle.canAttack = true;

                sliderBoss.sliderPerfectAttacSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderAttackPerfect(sliderBoss.sliderPerfectAttacSize);

                if (Input.GetButtonDown("InteractButton"))
                {
                    UIManagerBoss.ActiveUIAttack(false, true);
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
            else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartLooseFrameAttack && sliderBoss.sliderLooseAttackSize > 0)
            {
                Battle.canAttack = false;

                sliderBoss.sliderLooseAttackSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderAttackLoose(sliderBoss.sliderLooseAttackSize);

                if (Input.GetButtonDown("InteractButton"))
                {
                    UIManagerBoss.ActiveUIAttack(false, true);
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
                    UIManagerBoss.ActiveUIAttack(false, true);
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

            UIManagerBoss.ActiveUIEsquive(false, false, false);
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

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
                        UIManagerBoss.ActiveUIEsquive(false, true, true);
                    else
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
            }
            else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) <= 1 - sliderBoss.setUpStartLooseFrameEsquive && sliderBoss.sliderLooseEsquiveSize > 0)
            {
                Battle.canEsquive = false;

                sliderBoss.sliderLooseEsquiveSize -= Time.unscaledDeltaTime / sliderBoss.baseSetUpTimerSliderNormal;
                UIManagerBoss.UpdateSliderEsquiveLoose(sliderBoss.sliderLooseEsquiveSize);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
                {
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
                        UIManagerBoss.ActiveUIEsquive(false, true, true);
                    else
                        UIManagerBoss.ActiveUIEsquive(false, false, true);

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
                    if (Input.GetAxisRaw("HorizontalLeftButtonX") > 0)
                        UIManagerBoss.ActiveUIEsquive(false, true, true);
                    else
                        UIManagerBoss.ActiveUIEsquive(false, false, true);

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

            UIManagerBoss.ActiveUIEsquive(false, false, false);
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

                if (Input.GetButtonDown("BlockButton"))
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

                if (Input.GetButtonDown("BlockButton"))
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

                if (Input.GetButtonDown("BlockButton"))
                {
                    UIManagerBoss.ActiveUIBlock(false, true);
                    canApplyDamage = false;
                    canApplyDamageBlock = true;
                    canShakeCam = true;
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
            canApplyDamageBlock = false;
            canApplyDamage = true;
            canShakeCam = true;
            PlayerDoSomething();
            ResetAllSlider();

            UIManagerBoss.ActiveUIEsquive(false, false, false);
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

        if (countRoundAttack > 0)
            countRoundAttack--;
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

            if (Input.GetButtonDown("InteractButton"))
            {
                UIManagerBoss.ActiveUICounter(false, true);

                canShakeCam = false;
                ResetCounterSlider();
                ReturnToStatePatrol();
                FailText.ActiveText();
                Time.timeScale = 1;
                PlayQTEValidationSound(0);
            }
        }
        else if (sliderBoss.setUpTimerSliderNormal * (1f / sliderBoss.baseSetUpTimerSliderNormal) >= 0 && startQTECounter)
        {
            Battle.canCounter = true;

            if (Input.GetButtonDown("InteractButton"))
            {
                UIManagerBoss.ActiveUICounter(false, true);

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
            canShakeCam = false;

            UIManagerBoss.ActiveUIEsquive(false, false, false);
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
            sliderComboBoss.sliderLooseCombo1Size -= Time.unscaledDeltaTime * 0.5f;
        }
        if (sliderComboBoss.sliderLooseCombo1Size <=0 && sliderComboBoss.sliderPerfectSize > 0) // frame Perfect
        {
            sliderComboBoss.sliderPerfectSize -= Time.unscaledDeltaTime * 0.5f;

            if (Input.GetButtonDown("InteractButton"))
            {
                combo1Done = true;
                activeCombo1 = false;
                UIManagerBoss.ActiveUICombo1(false, true);
                StartCoroutine(LaunchNextCombo(true, false));
                sliderComboBoss.sliderPerfectSize = 0.15f;
            }
        }
        if (sliderComboBoss.sliderPerfectSize <= 0 && sliderComboBoss.sliderLoose2Combo1Size > 0)// frame loose
        {
            sliderComboBoss.sliderLoose2Combo1Size -= Time.unscaledDeltaTime * 0.5f;

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
            sliderComboBoss.sliderLoose2Combo2Size -= Time.unscaledDeltaTime * 0.5f;
        }
        if (sliderComboBoss.sliderLoose2Combo2Size <= 0 && sliderComboBoss.sliderPerfectSize > 0) // frame Perfect
        {
            sliderComboBoss.sliderPerfectSize -= Time.unscaledDeltaTime * 0.5f;

            if (Input.GetButtonDown("InteractButton"))
            {
                combo2Done = true;
                activeCombo2 = false;
                UIManagerBoss.ActiveUICombo2(false, true);
                StartCoroutine(LaunchNextCombo(false, true));
                sliderComboBoss.sliderPerfectSize = 0.15f;
            }
        }
        if (sliderComboBoss.sliderPerfectSize <= 0 && sliderComboBoss.sliderLooseCombo2Size > 0)// frame loose
        {
            sliderComboBoss.sliderLooseCombo2Size -= Time.unscaledDeltaTime * 0.5f;

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
            sliderComboBoss.sliderLooseCombo3Size -= Time.unscaledDeltaTime * 0.5f;
        }
        if (sliderComboBoss.sliderLooseCombo3Size <= 0 && sliderComboBoss.sliderPerfectSize > 0) // frame Perfect
        {
            sliderComboBoss.sliderPerfectSize -= Time.unscaledDeltaTime * 0.5f;

            if (Input.GetButtonDown("InteractButton"))
            {
                combo3Done = true;
                activeCombo3 = false;
                UIManagerBoss.ActiveUICombo3(false, true);
            }
        }
        if (sliderComboBoss.sliderPerfectSize <= 0 && sliderComboBoss.sliderLoose2Combo3Size > 0)// frame loose
        {
            sliderComboBoss.sliderLoose2Combo3Size -= Time.unscaledDeltaTime * 0.5f;

        }
        if (sliderComboBoss.sliderLoose2Combo3Size <= 0)// loose combo
        {
            FailCombo();
        }
    }

    IEnumerator LaunchNextCombo(bool active2, bool active3)
    {
        yield return new WaitForSeconds(1f);
        activeCombo2 = active2;
        activeCombo3 = active3;
        yield break;
    }

    void StateWaitingPlayer()
    {
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

        /* if (distPlayer < 10f)
             transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - player.position.x, transform.position.y, transform.position.z - player.position.z), 1f * Time.deltaTime);
         else if(distPlayer > 12f)
             transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), 1 * Time.deltaTime);*/

        if (distPlayer < 10f)
        {
            Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-30, transform.right) * -transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask); // Si l'angle est plus petit que l'angle de vision du bot, on tire un rayon vers le joueur
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
    }

    void RandomAttack()
    {
        if (randomTimeBeforeAttack > 0)
        {
            randomTimeBeforeAttack -= Time.deltaTime;
        }
        else
        {
            retrunState1 = false;
            canAttack = true;
            randomTimeBeforeAttack = Random.Range(2, 5);
            state = 2;

            if (Random.Range(0, 2) == 0)
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
        if (startBattle)
        {
            state = 1;
            patrolIsActive = false;
        }
        else
        {
            if (!patrolIsActive)
            {
                StartCoroutine("Walk");
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
        if (Battle.isCounter)
        {
            state = 3;
        }
        else if (Battle.isAttacking)
        {
            state = 3;
        }
    }

    void StateWaitFinalCombo()
    {

    }

    void StateHealth()
    {
        if(hpBoss.hp < hpBoss.maxHp/2)
        {
            hpBoss.hp += Time.deltaTime * 5;
            Debug.LogError("Health");
        }
        else
        {
            state = 1;
        }
    }

    void SmoothLookAt(Transform target)
    {
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
          
            StartCoroutine("StartAttack");
        }

        if (distPlayer > 5f)
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

    void FailCombo()
    {
        UIManager.ActiveManetteUI(false);

        UIManagerBoss.ActiveUICombo1(false, false);
        UIManagerBoss.ActiveUICombo2(false, false);
        UIManagerBoss.ActiveUICombo3(false, false);

        HPBoss.startFinalCombo = false;
        activeCombo1 = false;
        activeCombo2 = false;
        activeCombo3 = false;

        combo1Done = false;
        combo2Done = false;
        combo3Done = false;

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
        state = 0;
    }

    void TimeScaleNormal()
    {
        Time.timeScale = 1f;
    }

    // Animation event
    void BeginAttack()
    {
        isAttacking = true;
        Invoke("SetUpStartActionPlayer", sliderBoss.setUpStartActionPlayer);
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

        if (!playerCanEsquive)
        {
            UIManagerBoss.ActiveUIEsquive(false, false, false);
            UIManagerBoss.ActiveUIEsquive(false, true, false);
            Battle.canEsquive = false;
        }
        else
        {
            UIManagerBoss.ActiveUIEsquive(true, true, false);
            UIManagerBoss.ActiveUIEsquive(true, false, false);
            Battle.canEsquive = true;
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

    void EndHit()
    {
        myAnimator.SetBool("Hit", false);
        ReturnToStatePatrol();
    }

    void ApplyDamageToPlayer()
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