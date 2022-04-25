using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> audioClipList = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        audioClipList[0].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Player Sound
    public static void PlaySoundStepFoot(AudioSource sourceAudio, AudioClip clipAudio)
    {
        sourceAudio.clip = clipAudio;
        sourceAudio.Play();
    }
    public static void PlaySoundPlayerBattle(AudioSource sourceAudio, AudioClip clipAudio)
    {
        sourceAudio.PlayOneShot(clipAudio);
    }
    public static void PlaySoundPlayerInteraction(AudioSource sourceAudio, AudioClip clipAudio)
    {
        sourceAudio.PlayOneShot(clipAudio);
    }

    //Ennemi Sound
    public static void PlaySoundEnnemiStepFoot(AudioSource sourceAudio, AudioClip clipAudio)
    {
        sourceAudio.clip = clipAudio;
        sourceAudio.Play();
    }
    public static void PlaySoundEnnemiBattle(AudioSource sourceAudio, AudioClip clipAudio)
    {
        sourceAudio.PlayOneShot(clipAudio);
    }

    //Interactive props Sound
    public static void PlaySoundInteractProps(AudioSource sourceAudio, AudioClip clipAudio)
    {
        sourceAudio.PlayOneShot(clipAudio);
    }
}