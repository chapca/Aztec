using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiAttack : MonoBehaviour
{
    EnnemiHp ennemiHp;

    public Animator myAnimator;

    [SerializeField] float distPlayer;

    Transform player;

    Battle battleScript;

    [SerializeField] bool lookat, isAttacking, goBack, moveToPlayerBeforeAttack, launchAttack, canAttack;

    Quaternion lookPlayer;

    [SerializeField] int state;

    [SerializeField] bool moveRight;

    float time = 2f;

    [SerializeField] float randomTimeBeforeAttack;

    [SerializeField] bool randomAttack, retrunState1, paradeReussi, counterReussi, attackReussiperfect, esquiveReussiPerfect;

    [SerializeField] bool playerAction, startQTE, startQTECounter, canApplyDamage, canApplyDamageBlock;

    public bool thisSelected, resetEnnemiSelected;

    public static bool startBattle;

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

    [Header("AUTO Valeur à laquel la frame perfect démare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartPerfectFrameAttack;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartPerfectFrameEsquive;
    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartPerfectFrameBlock;

    [Header("AUTO durée au slider normal")]
    [Range(0.0f, 3f)]
    [SerializeField] float baseSetUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] float setUpSliderPerfect;

    [Header("Reference slider")]
    [SerializeField] Transform sliderAttackPerfect, sliderAttackNormal;
    [SerializeField] Transform sliderEsquivePerfect, sliderEsquiveNormal;
    [SerializeField] Transform sliderBlockPerfect, sliderBlockNormal;

    //(fillAmountSliderNormal *360f) +90f = angleSliderPerfect
    //(fillAmountSliderNormal *360f) = angleSliderPerfect - 90f
    //(fillAmountSliderNormal) = (angleSliderPerfect - 90f) /360f

    [Header("MANUAL Taille/durée slider")]
    [Range(0.0f, 1f)]
    [SerializeField] float sliderPerfectSize;
    float sliderPerfectAttack;
    float sliderPerfectEsquive;
    float sliderPerfectBlock;

    [Header("Execute code en hors Game (DESACTIVER AVANT DE LANCER)")]
    [SerializeField] bool activeThisinEditor, ManetteSpriteIsActive;

    void Start()
    {
        ennemiHp = GetComponent<EnnemiHp>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        battleScript = GameObject.FindWithTag("Player").GetComponent<Battle>();

        state = 0;

        if(!activeThisinEditor)
        {
            setUpStartActionPlayer /= 10;
            setUpEndActionPlayer /= 10;
        }

        SetPositionFramePerfect();

        convertion = 10f / 6f;

        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal /(1f/sliderPerfectSize);

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) *10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        sliderPerfectAttack = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectBlock = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectEsquive = setUpSliderPerfect / baseSetUpTimerSliderNormal;
    }

    void SetPositionFramePerfect()
    {
        sliderAttackPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameAttack);
        sliderEsquivePerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameEsquive);
        sliderBlockPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameBlock);
    }

    void Update()
    {
        if(activeThisinEditor)
        {
            UpdateSliderPosition();
            SetFramePerfectSize();
        }

        distPlayer = Vector3.Distance(transform.position, player.position);

        if(retrunState1 && state !=1)
        {
            ReturnToStatePatrol();
        }

        if(distPlayer <10)
        {
            startBattle = true;
            Debug.Log("Start battle" + startBattle);
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

        if (!thisSelected)
            ReturnToStatePatrol();

        if (playerAction)
        {
            if (Input.GetAxisRaw("VerticalLeftButtonY") !=0 || Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
            {
                PlayerDoSomething();
                TimeScaleNormal();
                playerAction = false;
            }
        }

        if(startQTE)
        {
            DelayInput();
        }
        if (startQTECounter)
        {
            setUpTimerSliderNormal -= Time.unscaledDeltaTime;
            TimingCounter();
        }
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

    void SetFramePerfectSize()
    {
        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        sliderPerfectAttack = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectBlock = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectEsquive = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        sliderAttackPerfect.GetComponent<Image>().fillAmount = sliderPerfectAttack;
        sliderEsquivePerfect.GetComponent<Image>().fillAmount = sliderPerfectEsquive;
        sliderBlockPerfect.GetComponent<Image>().fillAmount = sliderPerfectBlock;
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
        TimingAttack();
        TimingEsquive();
        TimingBlock();
    }

    void TimingAttack()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderAttack(setUpTimerSliderNormal * (1f/ baseSetUpTimerSliderNormal));
            Debug.Log("Démarage Slider Attack" + (1 - setUpStartPerfectFrameAttack));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1-setUpStartPerfectFrameAttack && sliderPerfectAttack > 0)
            {
                sliderPerfectAttack -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackPerfect(sliderPerfectAttack);

                if (Input.GetAxis("VerticalLeftButtonY") > 0)
                {
                    PlayerDoSomething();
                    ResetAllSlider();
                    AnimationEvent.attackPerfect = true;
                    canApplyDamage = false;
                    attackReussiperfect = true;
                    state = 4;
                }
            }
            else
            {
                if (Input.GetAxis("VerticalLeftButtonY") > 0)
                {
                    AnimationEvent.attackStandard = true;
                    PlayerDoSomething();
                    ResetAllSlider();
                }
            }
        }
        else
        {
            ResetAllSlider();
        }
    }
    void ResetAttackSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;

        sliderPerfectAttack = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        UIManager.UpdateSliderAttack(setUpTimerSliderNormal);
        UIManager.UpdateSliderAttackPerfect(sliderPerfectAttack);
        startQTE = false;
    }

    void TimingEsquive()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderEsquive(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1-setUpStartPerfectFrameEsquive && sliderPerfectEsquive > 0)
            {
                sliderPerfectEsquive -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquivePerfect(sliderPerfectEsquive);

                if (Input.GetAxis("HorizontalLeftButtonX") != 0)
                {
                    canApplyDamage = false;
                    esquiveReussiPerfect = true;
                    UIManager.ActiveUICounter(true);
                    Battle.canCounter = true;
                    startQTECounter = true;
                    state = 4;
                    PlayerDoSomething();
                    ResetAllSlider();
                }
            }
            else
            {
                if (Input.GetAxis("HorizontalLeftButtonX") != 0)
                {
                    canApplyDamage = false;
                    PlayerDoSomething();
                    ResetAllSlider();
                }
            }
        }
        else
        {
            ResetAllSlider();
        }
    }
    void ResetEsquiveSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;

        sliderPerfectEsquive = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        UIManager.UpdateSliderEsquive(setUpTimerSliderNormal);
        UIManager.UpdateSliderEsquivePerfect(sliderPerfectEsquive);
        startQTE = false;
    }

    void TimingBlock()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderBlock(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartPerfectFrameBlock && sliderPerfectBlock > 0)
            {
                sliderPerfectBlock -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderBlockPerfect(sliderPerfectBlock);

                if (Input.GetAxis("VerticalLeftButtonY") < 0)
                {
                    PlayerDoSomething();
                    ResetAllSlider();
                    canApplyDamage = false;
                }
            }
            else
            {
                if (Input.GetAxis("VerticalLeftButtonY") < 0)
                {
                    canApplyDamage = false;
                    canApplyDamageBlock = true;
                    PlayerDoSomething();
                    ResetAllSlider();
                }
            }
        }
        else
        {
            ResetAllSlider();
        }
    }
    void ResetBlockSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;

        sliderPerfectBlock = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        UIManager.UpdateSliderBlock(setUpTimerSliderNormal);
        UIManager.UpdateSliderBlockPerfect(sliderPerfectBlock);
        startQTE = false;
    }

    void TimingCounter()
    {
        Debug.Log("Counter");
        if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) > 0 && startQTECounter)
        {
            UIManager.UpdateSliderCounter(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));
            if (Input.GetAxis("VerticalLeftButtonY") > 0)
            {
                counterReussi = true;
                ResetCounterSlider();
            }
        }
        else
        {
            ResetCounterSlider();
            ReturnToStatePatrol();
            thisSelected = false;
        }
    }
    void ResetCounterSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / 4f;

        Battle.canCounter = false;
        UIManager.UpdateSliderCounter(setUpTimerSliderNormal);
        UIManager.ActiveUICounter(false);
        UIManager.ActiveManetteUI(false);
        startQTECounter = false;
    }

    void ResetAllSlider()
    {
        Battle.canEsquive = false;
        Battle.canBlock = false;
        Battle.canAttack = false;
        playerAction = false;
        ResetBlockSlider();
        ResetEsquiveSlider();
        ResetAttackSlider();

        SetUpEndFenetreAttack();
    }

    void StateWaitingPlayer()
    {
        canApplyDamageBlock = false;
        canApplyDamage = true;
        paradeReussi = false;
        counterReussi = false;
        attackReussiperfect = false;
        esquiveReussiPerfect = false;

        RandomAttack();

        if(!randomAttack && thisSelected)
        {
            RandomAttack();
        }

        SmoothLookAt(player);

        if (distPlayer < 6f)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - player.position.x, transform.position.y, transform.position.z - player.position.z), 2f * Time.deltaTime);
        else if(distPlayer > 8f)
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), 1 * Time.deltaTime);

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

        transform.RotateAround(player.position, Vector3.up, 20 * Time.deltaTime);
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
        }
    }

    void StatePatrol()
    {
        if(startBattle)
        {
            state = 1;
        }
    }

    void StateAttack()
    {
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
    {
        lookPlayer = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookPlayer, 10 * Time.deltaTime);
    }

    void LaunchAttack()
    {
        if (distPlayer <= 2.5f && !isAttacking)
        {
            battleScript.isAttacked = true;
            StartCoroutine("StartAttack");
        }

        if(distPlayer > 2.5f)
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
        if(!esquiveReussiPerfect)
            UIManager.ActiveManetteUI(false);

        Battle.canEsquive = false;
        Battle.canBlock = false;
        Battle.canAttack = false;
        UIManager.ActiveUIAttack(false);
        UIManager.ActiveUIBlock(false);
        UIManager.ActiveUIEsquive(false);
        yield return new WaitForSecondsRealtime(0.1f);
        yield break;
    }

    // fonction en lien avec les action du joueur
    void ReturnToStatePatrol()
    {
        state = 1;
        thisSelected = false;
    }

    void TimeScaleNormal()
    {
        Debug.Log("Time scale 1");
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

        Battle.canEsquive = true;
        Battle.canBlock = true;
        Battle.canAttack = true;

        UIManager.ActiveManetteUI(true);
        UIManager.ActiveUIAttack(true);
        UIManager.ActiveUIBlock(true);
        UIManager.ActiveUIEsquive(true);

        Time.timeScale = 0.25f;
        startQTE = true;
        playerAction = true;
    }

    void SetUpEndFenetreAttack()
    {
        Time.timeScale = 1f;
        playerAction = false;

        Battle.canEsquive = false;
        Battle.canBlock = false;
        Battle.canAttack = false;

        if(!esquiveReussiPerfect)
            UIManager.ActiveManetteUI(false);

        UIManager.ActiveUIAttack(false);
        UIManager.ActiveUIBlock(false);
        UIManager.ActiveUIEsquive(false);
    }

    void EndHit()
    {
        myAnimator.SetBool("Hit", false);
        ReturnToStatePatrol();
    }

    void ApplyDamageToPlayer()
    {
        if(canApplyDamage)
            PlayerHp.TakeDamage(20);

        if(canApplyDamageBlock)
        {
            PlayerHp.TakeDamage(10);
        }
    }
}