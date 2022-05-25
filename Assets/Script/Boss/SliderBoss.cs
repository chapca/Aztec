using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderBoss : MonoBehaviour
{
    public float convertion;

    [Header("MANUELLE Defini la durée du qte en scd(min:0 / max:1)")]
    [Range(0f, 10f)]
    [SerializeField] public float setUpStartActionPlayer;
    [Range(0.0f, 10f)]
    [SerializeField] public float setUpEndActionPlayer;

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
    [SerializeField] public float setUpStartPerfectFrameAttack;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartPerfectFrameEsquive;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartPerfectFrameBlock;

    [Header("AUTO Valeur à laquel la frame loose démare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameAttack;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameCounter;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameEsquive;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameBlock;

    [Header("AUTO durée au slider normal")]
    [Range(0.0f, 3f)]
    [SerializeField] public float baseSetUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderPerfect;

    [Header("AUTO durée au slider loose")]
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseAttack;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseCounter;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseEsquive;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseBlock;

    [Header("Reference slider")]
    [SerializeField] public Transform sliderAttackPerfect, sliderAttackNormal, sliderAttackLoose, sliderCounterLoose;
    [SerializeField] public Transform sliderEsquivePerfect, sliderEsquiveNormal, sliderEsquiveLoose;
    [SerializeField] public Transform sliderBlockPerfect, sliderBlockNormal, sliderBlockLoose;

    //(fillAmountSliderNormal *360f) +90f = angleSliderPerfect
    //(fillAmountSliderNormal *360f) = angleSliderPerfect - 90f
    //(fillAmountSliderNormal) = (angleSliderPerfect - 90f) /360f

    [Header("MANUAL Taille/durée slider")]
    [Range(0.0f, 1f)] [SerializeField] public float sliderPerfectSize;
    public float sliderPerfectAttacSize;
    public float sliderPerfectEsquiveSize;
    public float sliderPerfectBlockSize;

    [Header("MANUAL Taille/durée slider Loose")]
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseCounterSize;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseAttackSize;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseEsquiveSize;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseBlockSize;

    [Range(0.0f, 1f)] public float baseSliderLooseCounterSize;
    [Range(0.0f, 1f)] public float baseSliderLooseAttackSize;
    [Range(0.0f, 1f)] public float baseSliderLooseEsquiveSize;
    [Range(0.0f, 1f)] public float baseSliderLooseBlockSize;

    [Header("Execute code en hors Game (DESACTIVER AVANT DE LANCER)")]
    [SerializeField] public bool activeThisinEditor, ManetteSpriteIsActive;

    // Start is called before the first frame update
    void Start()
    {
        if (!activeThisinEditor)
        {
            setUpStartActionPlayer /= 10;
            setUpEndActionPlayer /= 10;
        }

        SetPositionFramePerfect();
        SetPositionFrameLoose();

        convertion = 10f / 6f;

        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
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

    // Update is called once per frame
    void Update()
    {
        if (activeThisinEditor)
        {
            UpdateSliderPosition();
            UpdateSliderLoosePosition();

            SetFramePerfectSize();
            SetFrameLooseSize();
        }
    }

    void UpdateSliderPosition()
    {
        sliderAttackPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameAttack);
        setUpStartPerfectFrameAttack = Mathf.Abs((sliderAttackPerfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);

        sliderEsquivePerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameEsquive);
        setUpStartPerfectFrameEsquive = Mathf.Abs((sliderEsquivePerfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);

        sliderBlockPerfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameBlock);
        setUpStartPerfectFrameBlock = Mathf.Abs((sliderBlockPerfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);
    }

    void UpdateSliderLoosePosition()
    {
        sliderAttackLoose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameAttack);
        setUpStartLooseFrameAttack = Mathf.Abs((sliderAttackLoose.eulerAngles.z - (360f * sliderLooseAttackSize)) / 360f);

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
}