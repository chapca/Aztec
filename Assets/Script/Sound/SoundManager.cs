using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
/*    public static List<AudioClip> audioClipsListPlayerBattleStatic = new List<AudioClip>();
    [Header("List de son player battle")] public List<AudioClip> audioClipsListPlayerBattle = new List<AudioClip>();

    public static List<AudioClip> audioClipsListPlayerExplorationStatic = new List<AudioClip>();
    [Header("List de son player exploration")] public List<AudioClip> audioClipsPlayerExplorationList = new List<AudioClip>();

    public static List<AudioClip> audioClipsListInteractionStatic = new List<AudioClip>();
    [Header("List de son player interaction")] public List<AudioClip> audioClipsListInteraction = new List<AudioClip>();

    public static List<AudioClip> audioClipsList2DStatic = new List<AudioClip>();
    [Header("List de son 2D")] public List<AudioClip> audioClipsList2D = new List<AudioClip>();

*/
    ///////////////////////////


    [Header("List de son player battle")] public List<SoundVolume> soundAndVolumePlayerBattle = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumePlayerBattleStatic = new List<SoundVolume>();

    [Header("List de son player exploration")] public List<SoundVolume> soundAndVolumePlayerExploration = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumePlayerExplorationStatic = new List<SoundVolume>();

    [Header("List de son player interaction")] public List<SoundVolume> soundAndVolumeListInteraction = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolumeListInteractionStatic = new List<SoundVolume>();

    [Header("List de son 2D")] public List<SoundVolume> soundAndVolume2D = new List<SoundVolume>();
    public static List<SoundVolume> soundAndVolume2DStatic = new List<SoundVolume>();


    private void Awake()
    {
        /*audioClipsListPlayerBattleStatic = audioClipsListPlayerBattle;

        audioClipsListPlayerExplorationStatic = audioClipsPlayerExplorationList;

        audioClipsListInteractionStatic = audioClipsListInteraction;

        audioClipsList2DStatic = audioClipsList2D;*/

        soundAndVolumePlayerBattleStatic = soundAndVolumePlayerBattle;

        soundAndVolumePlayerExplorationStatic = soundAndVolumePlayerExploration;

        soundAndVolumeListInteractionStatic = soundAndVolumeListInteraction;

        soundAndVolume2DStatic = soundAndVolume2D;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Player Sound
    public static void PlaySoundStepFoot(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.clip = soundVolume.clip;
        sourceAudio.Play();
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
    public static void PlaySoundEnnemiStepFoot(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.clip = soundVolume.clip;
        sourceAudio.Play();
    }
    public static void PlaySoundEnnemiBattle(AudioSource sourceAudio, SoundVolume soundVolume)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }

    //Interactive props Sound
    public static void PlaySoundInteractProps(AudioSource sourceAudio, SoundVolume soundVolume)
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
    public static void PlaySound2DOneShot(AudioSource sourceAudio, SoundVolume soundVolume, bool active)
    {
        sourceAudio.volume = soundVolume.volume;
        sourceAudio.PlayOneShot(soundVolume.clip);
    }
}

[System.Serializable]
public class SoundVolume
{
    public AudioClip clip;
    [Range(0.0f, 1f)] public float volume;
}