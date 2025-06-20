using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{
   public GameObject bk;
    private int weghit = 130;
    private int hithet = 1080;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        if (point.x<weghit && point.x > -weghit && point.y < hithet)
        {
            bk.transform.position = new Vector2(point.x,0);

        }
    }
}
