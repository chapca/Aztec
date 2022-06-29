using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiAnimEvent : MonoBehaviour
{
    EnnemiAttack ennemiAttack;
    AudioSource myAudioSource;

    void Start()
    {
        ennemiAttack = transform.parent.transform.parent.GetComponent<EnnemiAttack>();
        myAudioSource = transform.parent.transform.parent.GetComponent<AudioSource>();
    }

    void StartAttack()
    {
        SoundManager.PlaySoundEnnemiBattle(myAudioSource, SoundManager.soundAndVolumeEnnemiBattleStatic[Random.Range(0, 2)]);
        //ennemiAttack.SetUpStartActionPlayer();
    }
    void EndAttack()
    {
        ennemiAttack.EndAttack();
    }
    void ApplyDamageToPlayer()
    {
        ennemiAttack.ApplyDamageToPlayer();
    }

    void EndHit()
    {
        ennemiAttack.EndHit();
    }void StartHit()
    {
        ennemiAttack.StartHit();
    }
}