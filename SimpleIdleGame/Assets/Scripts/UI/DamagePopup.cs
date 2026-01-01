using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _alphaSpeed = 1f;
    [SerializeField] private float _destroyTime = 1f;

    [SerializeField] private Color _damageColor;
    [SerializeField] private Color _criticalColor;

    private TextMeshPro _text;
    private Color _textAlpha;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        Invoke(nameof(DestroyObject), _destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        _textAlpha.a = Mathf.Lerp(_textAlpha.a, 0, Time.deltaTime * _alphaSpeed);
        _text.color = _textAlpha;
    }

    private void DestroyObject() => Destroy(gameObject);

    public void Initialize(int damage, bool isCritical = false)
    {
        _text.text = damage.ToString();
        if(isCritical)
        {
            _text.color = _criticalColor;
            _text.fontSize = 7;
        }
        else
        {
            _text.color = _damageColor;
        }

        _textAlpha = _text.color;
    }
}
