using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float jumpInputBufferTime = 0.5f;

    private WaitForSeconds _waitJumpInputBufferTime;
    
    private PlayerInputAction _playerInputAction;
    private Vector2 axes => _playerInputAction.GamePlay.Axes.ReadValue<Vector2>();
    
    public bool HasJumpInputBuffer { get; set; }   //是否存在跳跃输入缓冲
    public bool Jump => _playerInputAction.GamePlay.Jump.WasPressedThisFrame();         //是否在这一帧按下了跳跃键
    public bool StopJump => _playerInputAction.GamePlay.Jump.WasReleasedThisFrame();    //是否在这一帧松开了跳跃键
    public bool Move => AxisX != 0f;        //水平输入信号
    public float AxisX => axes.x;
    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        _waitJumpInputBufferTime = new WaitForSeconds(jumpInputBufferTime);
    }

    private void OnEnable()
    {
        _playerInputAction.GamePlay.Jump.canceled += delegate
        {
            HasJumpInputBuffer = false;
        };//订阅跳跃按钮松开事件，松开跳跃按钮的同时，将跳跃输入缓冲关闭。
    }

    // private void OnGUI()
    // {
    //     Rect rect = new Rect(200, 200, 200, 200);//创建矩形
    //     string message = "has jump input buffer"+HasJumpInputBuffer;//字符串（输出到GUI中）
    //     GUIStyle style = new GUIStyle();//创建GUI样式
    //     style.fontSize = 20;
    //     style.fontStyle = FontStyle.Bold;
    //     
    //     GUI.Label(rect,message,style);//将信息输出到GUI中
    // }

    public void EnableGameplayInputs()
    {
        _playerInputAction.GamePlay.Enable();//启用玩家输入
        Cursor.lockState = CursorLockMode.Locked;//锁定光标
    }
    
    public void DisableGameplayInputs()
    {
        //禁用玩家输入
        _playerInputAction.GamePlay.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetJumpInputBufferTimer()
    {
        StopCoroutine(nameof(JumpInputBufferCoroutine));//先关闭协程，防止重复开启
        StartCoroutine(nameof(JumpInputBufferCoroutine));//再开启协程
    }
    
    IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;

        yield return _waitJumpInputBufferTime;

        HasJumpInputBuffer = false;
    }
}
