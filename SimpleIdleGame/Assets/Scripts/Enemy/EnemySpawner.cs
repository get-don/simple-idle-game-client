using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Enmey { get; private set; }

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemySpawnPosition;
    [SerializeField] private Sprite[] _enemySprites;

    // Test
    private int _testHp = 8;
    private float _testMul = 1.35f;

    public void SpawnEnemy(int stage)
    {
        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.5f);
        if (Enmey == null)
        {
            var obj = Instantiate(_enemyPrefab, _enemySpawnPosition.position, Quaternion.identity, transform);
            Enmey = obj.GetComponent<Enemy>();
        }

        var sprite = _enemySprites[Random.Range(0, _enemySprites.Length)];

        var nextHp = (long)(_testHp * _testMul);
        _testHp = (int)Mathf.Min(nextHp, int.MaxValue);

        Enmey.Initialize(sprite, _testHp);
        
        GameManager.Instance.StageReady();
    }
}
