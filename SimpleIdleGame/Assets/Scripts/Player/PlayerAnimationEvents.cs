using System;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static event Action OnAttackFinished;

    private void AttackFinished() => OnAttackFinished?.Invoke();        
}
