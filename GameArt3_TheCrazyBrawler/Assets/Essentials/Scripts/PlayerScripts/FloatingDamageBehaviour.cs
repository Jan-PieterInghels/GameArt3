using UnityEngine;

public class FloatingDamageBehaviour : MonoBehaviour
{
    [SerializeField] private Color[] _damageColor;
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private float _destroyTime = 1f;
    public float DamageAmount { get; set; }

    // Start is called before the first frame update
    public void Initialize ()
    {
        _textMesh.text = DamageAmount.ToString();
        if (DamageAmount < 25) _textMesh.color = _damageColor[0];
        else if (DamageAmount > 50) _textMesh.color = _damageColor[2];
        else _textMesh.color = _damageColor[1];

        Destroy(gameObject, _destroyTime);
    }
}
