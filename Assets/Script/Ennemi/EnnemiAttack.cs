using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Range(0.0f, 1f)]
    [SerializeField] float setUpStartActionPlayer, setUpEndActionPlayer;

    [Range(0.0f, 3f)]
    [SerializeField] float baseSetUpTimerSliderNormal, baseSetUpSliderPerfect, setUpTimerSliderNormal, setUpSliderPerfect;
    void Start()
    {
        ennemiHp = GetComponent<EnnemiHp>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        battleScript = GameObject.FindWithTag("Player").GetComponent<Battle>();

        state = 0;

        convertion = 10f / 6f;

        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / 4f;

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) *10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / 4f;

        sliderPerfectAttack = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectBlock = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectEsquive = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        Debug.Log((setUpEndActionPlayer - setUpStartActionPlayer) * 10f);
    }

    void Update()
    {
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
            TimingCounter();
        }


    }

    void DelayInput()
    {
        setUpTimerSliderNormal -= Time.unscaledDeltaTime;

        TimingAttack();
        TimingEsquive();
        TimingBlock();
    }

    [SerializeField] float sliderPerfectAttack;
    void TimingAttack()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderAttack(setUpTimerSliderNormal * (1f/ baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 0.25f && sliderPerfectAttack > 0)
            {
                sliderPerfectAttack -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackPerfect(sliderPerfectAttack);

                Debug.Log(sliderPerfectAttack);
                if (Input.GetAxis("VerticalLeftButtonY") > 0)
                {
                    AnimationEvent.attackPerfect = true;
                    canApplyDamage = false;
                    attackReussiperfect = true;
                    state = 4;
                    PlayerDoSomething();
                    ResetAllSlider();
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


    [SerializeField] float sliderPerfectEsquive;
    void TimingEsquive()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderEsquive(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 0.25f && sliderPerfectEsquive > 0)
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


    [SerializeField] float sliderPerfectBlock;
    void TimingBlock()
    {
        if (setUpTimerSliderNormal > 0)
        {
            UIManager.UpdateSliderBlock(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal));

            if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 0.25f && sliderPerfectBlock > 0)
            {
                sliderPerfectBlock -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderBlockPerfect(sliderPerfectBlock);

                if (Input.GetAxis("VerticalLeftButtonY") < 0)
                {
                    canApplyDamage = false;
                    PlayerDoSomething();
                    ResetAllSlider();
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

        UIManager.UpdateSliderCounter(setUpTimerSliderNormal);
        UIManager.ActiveUICounter(false);
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
        UIManager.ActiveUIAttack(false);
        UIManager.ActiveUIBlock(false);
        UIManager.ActiveUIEsquive(false);
        Battle.canEsquive = false;
        Battle.canBlock = false;
        Battle.canAttack = false;
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