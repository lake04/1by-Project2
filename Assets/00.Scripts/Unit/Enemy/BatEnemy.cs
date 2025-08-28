using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : EnemyBase
{

    //public virtual void MoveTo(Vector3 targetPos)
    //{
    //    if (!isLive)
    //        return;

    //    Vector2 targetPod = new Vector2(target.transform.position.x, target.transform.position.y);
    //    Vector2 dirVec = targetPod - rb.position;
    //    Vector2 nexVec = dirVec.normalized * speed * Time.fixedDeltaTime;

    //    rb.MovePosition(rb.position + nexVec);
    //    rb.velocity = Vector2.zero;
      
    //    spriter.flipX = dirVec.x < 0 ? true : false;
    //    if (spriter.flipX)
    //    {
    //        attackObject.transform.localPosition =
    //   new Vector3(-origtnPos.x, origtnPos.y, origtnPos.z);
    //    }
    //    else
    //    {
    //        attackObject.transform.localPosition = origtnPos;
    //    }
     
    //}



  

    public void DoDamage()
    {
        if (!canDamage) return;

        attackObject.SetActive(true);
    }
}
