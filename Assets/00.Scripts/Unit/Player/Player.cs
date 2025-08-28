using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : Unit
{
    #region 변수들
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private Transform dustPos;
    public static Player Instance;
    public UnitType unitType = UnitType.Player;

    [SerializeField] private Image hitEffect;
    private bool isInvincible = false;
    [SerializeField] private Transform handPos1;
    [SerializeField] private Transform handPos2;

    #region compoment
    [Header("컴포넌트들")]
    private Rigidbody2D rigidbody2D;
    private Vector2 movement;
    private Vector2 dir;
    public float angle;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region 구르기
    [Header("구르기 관련")]
    private Vector2 lastMoveDirection = Vector2.right;
    [SerializeField]
    private bool isRoll = true;
    [SerializeField]
    private float rollSpeed = 8f;
    private float rollCooltime = 1f;
    public float rollAnimTime = 0;
    #endregion

    #region 총 
    [Header("총 관련")]
    public GameObject hand;
    public Transform aimingPoint;
    public Gun gun;
    public ParticleSystem cartridgeCase;

    private float bulletSpeed = 20f;

    float handAngle;
    Vector2 target, mouse;
    [SerializeField]
    private FireEffct fireEffect;

    #endregion

    public Text noAmmoText;
    public float showTime = 1.5f;

    private bool isTextShowing = false;

    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else Instance = this;


        Init();
    }
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        AimingPoint();
        Roll();
        HandAngle();

        Fire();
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(1f);
        }
    }

    private void FixedUpdate()
    {
        ControlPlayer();
    }

    private void Init()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
        maxHp = 20f;
        curHp = maxHp;
        attackCoolTime = 0.5f;
        isAttack = true;

        target = transform.position;
        noAmmoText.gameObject.SetActive(false);

        cartridgeCase.Stop();
    }

    #region 움직임 
    private void ControlPlayer()
    {
        Move();
        FlipByMouse();
    }


    private void FlipByMouse()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreen);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (mouseWorldPos.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
            hand.transform.position = handPos1.position;
        }
        else
        {
            spriteRenderer.flipX = false;
            hand.transform.position = handPos2.position;
        }
    }


    public override void Move()
    {

        if (!isMove) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            moveing = true;
            OnAnim(1);
            lastMoveDirection = movement.normalized;

            if (!dust.isPlaying)
                dust.Play();
        }
        else
        {
            moveing = false;
            OnAnim(0);
            if (dust.isPlaying)
                dust.Stop();
        }

        rigidbody2D.velocity = movement.normalized * moveSpeed;
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isRoll)
        {
            isMove = false;
            isRoll = false;
            isAttack = false;

            OnAnim(2);

            Vector2 rollDirection = (movement != Vector2.zero) ? movement.normalized : lastMoveDirection;

            rigidbody2D.velocity = rollDirection * rollSpeed;
            rollAnimTime = 0;
            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            for (int i = 0; i < controller.animationClips.Length; i++)
            {
                rollAnimTime = i * 0.1f;
            }
            StartCoroutine(RollAnim());

        }
    }
    private IEnumerator RollAnim()
    {
        yield return new WaitForSeconds(rollAnimTime);
        isMove = true;
        isAttack = true;
        OnAnim(0);
        StartCoroutine(EndRoll());

    }

    private IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(rollCooltime);
        isRoll = true;
        //animator.SetBool("isRoll", false);

    }
    #endregion

    #region 공격

    private void AimingPoint()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        aimingPoint.transform.position = point;
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gun.isAtttack == true && gun.isReloading == false)
            {
                if (gun.curAmmo >= gun.ammoPerShot)
                {
                    fireEffect.FireAnim(gun.element);
                    CameraShake.Instance.OnShakeCamera(0.25f, 0.23f);
                    StartCoroutine(gun.Fire(UnitType.enemy));
                }
                else
                {
                    ShowNoAmmoText();
                    StartCoroutine(gun.MeleeAttack());

                }
            }

        }
        if (GunManager.Instance.curAmmo >= gun.ammoPerShot)
        {
            if (Input.GetKeyDown(KeyCode.R) && gun.isReloading == false)
            {
                cartridgeCase.Play();
                StartCoroutine(gun.Reload());
            }
        }
    }

    private void HandAngle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorld - hand.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool isLeft = mouseWorld.x < transform.position.x;

        if (isLeft)
        {
            hand.transform.rotation = Quaternion.Euler(0, 0, angle);
            hand.transform.localScale = new Vector3(1, -1, 1);
            hand.transform.position = handPos1.position;
        }
        else
        {
            hand.transform.rotation = Quaternion.Euler(0, 0, angle);
            hand.transform.localScale = new Vector3(1, 1, 1);
            hand.transform.position = handPos2.position;
        }
    }


    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet curBullet = collision.gameObject.GetComponent<Bullet>();
            if (curBullet.type == unitType)
            {
                TakeDamage(curBullet.damage);
                curBullet.Return();
            }
        }
    }

    public void OnAnim(int _animN)
    {
        animator.SetInteger("PlayerSet", _animN);

    }
    public override void TakeDamage(float _damage)
    {

        Debug.Log("피격 당함");
        curHp -= _damage;
        GameManager.Instance.ChageHp();
        ; StartCoroutine(HitEffect());
        StartCoroutine(Invincible(0.5f));
        if (curHp <= 0)
        {
            //Dead();
        }
    }

    public virtual void Dead()
    {
        hand.SetActive(false);
        OnAnim(3);
    }
    public void EndDir()
    {
        CircleEffect.Instance.LoadScene("Title");

    }
    private IEnumerator HitEffect()
    {
        if (hitEffect == null) hitEffect = GameObject.Find("Hit").GetComponent<Image>();
        Color color = hitEffect.color;
        color.a = 0.4f;
        hitEffect.color = color;

        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            hitEffect.color = color;

            yield return null;
        }

    }

    void ShowNoAmmoText()
    {
        if (!isTextShowing)
        {
            StartCoroutine(ShowTextCoroutine());
        }
    }

    private IEnumerator Invincible(float duration)
    {
        isInvincible = true;
        float elapsed = 0f;
        float blinkInterval = 0.15f;

        Color color = spriteRenderer.color;

        while (elapsed < duration)
        {
            color.a = 0.4f;
            spriteRenderer.color = color;
            hand.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(blinkInterval);

            color.a = 1f;
            spriteRenderer.color = color;
            hand.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2;
        }

        color.a = 1f;
        spriteRenderer.color = color;
        isInvincible = false;
    }

    private IEnumerator ShowTextCoroutine()
    {
        isTextShowing = true;
        noAmmoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        noAmmoText.gameObject.SetActive(false);
        isTextShowing = false;
    }
}
