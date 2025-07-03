using UnityEngine;

public class Enemy : Unit
{
    private Rigidbody2D rb;
    private Animator anim;

    public LayerMask playerLayer;
    [SerializeField] private float attackRange = 0.5f;
    public Transform attackPos;

    public bool canDamage = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void MoveTo(Vector3 targetPos)
    {
        Debug.Log("¿Ãµø");
        Vector2 dir = (targetPos - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
     
        if (anim) anim.SetInteger("EnemyState", 0); 

        if(dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void Idle()
    {
        if (anim) anim.SetInteger("EnemyState", 0); 
    }

    public void Attack()
    {
        if (anim) anim.SetInteger("EnemyState", 1); 
      
    }

    public void DoDamage()
    {
        if (!canDamage) return;

        Collider2D hit = Physics2D.OverlapCircle(attackPos.position,attackRange);

       
            Player player = hit.GetComponent<Player>();
            if(player != null)
            {
                player.TakeDamage(damage);
                canDamage = false;
            }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void EndAttack()
    {
        anim.SetInteger("EnemyState", 0); 
    }
}
