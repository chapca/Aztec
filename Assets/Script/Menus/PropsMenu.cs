using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropsMenu : MonoBehaviour
{
    [SerializeField] GameObject prefabProps;
    [SerializeField] Transform spawnProps;

    [SerializeField] GameObject cloneProps;

    EventSystem m_EventSystem;

    [SerializeField]
    Transform listButton;

    [SerializeField] float rotationY, rotSpeedY;

    void Start()
    {
        m_EventSystem = EventSystem.current;
        StartCoroutine("SelectFirstButton");
    }

    IEnumerator SelectFirstButton()
    {
        yield return new WaitForEndOfFrame();
        listButton.GetChild(0).transform.GetComponent<Button>().Select();
        yield break;
    }

    void Update()
    {
        if (spawnProps.childCount > 0)
        {
            if (spawnProps.GetChild(0).gameObject == cloneProps)
            {
                rotationY -= rotSpeedY * Time.deltaTime * Input.GetAxis("RightJoystickX");
                cloneProps.transform.rotation = Quaternion.Euler(0, rotationY, 0);
            }
        }
    }

    public void InstanciateProps()
    {
        if(cloneProps == null)
        {
            if (spawnProps.childCount > 0)
            {
                for (int i = 0; i < spawnProps.childCount; i++)
                {
                    Destroy(spawnProps.GetChild(i).gameObject);
                }
            }
            cloneProps = Instantiate(prefabProps);
            cloneProps.transform.parent = spawnProps;
            cloneProps.transform.localPosition = Vector3.zero;
            cloneProps.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}