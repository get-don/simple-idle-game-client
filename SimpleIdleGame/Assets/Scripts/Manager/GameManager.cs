using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public static event Action<int> OnStageChanged;         // 스테이지 변경 알림. <stage>
    public static event Action<int, int> OnEnemyHpChanged;  // 적 HP 변경 알림. <maxHp, currentHp>
    public static event Action<long> OnUpdateGold;          // 골드 변경 알림. <gold>

    public bool IsStageReady { get; private set; }

    [SerializeField] EnemySpawner _enemySpawner;

    private PlayerStat _playerStat; 

    private int _currentStage = 0;
    private long _gold = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        UIController.OnPlayerStatUpgraded += UpgradePlayerStat;
    }

    private void OnDisable()
    {
        UIController.OnPlayerStatUpgraded -= UpgradePlayerStat;
    }

    private void Start()
    {
        StartCoroutine(nameof(Initialize));
    }

    public void StageReady()
    {
        IsStageReady = true;
        OnStageChanged?.Invoke(_currentStage);        
    }

    public bool AttackEnemy()
    {
        var isCritical = _playerStat.IsCritical();
        var damage = _playerStat.GetDamage(isCritical);

        var (enemyMaxHp, enemyCurrentHp) = _enemySpawner.Enmey.TakeDamage(damage, isCritical);
        OnEnemyHpChanged?.Invoke(enemyMaxHp, enemyCurrentHp);

        if (enemyCurrentHp <= 0)
        {
            IsStageReady = false;
            _gold += (long)(_currentStage * 10 * _playerStat.GoldMultiplier);
            OnUpdateGold?.Invoke(_gold);

            NextStage();
        }

        return isCritical;
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1f);

        OnUpdateGold?.Invoke(_gold);

        _playerStat = new();
        _playerStat.CalcStat();

        NextStage();
    }

    private void NextStage()
    {
        _currentStage++;
        _enemySpawner.SpawnEnemy(_currentStage);
    }

    private void UpgradePlayerStat(PlayerStatType statType)
    {
        _playerStat.UpgradeStat(statType);

        Debug.Log("--- Stat ---------------------------------------");
        Debug.Log("AttackPower : " + _playerStat.AttackPower);
        Debug.Log("AttackSpeed : " + _playerStat.AttackSpeedMultiplier);
        Debug.Log("CriticalRate : " + _playerStat.CriticalRate);
        Debug.Log("CriticalPower : " + _playerStat.CriticalPowerMultiplier);
        Debug.Log("GoldMultiplier : " + _playerStat.GoldMultiplier);
        Debug.Log("------------------------------------------------");
    }

}
