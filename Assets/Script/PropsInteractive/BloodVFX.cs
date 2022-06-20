using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodVFX : MonoBehaviour
{
    [SerializeField] float speedUpApparition, speedMoveToPlayer, speedUp;

    [SerializeField] Transform target;

   [SerializeField] bool moveToPlayer, isUp;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        target = GameObject.FindWithTag("TargetBlood").transform;
        moveToPlayer = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveToPlayer)
        {
            if (transform.localPosition.y <= 1f && !isUp)
            {
                transform.Translate(Vector3.up * speedUpApparition * Time.fixedDeltaTime);
            }
            else
            {
                isUp = true;
                transform.Translate(Vector3.up * speedUpApparition * Time.fixedDeltaTime);
                transform.position = Vector3.Slerp(transform.position, target.position, speedMoveToPlayer);
            }

            if (Vector3.Distance(transform.position, target.position) < 0.01f && isUp)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
