using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    Battle battle;

    [SerializeField] int state;

    [SerializeField] float timer, setMaxTimer;

    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.FindWithTag("Player").GetComponent<Battle>();

        timer = setMaxTimer;
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!battle.isAttacked)
        {
            myAnimator.SetBool("Deploiment", false);

            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                if(state < 4)
                {
                    state++;
                    myAnimator.SetTrigger(("Idle" + state).ToString());
                    timer = setMaxTimer;
                }
                else
                {
                    state = 0;
                    timer = setMaxTimer;
                }
            }
        }
        else
        {
            myAnimator.SetBool("Deploiment", true);
        }
    }
}