using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    public GameObject hitPartcle;

    public Element element;
    public UnitType type;
    public bool isPooled;

    private void Start()
    {
        transform.tag = "Bullet";
    }

    private void OnEnable()
    {
        Invoke("Return", 1f);

    }

    void Update()
    {

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

    }

    public void Setting(UnitType _type,float _damage, Element _element, float _speed)
    {
        element = _element;
        damage  = _damage;
        type    = _type;

    }

    public void Return()
    {
        if (isPooled)
        {
            PoolingManager.Instance.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
       
    }
}
