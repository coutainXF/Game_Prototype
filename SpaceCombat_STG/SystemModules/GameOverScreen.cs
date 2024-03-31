using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] Canvas _canvasHUD;
    [SerializeField] AudioData confirmGameOverSound;
    
    Canvas _canvas;
    Animator _animator;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");
    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _animator = GetComponent<Animator>();

        _canvas.enabled = false;
        _animator.enabled = false;
    }

    void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        _input.onConfirmGameOver += OnConfirmGameOver;
    }

    void OnConfirmGameOver()
    {
        //1、确认结束游戏的音效
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        //2、禁用输入
        _input.DisableAllInputs();
        //3、播放动画效果
        _animator.Play(exitStateID);
        //加载积分面板
        SceneLoader.Instance.LoadScoringScene();
    }

    void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        _input.onConfirmGameOver -= OnConfirmGameOver;
    }
    
    void OnGameOver()
    {
        _canvasHUD.enabled = false;
        _canvas.enabled = true;
        _animator.enabled = true;
        _input.DisableAllInputs();
    }

    //将由动画事件调用
    public void EnableGameOverScreenInput()
    {
        _input.EnableGameOverScreenInput();
    }
}
