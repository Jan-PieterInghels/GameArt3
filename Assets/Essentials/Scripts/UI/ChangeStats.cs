using UnityEngine;
using UnityEngine.UI;

public class ChangeStats : MonoBehaviour
{
    [SerializeField] Text _healthText, _defenceText, _normalAttText, _heavyAttText, _speedText;

    private float _healthstat;
    [SerializeField] private Slider _healthSlider;
    public Slider HealthSlider { get => _healthSlider; set { _healthSlider = value; } }
    public float HealthStats { get => _healthstat; set { CheckAmount(value, _healthText, _healthSlider); } }

    private float _defenceStat;
    [SerializeField] private Slider _defenceSlider;
    public Slider DefenceSlider { get => _defenceSlider; set { _defenceSlider = value; } }
    public float DefenceStats { get => _defenceStat; set { CheckAmount(value, _defenceText, _defenceSlider); } }

    private float _normalAttStat;
    [SerializeField] private Slider _normalAttSlider;
    public Slider NormalSlider { get => _normalAttSlider; set { _normalAttSlider = value; } }
    public float NormalAttackStats { get => _normalAttStat; set { CheckAmount(value, _normalAttText, _normalAttSlider); } }

    private float _heavyAttStat;
    [SerializeField] private Slider _heavyAttSlider;
    public Slider HeavySlider { get => _heavyAttSlider; set { _heavyAttSlider = value; } }
    public float HeavyAttackStats { get => _heavyAttStat; set { CheckAmount(value, _heavyAttText, _heavyAttSlider); } }

    private float _speedStat;
    [SerializeField] private Slider _speedSlider;
    public Slider SpeedSlider { get => _speedSlider; set { _speedSlider = value; } }
    public float SpeedStats { get => _speedStat; set { CheckAmount(value, _speedText, _speedSlider); } }
    
    private void CheckAmount(float value, Text text, Slider checkValues)
    {
        checkValues.value = value;
    }

    public void ResetTexts()
    {
        _healthText.text = "";
        _defenceText.text = "";
        _normalAttText.text = "";
        _heavyAttText.text = "";
        _speedText.text = "";
    }
}
