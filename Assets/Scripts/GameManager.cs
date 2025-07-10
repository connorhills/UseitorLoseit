using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() => instance = this;

    public Data data;

    [SerializeField] private TMP_Text totalChipsText;
    [SerializeField] private TMP_Text chipsPerSecond;
    [SerializeField] private TMP_Text chipsPerClickText;

    [SerializeField] AudioSource singleChip;

    [SerializeField] private RectTransform chipButton;

    public double BaseChipsPerClick()
    {
        double total = 1;
        for (int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            total *= (data.clickUpgradeLevel[i] > 0) ? Math.Pow(10, data.clickUpgradeLevel[i]) : 1;
        }
        return total;
    }

    public double ChipsPerClick()
    {
        double total = BaseChipsPerClick();
        
        return total;
    }

    public double TotalCritChance()
    {
        double total = 0;
        for (int i = 0; i < data.critChanceUpgradeLevel.Count; i++)
        {
            if (data.critChanceUpgradeLevel[i] > 0)
                total += UpgradeManager.instance.critChanceUpgradeBasePow[i];
        }
        return total;
    }

    public double ChipsPerSecond()
    {
        double total = 0;
        for (int i = 0; i < data.autoGenUpgradeLevel.Count; i++)
        {
            total += UpgradeManager.instance.autoGenUpgradeBasePow[i] * data.autoGenUpgradeLevel[i];
        }

        return total;
    }

    private void Start()
    {
        data = new Data();

        UpgradeManager.instance.StartUpgradeManager();
    }

    private void Update()
    {
        totalChipsText.text    = $"{data.chips:F2}";
        chipsPerSecond.text    = $"{ChipsPerSecond():F2}/s";
        chipsPerClickText.text = "$" + BaseChipsPerClick();

        data.chips += ChipsPerSecond() * Time.deltaTime;
    }

    public void increase_chips()
    {
        double baseCLick = BaseChipsPerClick();
        float randomMultiplier = UnityEngine.Random.Range(0.5f, 1.5f);
        double clickValue = baseCLick * randomMultiplier;

        if (UnityEngine.Random.value < TotalCritChance())
        {
            double critBonus = 0.1;
            for (int i = 0; i < data.critBonusUpgradeLevel.Count; i++)
            {
                if (data.critBonusUpgradeLevel[i] > 0)
                {
                    critBonus += UpgradeManager.instance.critBonusUpgradeBasePow[i];
                }
            }
            double totalCritBonus = 1 + critBonus;
            clickValue *= totalCritBonus;
            Debug.Log($"hit {clickValue} multiplier {totalCritBonus}");
        }

        data.chips += clickValue;
        singleChip.PlayOneShot(singleChip.clip);
    }
}
