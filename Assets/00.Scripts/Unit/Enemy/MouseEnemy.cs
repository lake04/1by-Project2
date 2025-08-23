using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnemy : EnemyBase
{

    protected void Update()
    {
        if (!isLive)
            return;
        Vector2 attackDir = spriter.flipX ? Vector2.right : Vector2.left;

        float dist = Vector2.Distance(transform.position, target.transform.position);
        RaycastHit2D raycastHit2 = Physics2D.Raycast(transform.position, attackDir, attackRange, playerLayer);
        Debug.DrawRay(transform.position, attackDir * attackRange, Color.red);
        if (raycastHit2.collider != null && raycastHit2.collider.CompareTag("Player"))
        {
            Debug.Log("Player 공격 범위 안!");
            StartCoroutine(Attack());
        }
        else
        {
            MoveTo(target.transform.position);
        }
    }

    protected override void MoveTo(Vector3 targetPos)
    {
        if (!isLive)
            return;

        //anim.SetInteger("EnemyState", 1);

        Vector2 targetPod = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 dirVec = targetPod - rb.position;
        Vector2 nexVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nexVec);
        rb.velocity = Vector2.zero;

        spriter.flipX = dirVec.x < 0 ? false : true;
        if (spriter.flipX)
        {
            attackObject.transform.localPosition =
       new Vector3(-origtnPos.x, origtnPos.y, origtnPos.z);
        }
        else
        {
            attackObject.transform.localPosition = origtnPos;
        }

    }



 

}
