using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    public static event Action<PlayerStatType> OnPlayerStatUpgraded;

    [SerializeField] private Slider _hpBar;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _stageText;
    [SerializeField] private TMP_Text _goldText;

    [SerializeField] private Button _attackPowerUpgradeButton;
    [SerializeField] private Button _attackSpeedUpgradeButton;
    [SerializeField] private Button _criticalRateUpgradeButton;
    [SerializeField] private Button _criticalPowerUpgradeButton;
    [SerializeField] private Button _goldMultiplierUpgradeButton;

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
        GameManager.OnEnemyHpChanged += ChangeEnemyHpBar;
        GameManager.OnStageChanged += ChageStage;
        GameManager.OnUpdateGold += UpdateGold;

        _attackPowerUpgradeButton.onClick.AddListener(UpgradeAttak);
        _attackSpeedUpgradeButton.onClick.AddListener(UpgradeAttackSpeed);
        _criticalRateUpgradeButton.onClick.AddListener(UpgradeCriticalRate);
        _criticalPowerUpgradeButton.onClick.AddListener(UpgradeCriticalPower);
        _goldMultiplierUpgradeButton.onClick.AddListener(UpgradeGoldMultiplier);
    }

    private void OnDisable()
    {
        GameManager.OnEnemyHpChanged += ChangeEnemyHpBar;
        GameManager.OnStageChanged -= ChageStage;
        GameManager.OnUpdateGold -= UpdateGold;

        _attackPowerUpgradeButton.onClick.RemoveListener(UpgradeAttak);
        _attackSpeedUpgradeButton.onClick.RemoveListener(UpgradeAttackSpeed);
        _criticalRateUpgradeButton.onClick.RemoveListener(UpgradeCriticalRate);
        _criticalPowerUpgradeButton.onClick.RemoveListener(UpgradeCriticalPower);
        _goldMultiplierUpgradeButton.onClick.RemoveListener(UpgradeGoldMultiplier);
    }

    private void UpgradeAttak() => OnPlayerStatUpgraded?.Invoke(PlayerStatType.AttackPower);
    private void UpgradeAttackSpeed() => OnPlayerStatUpgraded?.Invoke(PlayerStatType.AttackSpeed);
    private void UpgradeCriticalRate() => OnPlayerStatUpgraded?.Invoke(PlayerStatType.CriticalRate);
    private void UpgradeCriticalPower() => OnPlayerStatUpgraded?.Invoke(PlayerStatType.CriticalPower);
    private void UpgradeGoldMultiplier() => OnPlayerStatUpgraded?.Invoke(PlayerStatType.GoldMultiplier);

    private void ChangeEnemyHpBar(int maxHp, int currentHp)
    {
        _hpBar.value = (float)currentHp / maxHp;
        _hpText.text = $"{currentHp} / {maxHp}";
    }

    private void ChageStage(int stage)
    {
        _stageText.text = $"Stage : {stage}";
    }

    private void UpdateGold(long amount)
    {
        _goldText.text = $"Gold : {amount}";
    }
}
