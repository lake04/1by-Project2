using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : Unit
{
    public Rigidbody2D rb;
    public Animator anim;
    protected SpriteRenderer spriter;

    public LayerMask playerLayer;
    [SerializeField] public float attackBoxRange = 0.5f;

    public GameObject attackObject;
    protected Vector3 origtnPos;
    public float speed;
    public bool canDamage = true;

    public RuntimeAnimatorController[] animCon;
    public GameObject target;

    public bool isLive = true;
    public float attackRange = 3f;
    public float attackDelay = 1.2f;

    protected bool isAttacking = false;

    private void OnEnable()
    {
        curHp = maxHp;
        attackObject.GetComponent<EnemyAttackOb>().damage = damage;
        attackObject.SetActive(false);
        isLive = true;
    }

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        origtnPos = attackObject.transform.localPosition;

    }


    protected void Update()
    {
        if (!isLive)
            return;
        Vector2 attackDir = spriter.flipX ? Vector2.left : Vector2.right;

        float dist = Vector2.Distance(transform.position, target.transform.position);
        RaycastHit2D raycastHit2 = Physics2D.Raycast(transform.position, attackDir, attackRange,playerLayer);
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

    protected virtual void MoveTo(Vector3 targetPos)
    {
        if (!isLive)
            return;

        Vector2 targetPod = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 dirVec = targetPod - rb.position;
        Vector2 nexVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nexVec);
        rb.velocity = Vector2.zero;

        spriter.flipX = dirVec.x < 0 ? true : false;
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


    protected virtual IEnumerator Attack()
    {
        if (isAttacking)
            yield return 0;
            isAttacking = true;
        canDamage = true;

        isMove = false;

        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
        canDamage = false;
    }

    protected virtual void Idle()
    {
        if (anim) anim.SetInteger("EnemyState", 0);
    }


    protected virtual void DoDamage()
    {
        if (!canDamage) return;

        attackObject.SetActive(true);
    }

    public virtual void Init(SpawnData data, GameObject _target)
    {
        speed = data.speed;
        maxHp = data.health;
        curHp = data.health;
        target = _target;
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

    protected virtual void EndAttack()
    {
        attackObject.SetActive(false);
        anim.SetInteger("EnemyState", 0);
        isMove = true;

    }

    protected virtual void Die()
    {
        isLive = false;
        GameManager.Instance.kill++;
        anim.SetTrigger("Die");
    }

    protected virtual void EndDie()
    {
        PoolingManager.Instance.Return(gameObject,0.2f);

    }

  
}
