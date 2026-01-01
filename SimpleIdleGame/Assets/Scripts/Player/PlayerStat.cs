using System;
using System.Diagnostics;

public class PlayerStat
{
    public static event Action<float> OnAttackSpeedChanged;

    // 공격력
    public int AttackPower { get; private set; }
    public int AttackPowerLevel { get; private set; } = 1;

    // 공격 속도
    private float _attackSpeedMultiplier;
    public float AttackSpeedMultiplier
    {
        get => _attackSpeedMultiplier;
        private set
        {
            _attackSpeedMultiplier = value;
            OnAttackSpeedChanged?.Invoke(_attackSpeedMultiplier);
        }
    }
    public int AttackSpeedLevel { get; private set; } = 1;

    // 크리티컬 확률
    public float CriticalRate { get; private set; }
    public int CriticalRateLevel { get; private set; } = 1;

    // 크리티컬 데미지 배율
    public float CriticalPowerMultiplier { get; private set; }
    public int CriticalPowerLevel { get; private set; } = 1;

    // 골드 획득 배율
    public float GoldMultiplier { get; private set; }
    public int GoldMultiplierLevel { get; private set; } = 1;


    // 일단 간단하게 계산함.
    // 시작값은 추후 변경.
    public void CalcStat()
    {
        AttackPower = 2 + (AttackPowerLevel - 1) * AttackPowerLevel / 2;
        AttackSpeedMultiplier = 1f + ((AttackSpeedLevel - 1) * 0.015f);
        CriticalRate = 0.01f + ((CriticalRateLevel - 1) * 0.005f);
        CriticalPowerMultiplier = 1.2f + ((CriticalPowerLevel - 1) * 0.01f);
        GoldMultiplier = 1f + ((GoldMultiplierLevel - 1) * 0.5f);
    }

    public int GetDamage(bool isCritical) => isCritical ? (int)(AttackPower * CriticalPowerMultiplier) : AttackPower;

    public bool IsCritical() => UnityEngine.Random.value < CriticalRate;

    // TODO: 레벨 제한 해야함.
    public void UpgradeStat(PlayerStatType stat)
    {
        switch (stat)
        {
            case PlayerStatType.AttackPower:
                AttackPowerLevel++;
                break;

            case PlayerStatType.AttackSpeed:
                AttackSpeedLevel++;
                break;

            case PlayerStatType.CriticalRate:
                CriticalRateLevel++;
                break;

            case PlayerStatType.CriticalPower:
                CriticalPowerLevel++;
                break;
            case PlayerStatType.GoldMultiplier:
                GoldMultiplierLevel++;
                break;
        }

        CalcStat();
    }
}
