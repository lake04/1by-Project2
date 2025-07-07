using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float searchRange = 5f;
    public float attackRange = 3f;
    public float attackDelay = 1.2f;

    private Enemy enemy;
    private bool isAttacking = false;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = GameObject.FindWithTag("Player")?.transform;

        StartCoroutine(AIThinkRoutine());
    }

    IEnumerator AIThinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (target == null) continue;

            float dist = Vector2.Distance(transform.position, target.position);

            if (dist < attackRange)
            {

                if (!isAttacking)
                {
                    StartCoroutine(AttackRoutine());
                }
            }
            else if(dist < searchRange )
            {

                if (dist >attackRange)
                {

                }
               
                    enemy.MoveTo(target.position);

            }
            else
            {
                enemy.Idle();
            }
        }
        
    }
 
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        enemy.canDamage = true;
        enemy.Attack();

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
    }


}
