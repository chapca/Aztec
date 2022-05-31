using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderComboBoss : MonoBehaviour
{
    public float convertion;

    [Header("MANUELLE Defini la durée du qte en scd(min:0 / max:1)")]
    [Range(0f, 10f)]
    [SerializeField] public float setUpStartActionPlayer;
    [Range(0.0f, 10f)]
    [SerializeField] public float setUpEndActionPlayer;

    [Header("MANUELLE Defini la position des frames perfect(min:90° / max:359.99°)")]
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameCombo1;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameCombo2;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAnglePerfectFrameCombo3;

    [Header("MANUELLE Defini la position des frames Loose(min:90° / max:359.99°)")]
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameCombo1;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameCombo2;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLooseFrameCombo3;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLoose2FrameCombo1;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLoose2FrameCombo2;
    [Range(90f, 359.99f)]
    [SerializeField] float setUpAngleLoose2FrameCombo3;

    [Header("AUTO Valeur à laquel la frame perfect démare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartPerfectFrameCombo1;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartPerfectFrameCombo2;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartPerfectFrameCombo3;

    [Header("AUTO Valeur à laquel la frame loose démare par rapport au slider normal")]
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameCombo1;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameCombo2;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLooseFrameCombo3;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLoose2FrameCombo1;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLoose2FrameCombo2;
    [Range(0.0f, 1f)]
    [SerializeField] public float setUpStartLoose2FrameCombo3;


    [Header("AUTO durée du slider normal")]
    [Range(0.0f, 3f)]
    [SerializeField] public float baseSetUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpTimerSliderNormal;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderPerfect;

    [Header("AUTO durée du slider loose")]
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseCombo1;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseCombo2;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLooseCombo3;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLoose2Combo1;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLoose2Combo2;
    [Range(0.0f, 3f)]
    [SerializeField] public float setUpSliderLoose2Combo3;

    [Header("Reference slider")]
    [SerializeField] public Transform sliderCombo1Perfect, sliderUILoose2Combo1, sliderCombo1Loose;
    [SerializeField] public Transform sliderCombo2Perfect, sliderUILoose2Combo2, sliderCombo2Loose;
    [SerializeField] public Transform sliderCombo3Perfect, sliderUILoose2Combo3, sliderCombo3Loose;

    //(fillAmountSliderNormal *360f) +90f = angleSliderPerfect
    //(fillAmountSliderNormal *360f) = angleSliderPerfect - 90f
    //(fillAmountSliderNormal) = (angleSliderPerfect - 90f) /360f

    [Header("MANUAL Taille/durée slider")]
    [Range(0.0f, 1f)] [SerializeField] public float sliderPerfectSize;
    public float sliderPerfectCombo1Size;
    public float sliderPerfectCombo2Size;
    public float sliderPerfectCombo3Size;

    [Header("MANUAL Taille/durée slider Loose")]
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseCombo1Size;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseCombo2Size;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLooseCombo3Size;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLoose2Combo1Size;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLoose2Combo2Size;
    [Range(0.0f, 1f)] [SerializeField] public float sliderLoose2Combo3Size;

    [Range(0.0f, 1f)] public float baseSliderLooseCombo1Size;
    [Range(0.0f, 1f)] public float baseSliderLooseCombo2Size;
    [Range(0.0f, 1f)] public float baseSliderLooseCombo3Size;
    [Range(0.0f, 1f)] public float baseSliderLoose2Combo1Size;
    [Range(0.0f, 1f)] public float baseSliderLoose2Combo2Size;
    [Range(0.0f, 1f)] public float baseSliderLoose2Combo3Size;

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

        sliderPerfectCombo1Size = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectCombo3Size = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectCombo2Size = setUpSliderPerfect / baseSetUpTimerSliderNormal;


        setUpSliderLooseCombo1 = setUpTimerSliderNormal / (1f / sliderLooseCombo1Size);
        setUpSliderLooseCombo2 = setUpTimerSliderNormal / (1f / sliderLooseCombo2Size);
        setUpSliderLooseCombo3 = setUpTimerSliderNormal / (1f / sliderLooseCombo3Size);

        setUpSliderLoose2Combo1 = setUpTimerSliderNormal / (1f / sliderLoose2Combo1Size);
        setUpSliderLoose2Combo2 = setUpTimerSliderNormal / (1f / sliderLoose2Combo2Size);
        setUpSliderLoose2Combo3 = setUpTimerSliderNormal / (1f / sliderLoose2Combo3Size);


        sliderLooseCombo1Size = setUpSliderLooseCombo1 / baseSetUpTimerSliderNormal;
        sliderLooseCombo2Size = setUpSliderLooseCombo2 / baseSetUpTimerSliderNormal;
        sliderLooseCombo3Size = setUpSliderLooseCombo3 / baseSetUpTimerSliderNormal;

        sliderLoose2Combo1Size = setUpSliderLoose2Combo1 / baseSetUpTimerSliderNormal;
        sliderLoose2Combo2Size = setUpSliderLoose2Combo2 / baseSetUpTimerSliderNormal;
        sliderLoose2Combo3Size = setUpSliderLoose2Combo3 / baseSetUpTimerSliderNormal;


        baseSliderLooseCombo2Size = sliderLooseCombo2Size;
        baseSliderLooseCombo1Size = sliderLooseCombo1Size;
        baseSliderLooseCombo3Size = sliderLooseCombo3Size;

        baseSliderLoose2Combo1Size = sliderLoose2Combo1Size;
        baseSliderLoose2Combo2Size = sliderLoose2Combo2Size;
        baseSliderLoose2Combo3Size = sliderLoose2Combo3Size;
    }

    void SetPositionFramePerfect()
    {
        sliderCombo1Perfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameCombo1);
        sliderCombo2Perfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameCombo2);
        sliderCombo3Perfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameCombo3);

    }

    void SetPositionFrameLoose()
    {
        sliderCombo1Loose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCombo1);
        sliderCombo2Loose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCombo2);
        sliderCombo3Loose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCombo3);

        sliderUILoose2Combo1.localRotation = Quaternion.Euler(0, 0, setUpAngleLoose2FrameCombo1);
        sliderUILoose2Combo2.localRotation = Quaternion.Euler(0, 0, setUpAngleLoose2FrameCombo2);
        sliderUILoose2Combo3.localRotation = Quaternion.Euler(0, 0, setUpAngleLoose2FrameCombo3);
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
        sliderCombo1Perfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameCombo1);
        setUpStartPerfectFrameCombo1 = Mathf.Abs((sliderCombo1Perfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);

        sliderCombo2Perfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameCombo2);
        setUpStartPerfectFrameCombo2 = Mathf.Abs((sliderCombo2Perfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);

        sliderCombo3Perfect.localRotation = Quaternion.Euler(0, 0, setUpAnglePerfectFrameCombo3);
        setUpStartPerfectFrameCombo3 = Mathf.Abs((sliderCombo3Perfect.eulerAngles.z - (360f * sliderPerfectSize)) / 360f);
    }

    void UpdateSliderLoosePosition()
    {
        sliderCombo1Loose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCombo1);
        setUpStartLooseFrameCombo1 = Mathf.Abs((sliderCombo1Loose.eulerAngles.z - (360f * sliderLooseCombo1Size)) / 360f);

        sliderCombo2Loose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCombo2);
        setUpStartLooseFrameCombo2 = Mathf.Abs((sliderCombo2Loose.eulerAngles.z - (360f * sliderLooseCombo2Size)) / 360f);

        sliderCombo3Loose.localRotation = Quaternion.Euler(0, 0, setUpAngleLooseFrameCombo3);
        setUpStartLooseFrameCombo3 = Mathf.Abs((sliderCombo3Loose.eulerAngles.z - (360f * sliderLooseCombo3Size)) / 360f);


        sliderUILoose2Combo1.localRotation = Quaternion.Euler(0, 0, setUpAngleLoose2FrameCombo1);
        setUpStartLoose2FrameCombo1 = Mathf.Abs((sliderUILoose2Combo1.eulerAngles.z - (360f * sliderLoose2Combo1Size)) / 360f);

        sliderUILoose2Combo2.localRotation = Quaternion.Euler(0, 0, setUpAngleLoose2FrameCombo2);
        setUpStartLoose2FrameCombo2 = Mathf.Abs((sliderUILoose2Combo2.eulerAngles.z - (360f * sliderLoose2Combo2Size)) / 360f);

        sliderUILoose2Combo3.localRotation = Quaternion.Euler(0, 0, setUpAngleLoose2FrameCombo3);
        setUpStartLoose2FrameCombo3 = Mathf.Abs((sliderUILoose2Combo3.eulerAngles.z - (360f * sliderLoose2Combo3Size)) / 360f);
    }

    void SetFramePerfectSize()
    {
        baseSetUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        setUpTimerSliderNormal = ((setUpEndActionPlayer - setUpStartActionPlayer) * 10f) / convertion;
        setUpSliderPerfect = setUpTimerSliderNormal / (1f / sliderPerfectSize);

        sliderPerfectCombo1Size = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectCombo3Size = setUpSliderPerfect / baseSetUpTimerSliderNormal;
        sliderPerfectCombo2Size = setUpSliderPerfect / baseSetUpTimerSliderNormal;

        sliderCombo1Perfect.GetComponent<Image>().fillAmount = sliderPerfectCombo1Size;
        sliderCombo2Perfect.GetComponent<Image>().fillAmount = sliderPerfectCombo2Size;
        sliderCombo3Perfect.GetComponent<Image>().fillAmount = sliderPerfectCombo3Size;
    }

    void SetFrameLooseSize()
    {
        setUpSliderLooseCombo1 = setUpTimerSliderNormal / (1f / sliderLooseCombo1Size);

        setUpSliderLooseCombo2 = setUpTimerSliderNormal / (1f / sliderLooseCombo2Size);

        setUpSliderLooseCombo3 = setUpTimerSliderNormal / (1f / sliderLooseCombo3Size);

        setUpSliderLoose2Combo1 = setUpTimerSliderNormal / (1f / sliderLoose2Combo1Size);

        setUpSliderLoose2Combo2 = setUpTimerSliderNormal / (1f / sliderLoose2Combo2Size);

        setUpSliderLoose2Combo3 = setUpTimerSliderNormal / (1f / sliderLoose2Combo3Size);

        sliderLooseCombo1Size = setUpSliderLooseCombo1 / baseSetUpTimerSliderNormal;
        sliderLooseCombo2Size = setUpSliderLooseCombo2 / baseSetUpTimerSliderNormal;
        sliderLooseCombo3Size = setUpSliderLooseCombo3 / baseSetUpTimerSliderNormal;

        sliderLoose2Combo1Size = setUpSliderLoose2Combo1 / baseSetUpTimerSliderNormal;
        sliderLoose2Combo2Size = setUpSliderLoose2Combo2 / baseSetUpTimerSliderNormal;
        sliderLoose2Combo3Size = setUpSliderLoose2Combo3 / baseSetUpTimerSliderNormal;

        sliderCombo1Loose.GetComponent<Image>().fillAmount = sliderLooseCombo1Size;
        sliderCombo2Loose.GetComponent<Image>().fillAmount = sliderLooseCombo2Size;
        sliderCombo3Loose.GetComponent<Image>().fillAmount = sliderLooseCombo3Size;

        sliderUILoose2Combo1.GetComponent<Image>().fillAmount = sliderLoose2Combo1Size;
        sliderUILoose2Combo2.GetComponent<Image>().fillAmount = sliderLoose2Combo2Size;
        sliderUILoose2Combo3.GetComponent<Image>().fillAmount = sliderLoose2Combo3Size;
    }
}