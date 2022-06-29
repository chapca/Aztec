using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{
    Boss boss;
    AudioSource myAudioSource;

    void Start()
    {
        boss = transform.parent.transform.parent.GetComponent<Boss>();
        myAudioSource = transform.parent.transform.parent.GetComponent<AudioSource>();
    }

    void StartAttack()
    {
        SoundManager.PlaySoundEnnemiBattle(myAudioSource, SoundManager.soundAndVolumeEnnemiBattleStatic[Random.Range(0, 2)]);
        //ennemiAttack.SetUpStartActionPlayer();
    }
    void EndAttack()
    {
        boss.EndAttack();
    }
    void ApplyDamageToPlayer()
    {
        boss.ApplyDamageToPlayer();
    }

    void EndHit()
    {
        boss.EndHit();
    }
    void StartHit()
    {
        boss.StartHit();
    }

    void StartHealth()
    {
        boss.StartHealth();
    }
    void EndHealth()
    {
        boss.EndHealth();
    }
}