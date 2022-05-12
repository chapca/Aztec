using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnEnnemi : MonoBehaviour
{
    [SerializeField] float x, y, z;
    [SerializeField] int nombreEnnemi;
    [SerializeField] GameObject prefabEnnemi;

    [SerializeField] bool once;

    private void Awake()
    {
        Debug.Log("zfzfzfz");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!once)
        {
            for (int i = 0; i < nombreEnnemi; i++)
            {
                GameObject clone = Instantiate(prefabEnnemi);
                clone.transform.parent = gameObject.transform;
                clone.transform.localPosition = new Vector3(x, y, z);
                x += 2;
                z += Random.Range(-2, 2);

                if(i == nombreEnnemi-1)
                {
                    once = true;
                }
            }
        }
    }
}
