using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text critChanceText;
    [SerializeField] private TMP_Text critBonusText;
    [SerializeField] private TMP_Text clickMultiplierRangeText;

    private void Update()
    {
        if (GameManager.instance != null)
        {
            UpdateStatsDisplay();
        }
    }

    private void UpdateStatsDisplay()
    {
        if (critChanceText != null)
            critChanceText.text = $"Crit Chance: {GameManager.instance.TotalCritChance() * 100:F2}%";

        if (critBonusText != null)
            critBonusText.text = $"Crit Bonus: +{GameManager.instance.TotalCritBonus() * 100:F1}%";

        if (clickMultiplierRangeText != null)
            clickMultiplierRangeText.text = $"Click Multiplier: {GameManager.instance.TotalClickMultMin():F1}x - {GameManager.instance.TotalClickMultMax():F1}x";
    }
}
