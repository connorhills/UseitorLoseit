using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public void Awake() => instance = this;

    [Header("Click Upgrade Data")]
    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradePrefab;

    public ScrollRect clickUpgradeScroll;
    public Transform clickUpgradePanel;

    public string[] clickUpgradeNames;

    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeCostMult;
    public double[] clickUpgradeBasePow;

    [Header("Auto Gen Upgrade Data")]
    public List<Upgrades> autoGenUpgrades;
    public Upgrades autoGenUpgradePrefab;

    public ScrollRect autoGenUpgradeScroll;
    public Transform autoGenUpgradePanel;

    public string[] autoGenUpgradeNames;

    public double[] autoGenUpgradeBaseCost;
    public double[] autoGenUpgradeCostMult;
    public double[] autoGenUpgradeBasePow;

    public void StartUpgradeManager()
    {
        MethodManager.UpgradeCheck(GameManager.instance.data.clickUpgradeLevel, 5);
        clickUpgradeNames    = new[] { "Chips Per Click +1", 
                                       "Chips Per Click +5", 
                                       "Chips Per Click +20", 
                                       "Chips Per Click +50", 
                                       "Chips Per Click +100" 
                                     };
        autoGenUpgradeNames  = new[] {
                                        "+1 Chips/s",
                                        "+5 Chips/s",
                                        "+10 Chips/s",
                                        "+25 Chips/s",
                                        "+50 Chips/s",
                                     };

        clickUpgradeBaseCost = new double[] { 50, 100, 500, 2500, 10000 };
        clickUpgradeCostMult = new double[] { 1.9, 1.8, 1.75, 1.6, 1.55 };
        clickUpgradeBasePow  = new double[] { 1, 5, 20, 50, 100 };

        autoGenUpgradeBaseCost = new double[] { 25, 100, 500, 1000, 10000 };
        autoGenUpgradeCostMult = new double[] { 2.25, 2, 1.8, 1.7, 1.6 };
        autoGenUpgradeBasePow  = new double[] { 1, 5, 10, 25, 50 };

        for (int i = 0; i < GameManager.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradePanel);
            upgrade.upgradeID = i;
            clickUpgrades.Add(upgrade);
        }

        for (int i = 0; i < GameManager.instance.data.autoGenUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(autoGenUpgradePrefab, autoGenUpgradePanel);
            upgrade.upgradeID = i;
            autoGenUpgrades.Add(upgrade);
        }
        clickUpgradeScroll.normalizedPosition = new Vector2(0, 0);
        autoGenUpgradeScroll.normalizedPosition = new Vector2(0, 0);
        UpdateUpgradeUI("click");
        UpdateUpgradeUI("autoGen");
    }

    public void UpdateUpgradeUI(string type, int upgradeID = -1)
    {
        var data = GameManager.instance.data;

        switch (type)
        {
            case "click":
                if (upgradeID == -1)
                {
                    for (int i = 0; i < clickUpgrades.Count; i++)
                    {
                        UpdateUI(clickUpgrades, data.clickUpgradeLevel, clickUpgradeNames, i);
                    }
                }
                else
                {
                    UpdateUI(clickUpgrades, data.clickUpgradeLevel, clickUpgradeNames, upgradeID);
                }
                break;

            case "autoGen":
                if (upgradeID == -1)
                {
                    for (int i = 0; i < autoGenUpgrades.Count; i++)
                    {
                        UpdateUI(autoGenUpgrades, data.autoGenUpgradeLevel, autoGenUpgradeNames, i);
                    }
                }
                else
                {
                    UpdateUI(autoGenUpgrades, data.autoGenUpgradeLevel, autoGenUpgradeNames, upgradeID);
                }
                break;
        }
        void UpdateUI(List<Upgrades> upgrade, List<int> upgradeLevels, string[] upgradeNames, int iD)
        {
            upgrade[iD].levelText.text = upgradeLevels[iD].ToString();
            upgrade[iD].costText.text = $"Cost: {UpgradeCost(type, iD):F2} Chips";
            upgrade[iD].nameText.text = upgradeNames[iD];
        }
    }

    public double UpgradeCost(string type, int upgradeID)
    {
        var data = GameManager.instance.data;
        switch (type)
        {
            case "click":
                return clickUpgradeBaseCost[upgradeID]
                * Math.Pow(clickUpgradeCostMult[upgradeID], (double)data.clickUpgradeLevel[upgradeID]);
            case "autoGen":
                return autoGenUpgradeBaseCost[upgradeID]
                * Math.Pow(autoGenUpgradeCostMult[upgradeID], (double)data.autoGenUpgradeLevel[upgradeID]);

        }

        return 0;
        
    }


    public void BuyUpgrade(string type, int upgradeID)
    {
        var data = GameManager.instance.data;

        switch (type)
        {
            case "click": Buy(data.clickUpgradeLevel);
                break;
            case "autoGen": Buy(data.autoGenUpgradeLevel);
                break;
        }

        void Buy(List<int> upgradeLevels)
        {
            if (data.chips >= UpgradeCost(type, upgradeID))
            {
                data.chips -= UpgradeCost(type, upgradeID);
                upgradeLevels[upgradeID] += 1;
            }

            UpdateUpgradeUI(type, upgradeID);
        }
    }
}
