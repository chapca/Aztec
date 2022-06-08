using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiManager : MonoBehaviour
{
    [SerializeField] int indexSelected;

    [SerializeField] bool launchSelectAttaker, checkEnnemiAttaker;

    [SerializeField] int indexEnnemi;

    [SerializeField] bool attackLaunched;

    [SerializeField] float distPlayer;
    [SerializeField] Transform player;

    [SerializeField] bool inFight, endFight;

    Battle battle;

    [SerializeField] int nombreEnnemi;

    [SerializeField]public GameObject prefabEnnemiManager;

    [SerializeField] float x, y, z;

    public bool startBattle;

    public float bloodQuantity;

    public bool bloodRecover, isInBloodTrigger, canRecoverBlood;

    void Start()
    {
        battle = GameObject.FindWithTag("Player").GetComponent<Battle>();
        isInBloodTrigger = false;
    }

    void Update()
    {
        distPlayer = Vector3.Distance(transform.position, player.position);

        if(transform.childCount == 0 && !endFight)
        {
            StartCoroutine("EndFight");
            EnnemiAttack.countRoundAttack = 0;
            endFight = true;
            startBattle = false;
            StartCoroutine("RespawnEnnemi");
        }

        if (startBattle)
        {
            battle.isAttacked = true;

            if(PlayerController.ennemi ==null)
            {
                PlayerController.ennemi = transform.GetChild(Random.Range(0, transform.childCount));
            }

            if (!checkEnnemiAttaker)
            {
                StartCoroutine("CheckEnnemiAttacker");
            }
            if (!attackLaunched && !launchSelectAttaker)
            {
                if (transform.childCount > 1)
                    StartCoroutine("SelectAttacker");
                else
                    StartCoroutine("SelectSoloAttacker");
            }
        }

        if(!bloodRecover && endFight && isInBloodTrigger)
        {
            StartCoroutine("RecoverBlood");
        }
    }

    IEnumerator RecoverBlood()
    {
        yield return new WaitForSeconds(0.75f);

        canRecoverBlood = true;

        if (Input.GetButtonDown("InteractButton"))
        {
            PlayerBlood.GetBlood(bloodQuantity);
            bloodRecover = true;
            yield break;
        }
    }

    IEnumerator EndFight()
    {
        yield return new WaitForSeconds(1f);
        battle.isAttacked = false;
        yield break;
    }

    IEnumerator CheckEnnemiAttacker()
    {
        checkEnnemiAttaker = true;
        int nbrSelected = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<EnnemiAttack>().thisSelected == true)
            {
                nbrSelected++;
                attackLaunched = true;
                indexSelected = indexEnnemi;
            }

            if(i == transform.childCount-1 && nbrSelected ==0)
            {
                attackLaunched = false;
                launchSelectAttaker = false;
            }
        }
        yield return new WaitForSeconds(0.1f);
        checkEnnemiAttaker = false;
    }

    IEnumerator SelectAttacker()
    {
        Debug.Log("SelectAttacker");
        if (transform.childCount == 0)
        {
            StopCoroutine("SelectAttacker");
            yield break;
        }

        launchSelectAttaker = true;
        yield return new WaitForSeconds(0.1f);

        if (indexEnnemi == indexSelected)
        {
            indexEnnemi = Random.Range(0, transform.childCount);
            Debug.Log("Change");
        }
        if (indexEnnemi != indexSelected)
        {
            transform.GetChild(indexEnnemi).gameObject.GetComponent<EnnemiAttack>().thisSelected = true;

            for (int j = 0; j < transform.childCount; j++)
            {
                if (j != indexEnnemi)
                    transform.GetChild(j).gameObject.GetComponent<EnnemiAttack>().thisSelected = false;
            }
        }

        yield break;
    }
    
    IEnumerator SelectSoloAttacker()
    {
        Debug.Log("SelectAttacker");
        launchSelectAttaker = true;
        yield return new WaitForSeconds(0.1f);

        transform.GetChild(0).gameObject.GetComponent<EnnemiAttack>().thisSelected = true;

        yield break;
    }

    IEnumerator RespawnEnnemi()
    {
        yield return new WaitForSeconds((int)Random.Range(13,17));

        GameObject clone = Instantiate(prefabEnnemiManager);

        clone.transform.parent = transform;
        clone.transform.localPosition = new Vector3(0,0,0);
        clone.GetComponent<EnnemiManager>().prefabEnnemiManager = prefabEnnemiManager;
        clone.SetActive(true);
        yield break;
    }
}