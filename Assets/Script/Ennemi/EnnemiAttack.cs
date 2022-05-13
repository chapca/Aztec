using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class EnnemiAttack : MonoBehaviour
{
    EnnemiHp ennemiHp;

    EnnemiManager ennemiManager;

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

        if(retrunState1 && state !=1)
        {
            ReturnToStatePatrol();
        }

        if(distPlayer < 10 || ennemiManager.startBattle)
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

        if (!thisSelected)
            ReturnToStatePatrol();

        if (playerAction)
        {
            if (Input.GetButtonDown("BlockButton") || Input.GetButtonDown("InteractButton") || Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
            {
                //PlayerDoSomething();
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
            setUpTimerSliderNormal -= Time.unscaledDeltaTime*1.5f;
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
                    PlayerDoSomething();
                    ResetAllSlider();
                    AnimationEvent.attackPerfect = true;
                    canApplyDamage = false;
                    attackReussiperfect = true;
                    state = 4;
                    PerfectText.ActiveText();
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameAttack && sliderLooseAttackSize > 0)
            {
                Battle.canAttack = false;

                sliderLooseAttackSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderAttackLoose(sliderLooseAttackSize);

                if (Input.GetButtonDown("InteractButton"))
                {
                    PlayerDoSomething();
                    ResetAllSlider();
                    FailText.ActiveText();
                    Time.timeScale = 1;
                }
            }
            else
            {
                Battle.canAttack = true;
                if (Input.GetButtonDown("InteractButton"))
                {
                    AnimationEvent.attackStandard = true;
                    PlayerDoSomething();
                    ResetAllSlider();
                }
            }
        }
        else
        {
            Debug.Log("REset UI");
            PlayerDoSomething();
            ResetAllSlider();
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
                    Debug.Log("Esquive Perfect");
                    canApplyDamage = false;
                    esquiveReussiPerfect = true;
                    UIManager.ActiveUICounter(true);
                    Battle.canCounter = true;
                    startQTECounter = true;
                    state = 4;
                    PlayerDoSomething();
                    ResetAllSlider();
                    PerfectText.ActiveText();
                }
            }
            else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameEsquive && sliderLooseEsquiveSize > 0)
            {
                Battle.canEsquive = false;

                sliderLooseEsquiveSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderEsquiveLoose(sliderLooseEsquiveSize);

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
                {
                    PlayerDoSomething();
                    ResetAllSlider();
                    FailText.ActiveText();
                    Time.timeScale = 1;
                }
            }
            else
            {
                Battle.canEsquive = true;

                if (Input.GetAxisRaw("HorizontalLeftButtonX") != 0)
                {
                    canApplyDamage = false;
                    PlayerDoSomething();
                    ResetAllSlider();
                }
            }
        }
        else
        {
            PlayerDoSomething();
            ResetAllSlider();
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
                    Debug.Log("Block");
                    PlayerDoSomething();
                    ResetAllSlider();
                    canApplyDamage = false;
                    PerfectText.ActiveText();
                    countRoundAttack = 2;
                }
            }
            else if(setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) <= 1 - setUpStartLooseFrameBlock && sliderLooseBlockSize > 0)
            {
                Battle.canBlock = false;

                sliderLooseBlockSize -= Time.unscaledDeltaTime / baseSetUpTimerSliderNormal;
                UIManager.UpdateSliderBlockLoose(sliderLooseBlockSize);

                if (Input.GetButtonDown("BlockButton"))
                {
                    PlayerDoSomething();
                    ResetAllSlider();
                    FailText.ActiveText();
                    Time.timeScale = 1;
                }
            }
            else
            {
                Battle.canBlock = true;

                if (Input.GetButtonDown("BlockButton"))
                {
                    canApplyDamage = false;
                    canApplyDamageBlock = true;
                    PlayerDoSomething();
                    ResetAllSlider();
                    countRoundAttack = 1;
                }
            }
        }
        else
        {
            Battle.canBlock = false;
            PlayerDoSomething();
            ResetAllSlider();
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
                ResetCounterSlider();
                ReturnToStatePatrol();
                FailText.ActiveText();
                Time.timeScale = 1;
            }
        }
        else if (setUpTimerSliderNormal * (1f / baseSetUpTimerSliderNormal) >= 0 && startQTECounter)
        {
            Battle.canCounter = true;

            if (Input.GetButtonDown("InteractButton"))
            {
                counterReussi = true;
                ResetCounterSlider();
            }
        }
        else
        {
            Battle.canCounter = false;
            PlayerDoSomething();
            ResetCounterSlider();
            ReturnToStatePatrol();
            thisSelected = false;
        }
    }
    void ResetCounterSlider()
    {
        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / 4f;

        sliderLooseCounterSize = baseSliderLooseCounterSize;

        UIManager.UpdateSliderCounter(setUpTimerSliderNormal);
        UIManager.UpdateSliderCounterLoose(sliderLooseCounterSize);
        UIManager.ActiveUICounter(false);
        UIManager.ActiveManetteUI(false);
        startQTECounter = false;
    }

    void ResetAllSlider()
    {
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
        if (distPlayer <= 5f && !isAttacking)
        {
            battleScript.isAttacked = true;
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
            UIManager.ActiveManetteUI(false);

        UIManager.ActiveUIAttack(false);
        UIManager.ActiveUIBlock(false);
        UIManager.ActiveUIEsquive(false);

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
        state = 1;
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
            UIManager.ActiveUIEsquive(false);
            Battle.canEsquive = false;
        }
        else
        {
            UIManager.ActiveUIEsquive(true);
            Battle.canEsquive = true;
        }

        if (countRoundAttack>0)
        {
            UIManager.ActiveUIAttack(true);
            Battle.canAttack = true;
        }
        else
        {
            UIManager.ActiveUIAttack(false);
            Battle.canAttack = false;
        }

        UIManager.ActiveManetteUI(true);
        UIManager.ActiveUIBlock(true);

        Battle.canBlock = true;

        Time.timeScale = 0.25f;
        startQTE = true;
        playerAction = true;
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
        if(canApplyDamage)
            PlayerHp.TakeDamage(damage);

        if(canApplyDamageBlock)
        {
            PlayerHp.TakeDamage(blockDamage);
        }
    }
}