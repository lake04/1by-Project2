using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        this.maxHp = 2;
        curHp= maxHp;
        this.damage = 1;

        moveSpeed = 5f;

        attackCoolTime = 1.5f;
    }
}
