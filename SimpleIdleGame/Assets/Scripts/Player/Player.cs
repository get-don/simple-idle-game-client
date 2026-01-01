using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip _attackAudioClip;
    [SerializeField] private AudioClip _criticalAudioClip;

    private AudioSource _audio;
    private Animator _anim;

    private int _animHashAttack;
    private int _animHashAttackSpeedMul;

    private bool _canAttack = true;


    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponentInChildren<Animator>();

        _animHashAttack = Animator.StringToHash("attack");
        _animHashAttackSpeedMul = Animator.StringToHash("attackSpeedMul");
    }

    private void OnEnable()
    {        
        PlayerStat.OnAttackSpeedChanged += UpdateAttackAnimSpeed;
        PlayerAnimationEvents.OnAttackFinished += HandleAttackFinished;
    }

    private void OnDisable()
    {
        PlayerStat.OnAttackSpeedChanged -= UpdateAttackAnimSpeed;
        PlayerAnimationEvents.OnAttackFinished -= HandleAttackFinished;
    }

    private void Update()
    {
        TryToAttack();
    }

    private void TryToAttack()
    {
        if (!GameManager.Instance.IsStageReady || !_canAttack) return;

        _canAttack = false;
        _anim.SetTrigger(_animHashAttack);
    }

    private void HandleAttackFinished()
    {
        _audio.clip = GameManager.Instance.AttackEnemy() ? _criticalAudioClip : _attackAudioClip;
        _audio.Play();
        _canAttack = true;
    }

    private void UpdateAttackAnimSpeed(float attackSpeedMultiplier) => _anim.SetFloat(_animHashAttackSpeedMul, attackSpeedMultiplier);
}
