using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    static public float hp = 50;

    [SerializeField] Battle battle;

    AudioSource playerGainHpAudioSource;

    private void Awake()
    {
        hp = 100;
    }

    private void Start()
    {
        playerGainHpAudioSource = GameObject.Find("PlayerGainHPSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("HealthButton") && hp < 100 && PlayerBlood.bloodQuantity >0 && !battle.isAttacked)
        {
            ManualHealth();
            SoundManager.PlaySound2DContinue(playerGainHpAudioSource, SoundManager.soundAndVolumePlayerExplorationStatic[0], true);
        }
        else
        {
            SoundManager.PlaySound2DContinue(playerGainHpAudioSource, SoundManager.soundAndVolumePlayerExplorationStatic[0], false);
        }

        if(hp <=0)
        {
            DeathPlayer();
        }
    }

    public static void TakeDamage(float damage)
    {
        hp -= damage;
        PlayerUI.UpdateSliderHp();
    }

    public static void ManualHealth()
    {
        hp += Time.deltaTime * 5f;
        PlayerUI.UpdateSliderHp();
        PlayerBlood.LooseBlood(Time.deltaTime * 10f);
    }

    void DeathPlayer()
    {
        Battle.myAnimator.SetBool("Death", true);
    }
}