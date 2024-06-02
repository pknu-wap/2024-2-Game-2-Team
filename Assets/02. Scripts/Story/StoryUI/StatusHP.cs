using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusHP : MonoBehaviour
{

    public Slider slider;

    [Header("현재 HP 텍스트 ")]
    public TMP_Text currentHPText;

    void Update()
    {
        int currentHP = Player.Instance.GetCurrentHP();
        currentHPText.text = currentHP.ToString();
        slider.value = currentHP / Player.Instance.GetMaxHP();
    }

}