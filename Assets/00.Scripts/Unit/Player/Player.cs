using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;


public class Player : Unit
{
    #region 변수들
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private Transform dustPos;
    public static Player Instance;
    public UnitType unitType = UnitType.Player;

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

    private float bulletSpeed = 20f;

    float handAngle;
    Vector2 target, mouse;
    #endregion

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
        FlipSpriteByMouse();

        Fire();
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
        moveSpeed = 5f;
        damage = 5f;
        attackCoolTime = 0.5f;
        isAttack = true;

        target = transform.position;

    }

    #region 움직임 
    private void ControlPlayer()
    {
        Move();
    }

    private void UpdateDirection()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            spriteRenderer.flipX = false;

        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            spriteRenderer.flipX = true;
        }
    }

    public override void Move()
    {
        UpdateDirection(); 

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

        animator.SetFloat("inPutX", movement.x);
        animator.SetFloat("inPutY", movement.y);

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
                    StartCoroutine(gun.Fire(UnitType.enemy));

                }
                else
                    StartCoroutine(gun.MeleeAttack());
            }

        }
        if (GunManager.Instance.curAmmo >= gun.ammoPerShot)
        {
            if (Input.GetKeyDown(KeyCode.R) && gun.isReloading == false)
            {
                StartCoroutine(gun.Reload());
            }
        }
    }
    private void HandAngle()
    {
        target = hand.transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouse - target;
        handAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        hand.transform.rotation = Quaternion.Euler(0, 0, handAngle);

        if (mouse.x < transform.position.x)
        {
            hand.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            hand.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    private void FlipSpriteByMouse()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            spriteRenderer.flipX = mouse.x < transform.position.x;
        }
    }

    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Bullet curBullet = collision.gameObject.GetComponent<Bullet>();
            if(curBullet.type == unitType)
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
        if (curHp <= 0)
        {
            Dead();
        }
        else
        {
            Debug.Log("피격 당함");
            curHp -= _damage;
        }
    }
}
