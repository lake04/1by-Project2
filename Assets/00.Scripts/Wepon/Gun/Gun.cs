using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Element
{
    None = 0,
    Fire = 1,
    Water = 2,
    Ice = 3,
    Electricity = 4
}

public class Gun : MonoBehaviour
{
    #region 변수들
    public int gunID;
    public string gunName;

    public GunKind gunKind;
    public Element element;

    #region 스탯
    [Header("스탯")]
    public int damage;
    public int ammoPerShot;
    public int bulletsPerShot;
    public float bulletSpread;
    public float reloadTime;
    public float fireRate;
    public float bulletSpeed;


    private Vector2 lastMoveDirection = Vector2.right;
    public Vector2 meleeSize = new Vector2(1.5f, 1.0f);
    public float meleeOffset = 1.0f;
    #endregion

    public bool isAtttack = true;

    [Header("탄창 관련")]
    public int maxAmmo;
    public int curAmmo;
    public bool isReloading = false;

    public bool canEquipParts;
    public int parts;

    [SerializeField] private Transform aimingPoint;

    [Header("오브젝트들 ")]
    public GameObject bulletPrefab;
    public GameObject firePos;
    public GameObject fireEffctePrefba;
    [SerializeField] private Slider slider;
    #endregion

    void Start()
    {
        Init(GunManager.Instance.gunDatas[0]);
    }

    void Update()
    {

    }

    public void Init(GunData gunData)
    {
        gunID = gunData.gunID;
        gunName = gunData.gunName;
        gameObject.GetComponent<SpriteRenderer>().sprite = GunManager.Instance.sprites[gunID];
        gunKind = gunData.gunKind;
        element = gunData.element;
        damage = gunData.damage;
        ammoPerShot = gunData.ammoPerShot;
        bulletsPerShot = gunData.bulletsPerShot;
        bulletSpread = gunData.bulletSpread;
        reloadTime = gunData.reloadTime;
        fireRate = gunData.fireRate;
        bulletSpeed = gunData.bulletSpeed;
        canEquipParts = gunData.canEquipParts;
        parts = gunData.parts;
        maxAmmo = gunData.maxAmmo;
        curAmmo = gunData.maxAmmo;
        slider.gameObject.SetActive(false);
    }

    public IEnumerator MeleeAttack()
    {
        isAtttack = false;
        Vector2 attackDir = (aimingPoint.position - transform.position).normalized;
        Vector2 attackCenter = (Vector2)transform.position + attackDir * meleeOffset;
        float attackAngle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, meleeSize, attackAngle);

        foreach (Collider2D collider in hits)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        lastMoveDirection = attackDir;
        yield return new WaitForSeconds(AttackTime());
        isAtttack = true;
    }

    public IEnumerator Reload()
    {
        if (curAmmo == maxAmmo) yield break;
        isReloading = true;
        Debug.Log("재장전 시작");
        GunManager.Instance.curAmmo -= (maxAmmo - curAmmo);

        slider.gameObject.SetActive(true);
        float timer = 0f;
        slider.maxValue = reloadTime * 10;
        slider.value = 0f;

        while (timer < reloadTime / 5)
        {
            timer += Time.deltaTime;
            slider.value = timer * 50;
            yield return null;
        }

        //yield return new WaitForSeconds(reloadTime / 10);
        curAmmo = maxAmmo;
        //UiManager.instance.UpdateUI();
        isReloading = false;
        slider.gameObject.SetActive(false);
    }

    private float AttackTime()
    {
        return 1f / fireRate;
    }

    public IEnumerator Fire(UnitType _type)
    {
        isAtttack = false;
        fireEffctePrefba.GetComponent<FireEffct>().FireAnim(element);

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Sprite bulletSprite = BulletManager.Instance.GetBulletSprite(gunKind, element);
            GameObject bulletObj = PoolingManager.Instance.GetObject(bulletPrefab, transform.position, Quaternion.identity);
            if (bulletObj == null) continue;

            Bullet bullet = bulletObj.GetComponent<Bullet>();

            bullet.isPooled = true;
            bullet.Setting(_type, damage, element, bulletSpeed);
            bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;

            Vector2 dir = (aimingPoint.position - transform.position).normalized;
            float angleOffset = UnityEngine.Random.Range(-bulletSpread, bulletSpread);
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;

            Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }
        }

        curAmmo -= ammoPerShot;
        yield return new WaitForSeconds(AttackTime());
        isAtttack = true;
    }

}
