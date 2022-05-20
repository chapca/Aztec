using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levier : MonoBehaviour
{
    public bool isActive;

    [SerializeField] Transform door;

    [SerializeField] Interaction interaction;

    AudioSource doorAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.transform.GetChild(1);

        interaction = GameObject.FindWithTag("Player").transform.parent.GetComponent<Interaction>();

        doorAudioSource = transform.parent.transform.GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if(door.transform.localPosition.y <6)
            {
                door.Translate(Vector3.up * 5 * Time.deltaTime);
            }
            else
            {
                gameObject.tag = "Untagged";
                interaction.EnableCamInteraction(false);
            }
        }
    }
}