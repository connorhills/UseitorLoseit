using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public void Awake() => instance = this;

    [Header("Upgrade UI")]
    public ScrollRect upgradeScroll;
    public Transform upgradePanel;

    public ScrollRect autoGenUpgradeScroll;
    public Transform autoGenUpgradePanel;

    [Header("Click Upgrade Data")]
    public List<Upgrades> clickUpgrades;
    public Upgrades[] clickUpgradePrefabs;

    public bool[] clickUpgradeIsUnlocked;

    public string[] clickUpgradeNames;

    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeBasePow;
    public double[] clickUpgradeUnlockVal;

    [Header("Crit Chance Upgrade Data")]
    public List<Upgrades> critChanceUpgrades;
    public Upgrades[] critChanceUpgradePrefabs;

    public bool[] critChanceUpgradeIsUnlocked;

    public string[] critChanceUpgradeNames;

    public double[] critChanceUpgradeBaseCost;
    public double[] critChanceUpgradeBasePow;
    public double[] critChanceUpgradeUnlockVal;

    [Header("Crit Bonus Upgrade Data")]
    public List<Upgrades> critBonusUpgrades;
    public Upgrades[] critBonusUpgradePrefabs;

    public bool[] critBonusUpgradeIsUnlocked;

    public string[] critBonusUpgradeNames;

    public double[] critBonusUpgradeBaseCost;
    public double[] critBonusUpgradeBasePow;
    public double[] critBonusUpgradeUnlockVal;

    [Header("Auto Gen Upgrade Data")]
    public List<Upgrades> autoGenUpgrades;
    public Upgrades[] autoGenUpgradePrefab;

    public string[] autoGenUpgradeNames;

    public double[] autoGenUpgradeBaseCost;
    public double[] autoGenUpgradeCostMult;
    public double[] autoGenUpgradeBasePow;
    public double[] autoGenUpgradesUnlocked;

    public void StartUpgradeManager()
    {
        MethodManager.UpgradeCheck(GameManager.instance.data.clickUpgradeLevel, 8);
        MethodManager.UpgradeCheck(GameManager.instance.data.critChanceUpgradeLevel, 9);
        MethodManager.UpgradeCheck(GameManager.instance.data.critBonusUpgradeLevel, 10);

        clickUpgradeBasePow = new double[GameManager.instance.data.clickUpgradeLevel.Count];
        clickUpgradeBaseCost = new double[] { 100, 1000, 25000, 50000, 100000, 250000, 500000, 1000000 };
        clickUpgradeUnlockVal = new double[] { 0, 100, 1000, 25000, 50000, 100000, 250000, 500000 };
        clickUpgradeIsUnlocked = new bool[clickUpgradeBaseCost.Length];

        double basePow = GameManager.instance.ChipsPerClick() * 2;
        for (int i = 0; i < clickUpgradeBasePow.Length; i++)
        {
            clickUpgradeBasePow[i] = basePow;
        }

        clickUpgradeNames = new string[clickUpgradeBasePow.Length];
        for (int i = 0; i < clickUpgradeBasePow.Length; i++)
        {
            clickUpgradeNames[i] = $"x{clickUpgradeBasePow[i]}";
        }

        critChanceUpgradeBasePow = new double[] { 0.01, 0.05, 0.075, 0.1, 0.222, 0.5, 0.777, 0.9, 1 };
        critChanceUpgradeBaseCost = new double[] { 250, 1250, 13000, 30000, 45000, 112500, 285000, 575000, 1150000 };
        critChanceUpgradeUnlockVal = new double[] { 0, 1250, 13000, 30000, 45000, 112500, 285000, 575000, 1150000 };
        critChanceUpgradeIsUnlocked = new bool[critChanceUpgradeBaseCost.Length];

        critChanceUpgradeNames = new string[critChanceUpgradeBasePow.Length];
        for (int i = 0; i < critChanceUpgradeBasePow.Length; i++)
        {
            critChanceUpgradeNames[i] = $"+{critChanceUpgradeBasePow[i] * 100}%";
        }

        critBonusUpgradeBasePow = new double[] { 0.001, 0.005, 0.01, 0.02, 0.04, 0.05, 0.1, 0.2, 0.4, 0.777 };
        critBonusUpgradeBaseCost = new double[] { 500, 1500, 15000, 35000, 60000, 125000, 325000, 650000, 1350000, 2000000 };
        critBonusUpgradeUnlockVal = new double[] { 0, 1500, 15000, 35000, 60000, 125000, 325000, 650000, 135000, 2000000 };
        critBonusUpgradeIsUnlocked = new bool[critBonusUpgradeBaseCost.Length];

        critBonusUpgradeNames = new string[critBonusUpgradeBasePow.Length];
        for (int i = 0; i < critBonusUpgradeBasePow.Length; i++)
        {
            critBonusUpgradeNames[i] = $"+{critBonusUpgradeBasePow[i] * 100}%";
        }

        autoGenUpgradeBasePow   = new double[] { 1, 5, 10, 20, 35, 50, 100, 250, 400, 1000, 5000, 10000};
        autoGenUpgradeBaseCost  = new double[] { 50, 100, 750, 1500, 5000, 12500, 23500, 55000, 115000, 225000, 545000, 1000000 };
        autoGenUpgradeCostMult  = new double[] { 1.4, 1.38, 1.45, 1.43, 1.41, 1.4, 1.39, 1.38, 1.37, 1.36, 1.35, 1.35 };
        autoGenUpgradesUnlocked = new double[] { 0, 50, 100, 750, 1500, 5000, 12500, 23500, 55000, 115000, 225000, 545000 };

        autoGenUpgradeNames = new string[autoGenUpgradeBasePow.Length];
        for (int i = 0; i < autoGenUpgradeBasePow.Length; i++)
        {
            autoGenUpgradeNames[i] = $"+{autoGenUpgradeBasePow[i]} Chips/s";
        }

        for (int i = 0; i < GameManager.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefabs[i], upgradePanel);
            upgrade.upgradeID = i;
            upgrade.gameObject.SetActive(false);
            clickUpgrades.Add(upgrade);
        }

        for (int i = 0; i < GameManager.instance.data.critChanceUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(critChanceUpgradePrefabs[i], upgradePanel);
            upgrade.upgradeID = i;
            upgrade.gameObject.SetActive(false);
            critChanceUpgrades.Add(upgrade);
        }

        for (int i = 0; i < GameManager.instance.data.critBonusUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(critBonusUpgradePrefabs[i], upgradePanel);
            upgrade.upgradeID = i;
            upgrade.gameObject.SetActive(false);
            critBonusUpgrades.Add(upgrade);
        }

        for (int i = 0; i < GameManager.instance.data.autoGenUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(autoGenUpgradePrefab[i], autoGenUpgradePanel);
            upgrade.upgradeID = i;
            upgrade.gameObject.SetActive(false);
            autoGenUpgrades.Add(upgrade);
        }

        upgradeScroll.normalizedPosition = new Vector2(0, 0);
        autoGenUpgradeScroll.normalizedPosition = new Vector2(0, 0);
        UpdateUpgradeUI("click");
        UpdateUpgradeUI("critChance");
        UpdateUpgradeUI("critBonus");
        UpdateUpgradeUI("autoGen");
    }

    public void Update()
    {
        for (var i = 0; i < clickUpgrades.Count; i++)
        {
            if(!clickUpgradeIsUnlocked[i] && GameManager.instance.data.chips >= clickUpgradeUnlockVal[i])
            {
                clickUpgradeIsUnlocked[i] = true;
            }

            bool isUnlocked = clickUpgradeIsUnlocked[i] && GameManager.instance.data.clickUpgradeLevel[i] == 0;
            clickUpgrades[i].gameObject.SetActive(isUnlocked);
        }

        for (var i = 0; i < critChanceUpgrades.Count; i++)
        {
            if (!critChanceUpgradeIsUnlocked[i] && GameManager.instance.data.chips >= critChanceUpgradeUnlockVal[i])
            {
                critChanceUpgradeIsUnlocked[i] = true;
            }

            bool isUnlocked = critChanceUpgradeIsUnlocked[i] && GameManager.instance.data.critChanceUpgradeLevel[i] == 0;
            critChanceUpgrades[i].gameObject.SetActive(isUnlocked);
        }

        for (var i = 0; i < critBonusUpgrades.Count; i++)
        {
            if (!critBonusUpgradeIsUnlocked[i] && GameManager.instance.data.chips >= critBonusUpgradeUnlockVal[i])
            {
                critBonusUpgradeIsUnlocked[i] = true;
            }

            bool isUnlocked = critBonusUpgradeIsUnlocked[i] && GameManager.instance.data.critBonusUpgradeLevel[i] == 0;
            critBonusUpgrades[i].gameObject.SetActive(isUnlocked);
        }

        for (var i = 0; i < autoGenUpgrades.Count; i++)
        {
            if (!autoGenUpgrades[i].gameObject.activeSelf)
                autoGenUpgrades[i].gameObject.SetActive(GameManager.instance.data.chips >= autoGenUpgradesUnlocked[i]);
        }
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

            case "critChance":
                if (upgradeID == -1)
                {
                    for (int i = 0; i < critChanceUpgrades.Count; i++)
                    {
                        UpdateUI(critChanceUpgrades, data.critChanceUpgradeLevel, critChanceUpgradeNames, i);
                    }
                }
                else
                {
                    UpdateUI(critChanceUpgrades, data.critChanceUpgradeLevel, critChanceUpgradeNames, upgradeID);
                }
                break;

            case "critBonus":
                if (upgradeID == -1)
                {
                    for (int i = 0; i < critBonusUpgrades.Count; i++)
                    {
                        UpdateUI(critBonusUpgrades, data.critBonusUpgradeLevel, critBonusUpgradeNames, i);
                    }
                }
                else
                {
                    UpdateUI(critBonusUpgrades, data.critBonusUpgradeLevel, critBonusUpgradeNames, upgradeID);
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
            if(upgrade == autoGenUpgrades)
            {
                upgrade[iD].levelText.text = upgradeLevels[iD].ToString();
            }
            upgrade[iD].costText.text = $"{UpgradeCost(type, iD):F0} Chips";
            upgrade[iD].nameText.text = upgradeNames[iD];
        }
    }

    public double UpgradeCost(string type, int upgradeID)
    {
        var data = GameManager.instance.data;
        switch (type)
        {
            case "click":
                return clickUpgradeBaseCost[upgradeID];
            case "critChance":
                return critChanceUpgradeBaseCost[upgradeID];
            case "critBonus":
                return critBonusUpgradeBaseCost[upgradeID];
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
            case "click":
                if (data.clickUpgradeLevel[upgradeID] == 0 && data.chips >= UpgradeCost(type, upgradeID))
                {
                    data.chips -= UpgradeCost(type, upgradeID);
                    data.clickUpgradeLevel[upgradeID] = 1;

                    clickUpgrades[upgradeID].gameObject.SetActive(false);
                }
                UpdateUpgradeUI(type, upgradeID);
                break;
            case "critChance":
                if (data.critChanceUpgradeLevel[upgradeID] == 0 && data.chips >= UpgradeCost(type, upgradeID))
                {
                    data.chips -= UpgradeCost(type, upgradeID);
                    data.critChanceUpgradeLevel[upgradeID] = 1;
                    critChanceUpgrades[upgradeID].gameObject.SetActive(false);
                }
                UpdateUpgradeUI(type, upgradeID);
                break;
            case "critBonus":
                if (data.critBonusUpgradeLevel[upgradeID] == 0 && data.chips >= UpgradeCost(type, upgradeID))
                {
                    data.chips -= UpgradeCost(type, upgradeID);
                    data.critBonusUpgradeLevel[upgradeID] = 1;
                    critBonusUpgrades[upgradeID].gameObject.SetActive(false);
                }
                UpdateUpgradeUI(type, upgradeID);
                break;
            case "autoGen":
                if (data.chips >= UpgradeCost(type, upgradeID))
                {
                    data.chips -= UpgradeCost(type, upgradeID);
                    data.autoGenUpgradeLevel[upgradeID] += 1;
                }
                UpdateUpgradeUI(type, upgradeID);
                break;
        }
    }
}
