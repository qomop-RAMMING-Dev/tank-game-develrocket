using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUIController : MonoBehaviour
{
    [Header("HP UI References")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    public void UpdateHP (float currentHp, float maxHp)
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;

        hpText.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
    }
}
