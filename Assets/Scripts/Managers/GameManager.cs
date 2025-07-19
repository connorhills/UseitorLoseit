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
            total *= (data.clickUpgradeLevel[i] > 0) ? Math.Pow(2, data.clickUpgradeLevel[i]) : 1;
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
        double critChance = 0;
        for (int i = 0; i < data.critChanceUpgradeLevel.Count; i++)
        {
            if (data.critChanceUpgradeLevel[i] > 0)
                critChance += UpgradeManager.instance.critChanceUpgradeBasePow[i];
        }
        return critChance;
    }

    public double TotalCritBonus()
    {
        double critBonus = 0.1;
        for (int i = 0; i < data.critBonusUpgradeLevel.Count; i++)
        {
            if (data.critBonusUpgradeLevel[i] > 0)
                critBonus += UpgradeManager.instance.critBonusUpgradeBasePow[i];
        }
        return critBonus;
    }

    public double TotalClickMultMin()
    {
        double minMultipler = 0.5;
        for (int i = 0; i < data.clickMultMinUpgradeLevel.Count; i++)
        {
            if (data.clickMultMinUpgradeLevel[i] > 0)
            {
                minMultipler += UpgradeManager.instance.clickMultMinUpgradeBasePow[i];
            }
        }
        return minMultipler;
    }

    public double TotalClickMultMax()
    {
        double maxMultipler = 1.5;
        for(int i = 0; i < data.clickMultMaxUpgradeLevel.Count; i++)
        {
            if(data.clickMultMaxUpgradeLevel[i] > 0)
            {
                maxMultipler += UpgradeManager.instance.clickMultMaxUpgradeBasePow[i];
            }
        }
        return maxMultipler;
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
        double baseClick = BaseChipsPerClick();

        double minMultipler = 0.5;
        double maxMultipler = 1.5;

        for (int i = 0; i < data.clickMultMinUpgradeLevel.Count; i++)
        {
            if(data.clickMultMinUpgradeLevel[i] > 0)
            {
                minMultipler += UpgradeManager.instance.clickMultMinUpgradeBasePow[i];
            }
        }

        for (int i = 0; i < data.clickMultMaxUpgradeLevel.Count; i++)
        {
            if(data.clickMultMaxUpgradeLevel[i] > 0)
            {
                maxMultipler += UpgradeManager.instance.clickMultMaxUpgradeBasePow[i];
            }
        }

        double randomMultiplier = UnityEngine.Random.Range((float)minMultipler, (float)maxMultipler);
        double clickValue = baseClick * randomMultiplier;

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
            Debug.Log($"Pre-crit click value: {clickValue}");
            clickValue *= totalCritBonus;
            if (clickValue < BaseChipsPerClick() && randomMultiplier < 1)
            {
                Debug.Log($"MINI WIN! {clickValue} Crit Bonus: {totalCritBonus} Random Multiplier: {randomMultiplier}");
            }
            else if (clickValue > (BaseChipsPerClick() * 1.45) && randomMultiplier > 1.45)
            {
                Debug.Log($"JACKPOT WIN! {clickValue} Crit Bonus: {totalCritBonus} Random Multiplier: {randomMultiplier}");
            }
            else if (clickValue > (BaseChipsPerClick() * 1.3) && randomMultiplier > 1.3)
            {
                Debug.Log($"MEGA WIN! {clickValue} Crit Bonus: {totalCritBonus} Random Multiplier: {randomMultiplier}");
            }
            else
            {
                Debug.Log($"MINOR WIN! {clickValue} Crit Bonus: {totalCritBonus} Random Multiplier: {randomMultiplier}");
            }
        }

        data.chips += clickValue;
        singleChip.PlayOneShot(singleChip.clip);
    }
}
