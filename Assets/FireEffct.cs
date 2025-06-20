using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffct : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    
    public void FireAnim(Element element)
    {
        int elementIndex = (int)element;
        A_OnToggle(1);
        animator.SetInteger("fire", elementIndex);
    }

    public void A_OnToggle(int _n)
    {
        if(_n == 0) gameObject.SetActive(false);
        else gameObject.SetActive(true);

    }
   
}
