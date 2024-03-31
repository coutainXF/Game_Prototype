using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine_System.Base;
using State_Machine_System.Player_States;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VoidEventChannel victoryEventChannel;
    
    private PlayerInput input;
    private Rigidbody playerRigid;
    private PlayerGroundedDetective groundDetective;
    public AudioSource audioSource;
    public bool canAirJump { get; set; }
    public float MoveSpeed => Mathf.Abs(playerRigid.velocity.x);
    public bool isGrounded => groundDetective.isGround;
    public bool isFalling => playerRigid.velocity.y < 0 && !isGrounded ;//当不在地面上且玩家的y轴速度小于零时，玩家处于下落状态
    public bool Victory { get; private set; }
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody>();
        groundDetective = GetComponentInChildren<PlayerGroundedDetective>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        victoryEventChannel.AddListener(OnVictory);
    }

    private void OnDisable()
    {
        victoryEventChannel.RemoveListener(OnVictory);
    }

    private void OnVictory()
    {
        Victory = true;
        input.DisableGameplayInputs();
    }
    
    private void Start()
    {
        input.EnableGameplayInputs();
    }

    public void Move(float speed)
    {
        if (input.Move)
        {
            transform.localScale = new Vector3(input.AxisX, 1f, 1f);
        }
        SetVelocityX(speed * input.AxisX);
    }
    
    public void SetVelocity(Vector3 velocity)
    {
        playerRigid.velocity = velocity;
    }
    
    public void SetVelocityX(float velocityX)
    {
        playerRigid.velocity = new Vector3(velocityX, playerRigid.velocity.y);
    }
    
    public void SetVelocityY(float velocityY)
    {
        playerRigid.velocity = new Vector3(playerRigid.velocity.x, velocityY );
    }
    
    
    public void SetUseGravity(bool value)
    {
        playerRigid.useGravity = value;
    }

    public void OnDefeated()
    {
        input.DisableGameplayInputs();
        
        playerRigid.velocity = Vector3.zero;
        playerRigid.useGravity = false;
        playerRigid.detectCollisions = false;
        
        GetComponent<StateMachine>().SwitchState(typeof(PlayerState_Defeated));
    }
}
