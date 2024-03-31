using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Character
{
    #region 参数
    [Header("-----生命值参数-----")]
    [SerializeField] private bool regenerateHealth = true;//是否再生生命
    [SerializeField] private float healthRegerateTime;//再生时间
    [SerializeField] private float healthRegeratePercent;//生命值再生百分比
    [SerializeField] private StatsBar_HUD _statsBarHUD;

    readonly float InvincibleTime = 1f;//无敌时间
    WaitForSeconds waitInvincibleTime;
    
    [Header("-----input-----")]
    [SerializeField] private PlayerInput _input;

    [Header("移动相关")]   
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelerationTime;//加速时间
    [SerializeField] private float decelerationTime;//减速时间
    [SerializeField] private float moveRotationAngle;//移动时的偏转偏转角度
    Vector2 moveDirection;

    [Header("射弹相关")] 
    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2; 
    [SerializeField] private GameObject projectile3;
    [SerializeField] private Transform muzzletop;
    [SerializeField] private Transform muzzlemid;
    [SerializeField] private Transform muzzlebottom;
    [SerializeField] ParticleSystem muzzleVFX;
    [SerializeField, Range(0, 2)] private int weaponPower = 0;
    [SerializeField] private float fireInterval = .2f;//射击间隔

    [SerializeField] private GameObject projectileOverDrive;//暴走时的射弹
    
    [Header("能力相关")] 
    [SerializeField,Range(0,100)] private int dodgeEnergyCost = 20;
    [SerializeField] private float maxRoll = 720f;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private bool isDodging = false;
    [SerializeField] private Vector3 dodgeScale = new Vector3(.5f,.5f,.5f);
    [SerializeField] private float slowMotionDuration = .1f;
    
    [Header("音效播放")] 
    [SerializeField] private AudioData playerProjectileAudioData;//指定玩家射弹音效
    [SerializeField] private AudioData playerDodgeAudioData;//指定玩家闪避音效
    
    [Header("OverDrive")] 
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private int overdriveDodgeFactor = 2;
    [SerializeField] private float overdriveFireFactor = 1.2f;
    [SerializeField] private float BulletKeepingDuration = 1f;
    [SerializeField] private float BulletInDuration = .8f;
    [SerializeField] private float BulletOutDuration = .8f;
    
    
    bool isOverDriving = false;
    private WaitForSeconds waitForOverdriveFireInterval;//开火间隔：能量爆发时
    
    private float paddingX;
    private float paddingY;
    
    MissileSystem missile;//导弹系统
    private Rigidbody2D playerRigid;
    private Collider2D playerCollider;
    private Coroutine moveCoroutine;//标识移动协程
    private Coroutine healthRegenateCoroutine;//标识生命值再生协程，实际上只有带参数的协程才这么标识，为了方便停止关闭。
    private float currentRoll;
    private Vector2 previousVelocity;
    private Quaternion previousRotation;
    private WaitForSeconds Interval;//实际的间隔,需要重新设置开火间隔
    private WaitForSeconds waitHealthRegenateTime;//实际的间隔,需要重新设置开火间隔
    
    
    private float t;
    WaitForFixedUpdate waitforfixedupdate = new WaitForFixedUpdate();

    
    
    #endregion
    
    #region 生命周期函数
    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();
        
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;
        
        Interval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);
        waitHealthRegenateTime = new WaitForSeconds(healthRegerateTime);
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);
    }

    private void Start()
    {
        playerRigid.gravityScale = 0;
        _statsBarHUD.Initialize(health,maxHealth);
        _input.EnableGameplayInput();
        
        //TakeDamage(50);
    }

    // public override void RestoreHealth(float value)
    // {
    //     base.RestoreHealth(value);
    //     Debug.Log("当前生命值："+health+"时间:"+Time.time);
    // }

    protected override void OnEnable()
    {
        base.OnEnable();
        _input.onMove += Move;
        _input.onStopMove += StopMove;
        _input.onFire += Fire;
        _input.onStopFire += StopFire;
        _input.onDodge += Dodge;
        _input.onOverdrive += OverDrive;
        _input.onLaunchMissile += LaunchMissile;

        PlayerOverDrive.on += OverDriveOn;
        PlayerOverDrive.off += OverDriveOff;
    }
    
    private void OnDisable()
    {
        _input.onMove -= Move;
        _input.onStopMove -= StopMove;
        _input.onFire -= Fire;
        _input.onStopFire -= StopFire;
        _input.onDodge -= Dodge;
        _input.onOverdrive -= OverDrive;
        _input.onLaunchMissile -= LaunchMissile;

        PlayerOverDrive.on -= OverDriveOn;
        PlayerOverDrive.off -= OverDriveOff;
    }
    #endregion
    
    #region 移动控制
    private void Move(Vector2 moveInput)
    {
        //playerRigid.velocity = moveInput * moveSpeed;直接将速度设置为某个值
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        //Quaternion.AngleAxis(moveRotationAngle*moveInput.y,Vector3.right) 乘上input.y值使得旋转角度随着玩家输入的y值（上下移动时）而改变大小，Vector3.right指定旋转轴为x轴（红色）。
        moveDirection = moveInput.normalized;
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized*moveSpeed,Quaternion.AngleAxis(moveRotationAngle*moveInput.y,Vector3.right)));
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    
    private void StopMove()
    {
        //playerRigid.velocity = Vector2.zero;直接将速度设为0
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = Vector2.zero;
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, moveDirection,Quaternion.identity));
        StopCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    IEnumerator MoveRangeLimitationCoroutine()
    {
        while (true)
        {
            transform.position = ViewPort.Instance.ClampPlayerMoveableArea(transform.position,paddingX,paddingY);
            yield return null;
        }
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = playerRigid.velocity;
        previousRotation = transform.rotation;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            playerRigid.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);
            yield return waitforfixedupdate;
        }
    }
    #endregion
    
    #region 开火的控制
    //开火
    private void Fire()
    {
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }
    
    
    //停火
    private void StopFire()
    {
        muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }
    
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverDriving ? projectileOverDrive : projectile1, muzzlemid.position);
                    break;
                case 1:
                    PoolManager.Release(isOverDriving ? projectileOverDrive : projectile1, muzzlemid.position);
                    PoolManager.Release(isOverDriving ? projectileOverDrive : projectile1, muzzlebottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverDriving ? projectileOverDrive : projectile2, muzzletop.position);
                    PoolManager.Release(isOverDriving ? projectileOverDrive : projectile1, muzzlemid.position);
                    PoolManager.Release(isOverDriving ? projectileOverDrive : projectile3, muzzlebottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(playerProjectileAudioData);
            
            //yield return Interval;
            
            // if (isOverDriving)
            // {
            //     yield return waitForOverdriveFireInterval;
            // }
            // else
            // {
            //     yield return Interval;
            // }
            yield return isOverDriving ? waitForOverdriveFireInterval : Interval ;
        }
    }
    
    
    void LaunchMissile()
    {
        //Release a missile clone from object pool
        missile.Lauch(muzzlemid);
        //play the sfx;
    }

    public void PickUpMissile()
    {
        missile.PickUp();
    }
    
    #endregion
    
    #region health

    public bool IsFullhealth => health==maxHealth;
    public bool IsFullPower => weaponPower==2;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PowerDown();
        _statsBarHUD.UpdateStats(health,maxHealth);
        TimeController.Instance.BulletTime(slowMotionDuration);
        if (gameObject.activeSelf)
        {
            Move(moveDirection);
            StartCoroutine(InvincibleCoroutine());
            if (regenerateHealth)
            {
                if(healthRegenateCoroutine!=null) StopCoroutine(healthRegenateCoroutine);
                healthRegenateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenateTime,healthRegeratePercent));
            }
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        _statsBarHUD.UpdateStats(health,maxHealth);
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        //1、停止背景的卷动=>修改背景滚动的协程执行条件为游戏状态不为GameOver时
        
        //2、停止敌人开火=>yield break
        _statsBarHUD.UpdateStats(health,maxHealth);
        base.Die();
    }

    IEnumerator InvincibleCoroutine()
    {
        playerCollider.isTrigger = true;
        yield return waitInvincibleTime;
        playerCollider.isTrigger = false;
    }
    
    #endregion

    #region 翻滚
    private void Dodge()
    {
        if(isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //消耗能量
        StartCoroutine(nameof(DodgeCoroutine));
        //make player invincible 使玩家无敌

        //改变玩家的缩放值
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        AudioManager.Instance.PlayRandomSFX(playerDodgeAudioData);
        //无敌，就是使得玩家不与射弹等发生碰撞，可以通过更改图层、修改碰撞体的开关、变碰撞体为触发器等...来实现
        playerCollider.isTrigger = true;
        currentRoll = 0f;
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation =Quaternion.AngleAxis(currentRoll,Vector3.right);
            // if (currentRoll < maxRoll / 2f)
            // {
            //     t1 += Time.deltaTime / dodgeDuration;
            //     transform.localScale = Vector3.Lerp(transform.localScale,dodgeScale,t1);
            // }
            // else
            // {
            //     t2 += Time.deltaTime / dodgeDuration;
            //     transform.localScale = Vector3.Lerp(transform.localScale,dodgeScale,t2);
            // }
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale,currentRoll/maxRoll);
            yield return null;
        }
        
        playerCollider.isTrigger = false;
        isDodging = false;
    }
    #endregion

    #region OverDrive

    void OverDrive()
    {
        //玩家能量非满时，返回
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;
        //若满
        PlayerOverDrive.on.Invoke();//执行委托开启能量爆发
        TimeController.Instance.BulletTime(BulletInDuration,BulletKeepingDuration,BulletOutDuration);
    }
    
    //退出能量爆发状态
    private void OverDriveOff()
    {
        isOverDriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
        
    }
    
    //处于能量爆发状态
    private void OverDriveOn()
    {
        isOverDriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
    }

    #endregion

    public void WeaponPowerUp1Rank()
    {
        weaponPower++;
        weaponPower = Mathf.Clamp(weaponPower ,0 , 2);
    }

    public void PowerDown()
    {
        weaponPower = Mathf.Max(--weaponPower, 0);
        Debug.Log(weaponPower);
    }
}