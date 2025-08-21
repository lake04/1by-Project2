using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Rigidbody2D rb;
    public Animator anim;
    private SpriteRenderer spriter;

    public LayerMask playerLayer;
    [SerializeField] public float attackBoxRange = 0.5f;
    public Transform attackPos;
    public float speed;
    public bool canDamage = true;

    public RuntimeAnimatorController[] animCon;
    public GameObject target;

    bool isLive = true;
    public float attackRange = 3f;
    public float attackDelay = 1.2f;

    private bool isAttacking = false;
  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        curHp = maxHp;

        StartCoroutine(AIThinkRoutine());
    }
    private void FixedUpdate()
    {
       
        MoveTo(target.transform.position);
    }

    public void MoveTo(Vector3 targetPos)
    {
        if (!isLive || isAttacking)
            return;

        Vector2 targetPod = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 dirVec = targetPod - rb.position;
        Vector2 nexVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nexVec);
        rb.velocity = Vector2.zero;

        spriter.flipX = dirVec.x < 0 ? true : false;
    }

    IEnumerator AIThinkRoutine()
    {
        while (true)
        {
          
            yield return new WaitForSeconds(0.1f);

            if (target == null) continue;

            float dist = Vector2.Distance(transform.position, target.transform.position);

            if (dist < attackRange)
            {

                if (!isAttacking)
                {
                    StartCoroutine(AttackRoutine());
                }
            }
          
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        canDamage = true;
        Attack();

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
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

        Collider2D hit = Physics2D.OverlapCircle(attackPos.position, attackBoxRange);

        Player player = hit.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            canDamage = false;
        }
    }

    public void Init(SpawnData data,GameObject _target)
    {
        int anIndex = Random.Range(0, data.spriteType.Length);
        anim.runtimeAnimatorController = animCon[anIndex];
        speed = data.speed;
        maxHp = data.health;
        curHp = data.health;
        target = _target;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackBoxRange);
    }

    public virtual void TakeDamage(float _damage)
    {
        Debug.Log("피격 당함");
        curHp -= _damage;
        if (curHp <= 0)
        {
            Die();
        }
    }

    public void EndAttack()
    {
        anim.SetInteger("EnemyState", 0);
        isMove = true;

    }

    public void Die()
    {
        isLive = false;
        GameManager.Instance.kill++;
        anim.SetInteger("EnemyState", 2);
    }

    public void EndDie()
    {
        Destroy(gameObject,0.2f);
    }
}
