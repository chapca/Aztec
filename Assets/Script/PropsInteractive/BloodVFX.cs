using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodVFX : MonoBehaviour
{
    [SerializeField] float speedUpApparition, speedMoveToPlayer, speedUp;

    [SerializeField] Transform target;

    [SerializeField] bool moveToPlayer, isUp;

    [SerializeField] Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        myAnimator = GetComponent<Animator>();
        myAnimator.enabled = false;
        target = GameObject.FindWithTag("TargetBlood").transform;
        moveToPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPlayer)
        {
            if (transform.localPosition.y <= 1f && !isUp)
            {
                transform.Translate(Vector3.forward * speedUpApparition * Time.deltaTime);
            }
            else
            {
                isUp = true;
                myAnimator.enabled = true;
                transform.Translate(Vector3.forward * speedUp * Time.deltaTime);
                transform.position = Vector3.Slerp(transform.position, target.position, speedMoveToPlayer);
            }

            if (Vector3.Distance(transform.position, target.position) < 0.1f && isUp)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
