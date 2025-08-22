using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnemy : EnemyBase
{

   


    protected override void MoveTo(Vector3 targetPos)
    {
        if (!isLive)
            return;

        anim.SetInteger("EnemyState", 1);

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


    public void Idle()
    {
        if (anim) anim.SetInteger("EnemyState", 0);
    }

    protected virtual void Attack()
    {
        isMove = false;

        anim.SetInteger("EnemyState", 2);
    }

}
