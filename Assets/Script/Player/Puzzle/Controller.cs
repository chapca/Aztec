using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] bool isBlock;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float bloodUsed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float valX = 0;
        float valY = 0;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                valX = 0;
                valY = -100;
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                valX = 0;
                valY = 100;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                valX = -100;
                valY = 0;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                valX = 100;
                valY = 0;
            }
        }

        ApplyMovement(valX, valY);
    }

    public virtual bool ApplyMovement(float valX, float valY)
    {
        if (valX != 0 || valY != 0)
        {
            Collider2D myCollid = Physics2D.OverlapBox(new Vector2(transform.position.x + valX, transform.position.y + valY), new Vector2(0.5f, 0.5f), 0, layerMask);

            Debug.Log(myCollid);

            if (myCollid == null || myCollid.gameObject.layer == 10 || myCollid.gameObject.layer == 7 || myCollid.gameObject.layer == 3)
            {
                transform.Translate(valX, valY, 0);
                BloodSpent();

                if (myCollid != null)
                    if (myCollid.gameObject.layer == 3)
                        CaseIce(valX, valY);

                if (myCollid != null)
                    CheckNewPlace(myCollid);

                return true;
            }
        }
        return false;
    }

    void CheckNewPlace(Collider2D myCollid)
    {
        Debug.Log(myCollid);

        if (myCollid.gameObject.layer == 10)
        {
            myCollid.gameObject.SetActive(false);
        }
        if (myCollid.gameObject.layer == 7)
        {
            myCollid.gameObject.SetActive(false);
        }
    }

    void CaseIce(float valX, float valY)
    {
        Collider2D myCollid = Physics2D.OverlapBox(new Vector2(transform.position.x + valX, transform.position.y + valY), new Vector2(0.5f, 0.5f), 0, layerMask);

        if (myCollid == null || myCollid.gameObject.layer == 10 || myCollid.gameObject.layer == 7 || myCollid.gameObject.layer == 3)
        {
            transform.Translate(valX, valY, 0);
            BloodSpent();

            if (myCollid != null)
                CheckNewPlace(myCollid);
        }
    }

    void BloodSpent()
    {
        PlayerBlood.LooseBlood(bloodUsed);
    }
}
