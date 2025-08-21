using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    public GameObject hitPartcle;

    public Element element;
    public UnitType type;
    public bool isPooled;
    private Rigidbody2D rigid;
    private float speed;
    private TrailRenderer trail;
    private Vector3 startPos;
    private float maxDistanc = 20f;

    private void Start()
    {
        transform.tag = "Bullet";
    }

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        rigid.velocity = Vector2.zero;
        startPos = transform.position;

        if (trail != null) trail.Clear();

    }

    void Update()
    {
        float curPos = Vector2.Distance(startPos, transform.position);
        if(curPos >= maxDistanc)
        {
            Return();
        }
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            GameObject curHit = hitPartcle;
            if (hitPartcle != null)
            {
                PoolingManager.Instance.GetObject(curHit, transform.position, Quaternion.Euler(0, transform.eulerAngles.y + 180, 0));
            }
            Return();
        }

        if(collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            GameObject curHit = hitPartcle;
            if (hitPartcle != null)
            {
                PoolingManager.Instance.GetObject(curHit, transform.position, Quaternion.Euler(0, transform.eulerAngles.y + 180, 0));
            }
            Return();
        }

    }

    public void Setting(UnitType _type,float _damage, Element _element,float _speed, Vector3 dir)
    {
        element = _element;
        damage  = _damage;
        type    = _type;
        rigid.velocity = dir * _speed;
        if (trail != null) trail.Clear();
    }

  

    public void Return()
    {
        if (isPooled)
        {
            rigid.velocity = Vector3.zero;
            PoolingManager.Instance.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        trail.Clear();
        rigid.velocity = Vector2.zero;
    }
}
