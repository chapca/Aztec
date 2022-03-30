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

    void Start()
    {
        ennemiHp = GetComponent<EnnemiHp>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        battleScript = GameObject.FindWithTag("Player").GetComponent<Battle>();

        state = 0;
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
        TimingAttack();
        TimingEsquive();
        TimingBlock();
    }

    [SerializeField] float i = 1f;
    [SerializeField] float j = 0.25f;
    void TimingAttack()
    {
        if (i > 0)
        {
            i -= Time.unscaledDeltaTime;
            UIManager.UpdateSliderAttack(i);

            if (i <= 0.75f && j > 0)
            {
                j -= Time.unscaledDeltaTime;
                UIManager.UpdateSliderAttackPerfect(j);
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
        i = 1;
        j = 0.25f;
        UIManager.UpdateSliderAttackPerfect(j);
        UIManager.UpdateSliderAttack(i);
        startQTE = false;
    }


    [SerializeField] float k = 1;
    [SerializeField] float l = 0.25f;
    void TimingEsquive()
    {
        if (k > 0)
        {
            k -= Time.unscaledDeltaTime;
            UIManager.UpdateSliderEsquive(k);

            if (k <= 0.9f && l > 0)
            {
                l -= Time.unscaledDeltaTime;
                UIManager.UpdateSliderEsquivePerfect(l);
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
        k = 1;
        l = 0.25f;
        UIManager.UpdateSliderEsquive(k);
        UIManager.UpdateSliderEsquivePerfect(l);
        startQTE = false;
    }


    [SerializeField] float m = 1;
    [SerializeField] float n = 0.25f;
    void TimingBlock()
    {
        if (m > 0)
        {
            m -= Time.unscaledDeltaTime;
            UIManager.UpdateSliderBlock(m);

            if (m <= 0.35f && n > 0)
            {
                n -= Time.unscaledDeltaTime;
                UIManager.UpdateSliderBlockPerfect(n);
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
        m = 1;
        n = 0.25f;

        UIManager.UpdateSliderBlock(m);
        UIManager.UpdateSliderBlockPerfect(n);
        startQTE = false;
    }


    [SerializeField] float o = 1;
    [SerializeField] float p = 0.25f;
    void TimingCounter()
    {
        Debug.Log("Counter");
        if (o > 0 && startQTECounter)
        {
            o -= Time.unscaledDeltaTime;
            UIManager.UpdateSliderCounter(o);
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
        o = 1;
        p = 0.25f;
        UIManager.UpdateSliderCounter(o);
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

        if(distPlayer >2.5f)
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
        attackReussiperfect = false;
        StopCoroutine("StartAttack");
        yield break;
    }

    void StopLookAt()
    {
        lookat = false;
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

   /* IEnumerator delayAttak()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        Debug.Log("pd");

        if (playerAction)
        {
            i -= 0.015f;
            UIManager.UpdateSliderAttack(i);
        }
        else
        {
            ResetSliderAttackValue();
            yield break;
        }

        if (i <= 0.75f && j > 0)
        {
            Debug.Log("perfect");
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                Debug.Log("perfect");
                attackReussiperfect = true;
                state = 3;
                PlayerDoSomething();
                ResetSliderAttackValue();
                yield break;
            }
            j -= 0.015f;
            UIManager.UpdateSliderAttackPerfect(j);
        }
        if (i <= 0)
        {
            ResetSliderAttackValue();
            StopCoroutine("delayAttak");
            yield break;
        }
        else
        {
            StartCoroutine("delayAttak");
        }
    }
    void ResetSliderAttackValue()
    {
        i = 1f;
        j = 0.25f;
        UIManager.UpdateSliderAttackPerfect(j);
        UIManager.UpdateSliderAttack(i);
    }

    IEnumerator delayEsquive()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        Debug.Log("b");

        k -= 0.015f;
        UIManager.UpdateSliderEsquive(k);

        if (k <= 0.9f && l > 0)
        {
            if (Input.GetAxisRaw("Horizontal") !=0)
            {
                esquiveReussiPerfect = true;
                state = 4;
                StartCoroutine("DelayCounter");
                PlayerDoSomething();
                EndAttack();
                ResetSliderEsquiveValue();
                yield break;
            }
            l -= 0.015f;
            UIManager.UpdateSliderEsquivePerfect(l);
        }
        if (k <= 0)
        {
            ResetSliderEsquiveValue();
            StopCoroutine("delayEsquive");
            yield break;
        }
        else
        {
            StartCoroutine("delayEsquive");
        }
    }
    void ResetSliderEsquiveValue()
    {
        k = 1f;
        l = 0.25f;
        UIManager.UpdateSliderEsquivePerfect(l);
        UIManager.UpdateSliderEsquive(k);
    }

    IEnumerator DelayCounter()
    {
        Battle.canCounter = true;
        playerAction = true;
        UIManager.ActiveUICounter(true);
        yield return new WaitForSeconds(0.01f);

        m -= 0.015f;
        UIManager.UpdateSliderCounter(m);

        if (m <= 0.5f && n > 0)
        {
            n -= 0.015f;
            UIManager.UpdateSliderCounterPerfect(n);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine("DelayCounter");
                state = 3;
                counterReussi = true;
                PlayerDoSomething();
                EndAttack();
                ResetSliderCounter();
                yield break;
            }
        }
        if (m <= 0)
        {
            StartCoroutine("DelayBeforReturnPatrol");
            ResetSliderCounter();
            StopCoroutine("DelayCounter");
            yield break;
        }
        else
        {
            StartCoroutine("DelayCounter");
        }
    }
    void ResetSliderCounter()
    {
        UIManager.ActiveUICounter(false);
        Battle.canCounter = false;

        m = 1f;
        n = 0.25f;
        UIManager.UpdateSliderCounterPerfect(n);
        UIManager.UpdateSliderCounter(m);
    }

    IEnumerator DelayBeforReturnPatrol()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        ReturnToStatePatrol();
        yield break;
    }*/

    // Animation event
    void BeginAttack()
    {
        isAttacking = true;
    }
    void EndAttack()
    {
        if (!attackReussiperfect && !esquiveReussiPerfect)
            ReturnToStatePatrol();
        myAnimator.SetBool("Attack", false);
        isAttacking = false;
    }

    void FenetreStartActionPlayer()
    {
        Debug.Log("Choix action");

        Battle.canEsquive = true;
        Battle.canBlock = true;
        Battle.canAttack = true;
        UIManager.ActiveUIAttack(true);
        UIManager.ActiveUIBlock(true);
        UIManager.ActiveUIEsquive(true);

        //StartCoroutine("delayEsquive");
        //StartCoroutine("delayAttak");
        startQTE = true;
        Time.timeScale = 0.25f;
        playerAction = true;
    }
    void FenetreEndActionPlayer()
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