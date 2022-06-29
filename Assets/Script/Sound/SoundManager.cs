using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("List de son player battle")] public List<SoundVolume> soundAndVolumePlayerBattle = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumePlayerBattleStatic = new List<SoundVolume>();

    [Header("List de son player exploration")] public List<SoundVolume> soundAndVolumePlayerExploration = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumePlayerExplorationStatic = new List<SoundVolume>();

    [Header("List de son player interaction")] public List<SoundVolume> soundAndVolumeListInteraction = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumeListInteractionStatic = new List<SoundVolume>();

    [Header("List de son 2D")] public List<SoundVolume> soundAndVolume2D = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolume2DStatic = new List<SoundVolume>();

    [Header("List de son ennemi Idle")] public List<SoundVolume> soundAndVolumeEnnemiIdle = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumeEnnemiIdleStatic = new List<SoundVolume>();

    [Header("List de son ennemi Battle")] public List<SoundVolume> soundAndVolumeEnnemiBattle = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumeEnnemiBattleStatic = new List<SoundVolume>();

    [Header("List de son boss health")] public List<SoundVolume> soundAndVolumeBossHealth = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumeBossHealthStatic = new List<SoundVolume>();

    private void Awake()
    {
        soundAndVolumePlayerBattleStatic = soundAndVolumePlayerBattle;

        soundAndVolumePlayerExplorationStatic = soundAndVolumePlayerExploration;

        soundAndVolumeListInteractionStatic = soundAndVolumeListInteraction;

        soundAndVolume2DStatic = soundAndVolume2D;

        soundAndVolumeEnnemiIdleStatic = soundAndVolumeEnnemiIdle;

        soundAndVolumeEnnemiBattleStatic = soundAndVolumeEnnemiBattle;

        soundAndVolumeBossHealthStatic = soundAndVolumeBossHealth;
    }
    void Start()
    {

    }

    void Update()
    {
        
    }

    //Player Sound
    public static void PlaySoundStepFoot(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        /*sourceAudio.volume = soundVolume.volume;
        sourceAudio.clip = soundVolume.clip;
        sourceAudio.Play();*/

        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }
    public static void PlaySoundPlayerBattle(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }
    public static void PlaySoundPlayerInteraction(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }

    //Ennemi Sound
    public static void PlaySoundEnnemiBattle(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }
    public static void PlaySoundEnnemiIdle(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);

        Debug.Log("Sound GEcko idle");
    }

    //Interactive props Sound
    public static void PlaySoundInteractProps(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }

    // sound boss 
    public static void PlaySoundBoss(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }

    //2D sound continue
    public static void PlaySound2DContinue(AudioSource sourceAudio, SoundVolume soundVolume, bool active)
    {
        if(active)
        {
            sourceAudio.volume = soundVolume.volume;
            sourceAudio.clip = soundVolume.clip;
            sourceAudio.loop = active;
            sourceAudio.Play();
        }
        else
        {
            sourceAudio.Stop();
        }
    }

    //2D sound one shot
    public static void PlaySound2DOneShot(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }
}

[System.Serializable]
public class SoundVolume
{
    public string nameSound;
    public AudioClip clip;
    [Range(0.0f, 1f)] public float volume;
}