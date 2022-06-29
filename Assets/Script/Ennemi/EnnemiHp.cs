using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiHp : MonoBehaviour
{
    EnnemiManager ennemiManager;
    public int maxHp, hp, nbrBlood;
    EnnemiAttack ennemiAttack;
    [SerializeField] Animator myAnimator;
    Transform parentObject;

    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    [SerializeField] Material materialLowLife;

    [SerializeField] GameObject bloodDecal;

    Interaction interaction;

    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        ennemiAttack = GetComponent<EnnemiAttack>();

        hp = maxHp;

        parentObject = transform.parent;

        ennemiManager = GetComponentInParent<EnnemiManager>();
        ennemiManager.bloodQuantity += nbrBlood;

        skinnedMesh = transform.GetChild(0).transform.GetChild(1).transform.GetChild(3).transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();

        interaction = GameObject.FindWithTag("Player").GetComponentInParent<Interaction>();

        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if(hp<100)
        {
            SoundManager.PlaySoundEnnemiBattle(myAudioSource, SoundManager.soundAndVolumeEnnemiBattleStatic[Random.Range(2, 4)]);
            skinnedMesh.material = materialLowLife;
        }

        if(hp <=0)
        {
            SoundManager.PlaySoundEnnemiBattle(myAudioSource, SoundManager.soundAndVolumeEnnemiBattleStatic[4]);
            bloodDecal.SetActive(true);
            EnnemiAttack.QTEDone = true;
            EnnemiAttack.tutoDone = true;
            EnnemiAttack.coroutineLaunch = true;
            transform.parent = null;
            ennemiAttack.enabled = false;
            //PlayerBlood.GetBlood(nbrBlood);
            myAnimator.SetBool("Death", true);
            PlayerController.ennemi = null;
            //StartCoroutine("RespawnEnnemi");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!ennemiManager.bloodRecover)
            {
                ennemiManager.isInBloodTrigger = true;

                if (ennemiManager.canRecoverBlood)
                {
                    UIManager.ActiveManetteInputInteract(true);
                }
            }
            else
            {
                UIManager.ActiveManetteInputInteract(false);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ennemiManager.isInBloodTrigger = false;
            UIManager.ActiveManetteInputInteract(false);
        }
    }

    /*IEnumerator RespawnEnnemi()
    {
        yield return new WaitForSeconds(5f);

        transform.parent = parentObject;
        hp = maxHp;
        ennemiAttack.state = 0;
        ennemiAttack.startBattle = false;
        ennemiAttack.enabled = true;
        myAnimator.SetBool("Die", false);

        yield break;
    }*/
}