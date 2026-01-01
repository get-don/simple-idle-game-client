using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private float _hitMotionMoveX = 0.2f;

    private SpriteRenderer _renderer;

    private Vector3 _originPosition;
    private int _maxHp;
    private int _hp;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _originPosition = transform.position;        
    }
    
    public void Initialize(Sprite sprite, int maxHp)
    {
        transform.position = _originPosition;
        _renderer.sprite = sprite;
        _maxHp = maxHp;
        _hp = maxHp;        
    }

    private void PlayHitEffect(int damage, bool isCritical = false)
    {
        StopCoroutine(nameof(PlayHitMotion));
        StartCoroutine(nameof(PlayHitMotion));

        var damageTextObj = Instantiate(_damageTextPrefab, transform.position, Quaternion.identity, transform);
        var damageText = damageTextObj.GetComponent<DamagePopup>();
        damageText.Initialize(damage, isCritical);
    }

    private IEnumerator PlayHitMotion()
    {
        transform.position = new Vector3(_originPosition.x + _hitMotionMoveX, _originPosition.y, _originPosition.z);
        yield return new WaitForSeconds(0.1f);
        transform.position = _originPosition;
    }
    public (int, int) TakeDamage(int damage, bool isCritical = false)
    {
        if (_hp <= 0) return (_maxHp, _hp);

        PlayHitEffect(damage, isCritical);

        _hp -= damage;
        if (_hp <= 0)
        {
            _hp = 0;
        }

        return (_maxHp, _hp);
    }

}
