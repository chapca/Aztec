using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ShededBlood : MonoBehaviour
{
    DecalProjector decal;

    [SerializeField] Vector3 decalSize;

    float x;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        decal = GetComponent<DecalProjector>();
        decal.size = decalSize;
        StartCoroutine("Sheded");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Sheded()
    {
        x += 0.1f;
        decalSize = new Vector3(x, x, 5);
        decal.size = decalSize;
        yield return new WaitForSeconds(0.01f);

        if(decal.size.x >=10)
        {
            decal.size = decalSize;
        }
        else
        {
            StartCoroutine("Sheded");
        }
    }
}
