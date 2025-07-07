using UnityEngine;

public class Enemy : Unit
{
    public Rigidbody2D rb;
    public Animator anim;

    public LayerMask playerLayer;
    [SerializeField] public float attackRange = 0.5f;
    public Transform attackPos;

    public bool canDamage = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        curHp = maxHp;
    }

    public void MoveTo(Vector3 targetPos)
    {
    {
        Vector2 dir = (targetPos - transform.position).normalized;

        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (anim) anim.SetInteger("EnemyState", 0);

        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

}

public void Idle()
    {
        if (anim) anim.SetInteger("EnemyState", 0);
    }

    public void Attack()
    {
       isMove = false;
        if (anim) anim.SetInteger("EnemyState", 1);

    }

    public void DoDamage()
    {
        if (!canDamage) return;

        Collider2D hit = Physics2D.OverlapCircle(attackPos.position, attackRange);

        Player player = hit.GetComponent<Player>();
        if (player != null)
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

    public virtual void TakeDamage(float _damage)
    {
        if (curHp <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("피격 당함");
            curHp -= _damage;
        }
    }

    public void EndAttack()
    {
        anim.SetInteger("EnemyState", 0);
        isMove = true;

    }

    public void Die()
    {
        anim.SetInteger("EnemyState", 2);
    }

    public void EndDie()
    {
        Destroy(gameObject,0.5f);

    }
}
