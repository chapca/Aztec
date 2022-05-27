using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCursor : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField] float distanceFeedBack;

    Transform player;

    [SerializeField] Camera cam;

    [SerializeField] GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= distanceFeedBack)
        {
            Vector3 playerPos;
            playerPos.x = player.position.x;
            playerPos.y = player.position.y +1;
            playerPos.z = player.position.z;

            Physics.Raycast(transform.position, transform.TransformDirection(playerPos - transform.position), out hit, distanceFeedBack + 1);
            Debug.DrawRay(transform.position, transform.TransformDirection(playerPos - transform.position)* distanceFeedBack, Color.red);

            Debug.Log(hit.transform.gameObject.name);

            if (hit.transform.gameObject.name == "EmptyPlayer")
            {
                cursor.SetActive(true);

                Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
                cursor.transform.position = screenPoint;
            }
            else
            {
                cursor.SetActive(false);
            }
        }
    }
}