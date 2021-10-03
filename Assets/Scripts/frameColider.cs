using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameColider : MonoBehaviour
{
    public bool access;
    

    public GameManager gamemanager;



    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D c)
    {
        if (access)
        {
            if (c.gameObject.tag == "frame")
                gamemanager.outside += 1;
            else if (c.gameObject.tag == "inside")
            {
                //Debug.Log(c.gameObject.name);
                gamemanager.inside += 1;
            }
        }
    }
}
