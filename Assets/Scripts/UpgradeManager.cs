using System;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Click Mult Min Upgrade Data")]
    public List<Upgrades> clickMultMinUpgrades;
    public Upgrades[] clickMultMinUpgradePrefabs;

    public bool[] clickMultMinUpgradeIsUnlocked;

    public string[] clickMultMinUpgradeNames;

    public double[] clickMultMinUpgradeBaseCost;
    public double[] clickMultMinUpgradeBasePow;
    public double[] clickMultMinUpgradeUnlockVal;

    [Header("Click Mult Max Upgrade Data")]
    public List<Upgrades> clickMultMaxUpgrades;
    public Upgrades[] clickMultMaxUpgradePrefabs;

    public bool[] clickMultMaxUpgradeIsUnlocked;

    public string[] clickMultMaxUpgradeNames;

    public double[] clickMultMaxUpgradeBaseCost;
    public double[] clickMultMaxUpgradeBasePow;
    public double[] clickMultMaxUpgradeUnlockVal;

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
        MethodManager.UpgradeCheck(GameManager.instance.data.clickMultMinUpgradeLevel, 5);
        MethodManager.UpgradeCheck(GameManager.instance.data.clickMultMaxUpgradeLevel, 6);

        /* Click Upgrades */

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

        /* Crit Chance Upgrades */

        critChanceUpgradeBasePow = new double[] { 0.01, 0.0125, 0.015, 0.02, 0.0222, 0.01, 0.015, 0.02, 0.0777 };
        critChanceUpgradeBaseCost = new double[] { 250, 1250, 13000, 30000, 45000, 112500, 285000, 575000, 1150000 };
        critChanceUpgradeUnlockVal = new double[] { 0, 250, 1250, 13000, 30000, 45000, 112500, 285000, 575000 };
        critChanceUpgradeIsUnlocked = new bool[critChanceUpgradeBaseCost.Length];

        critChanceUpgradeNames = new string[critChanceUpgradeBasePow.Length];
        for (int i = 0; i < critChanceUpgradeBasePow.Length; i++)
        {
            critChanceUpgradeNames[i] = $"+{critChanceUpgradeBasePow[i] * 100}%";
        }

        /* Crit Bonus Upgrades */

        critBonusUpgradeBasePow = new double[] { 0.001, 0.005, 0.01, 0.02, 0.04, 0.05, 0.1, 0.2, 0.4, 0.777 };
        critBonusUpgradeBaseCost = new double[] { 500, 1500, 15000, 35000, 60000, 125000, 325000, 650000, 1350000, 2000000 };
        critBonusUpgradeUnlockVal = new double[] { 0, 500, 1500, 15000, 35000, 60000, 125000, 325000, 650000, 135000 };
        critBonusUpgradeIsUnlocked = new bool[critBonusUpgradeBaseCost.Length];

        critBonusUpgradeNames = new string[critBonusUpgradeBasePow.Length];
        for (int i = 0; i < critBonusUpgradeBasePow.Length; i++)
        {
            critBonusUpgradeNames[i] = $"+{critBonusUpgradeBasePow[i] * 100}%";
        }

        /* Click Mult Min Upgrades */

        clickMultMinUpgradeBasePow = new double[] { 0.1, 0.1, 0.1, 0.1, 0.1 };
        clickMultMinUpgradeBaseCost = new double[] { 10000, 100000, 1000000, 5000000, 10000000 };
        clickMultMinUpgradeUnlockVal = new double[] { 0, 10000, 100000, 1000000, 5000000, };
        clickMultMinUpgradeIsUnlocked = new bool[clickMultMinUpgradeBaseCost.Length];

        clickMultMinUpgradeNames = new string[clickMultMinUpgradeBasePow.Length];
        for (int i = 0; i < clickMultMinUpgradeBasePow.Length; i++)
        {
            clickMultMinUpgradeNames[i] = $"+{clickMultMinUpgradeBasePow[i]}x";
        }

        /* Click Mult Max Upgrades */

        clickMultMaxUpgradeBasePow = new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.5 };
        clickMultMaxUpgradeBaseCost = new double[] { 15000, 200000, 1500000, 7777777, 12500000, 21500000 };
        clickMultMaxUpgradeUnlockVal = new double[] { 0, 15000, 200000, 1500000, 7777777, 1250000 };
        clickMultMaxUpgradeIsUnlocked = new bool[clickMultMaxUpgradeBaseCost.Length];

        clickMultMaxUpgradeNames = new string[clickMultMaxUpgradeBasePow.Length];
        for (int i = 0; i < clickMultMaxUpgradeBasePow.Length; i++)
        {
            clickMultMaxUpgradeNames[i] = $"+{clickMultMaxUpgradeBasePow[i]}x";
        }

        /* AutoGen Upgrades */

        autoGenUpgradeBasePow = new double[] { 1, 5, 10, 20, 35, 50, 100, 250, 400, 1000, 5000, 10000 };
        autoGenUpgradeBaseCost = new double[] { 50, 100, 750, 1500, 5000, 12500, 23500, 55000, 115000, 225000, 545000, 1000000 };
        autoGenUpgradeCostMult = new double[] { 1.4, 1.38, 1.45, 1.43, 1.41, 1.4, 1.39, 1.38, 1.37, 1.36, 1.35, 1.35 };
        autoGenUpgradesUnlocked = new double[] { 0, 50, 100, 750, 1500, 5000, 12500, 23500, 55000, 115000, 225000, 545000 };

        autoGenUpgradeNames = new string[autoGenUpgradeBasePow.Length];
        for (int i = 0; i < autoGenUpgradeBasePow.Length; i++)
        {
            autoGenUpgradeNames[i] = $"+{autoGenUpgradeBasePow[i]} Chips/s";
        }

        /* Instantiate Upgrade Prefabs */

        InstantiateUpgrades(GameManager.instance.data.clickUpgradeLevel, clickUpgradePrefabs, upgradePanel, clickUpgrades);
        InstantiateUpgrades(GameManager.instance.data.critChanceUpgradeLevel, critChanceUpgradePrefabs, upgradePanel, critChanceUpgrades);
        InstantiateUpgrades(GameManager.instance.data.critBonusUpgradeLevel, critBonusUpgradePrefabs, upgradePanel, critBonusUpgrades);
        InstantiateUpgrades(GameManager.instance.data.clickMultMinUpgradeLevel, clickMultMinUpgradePrefabs, upgradePanel, clickMultMinUpgrades);
        InstantiateUpgrades(GameManager.instance.data.clickMultMaxUpgradeLevel, clickMultMaxUpgradePrefabs, upgradePanel, clickMultMaxUpgrades);

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
        UpdateUpgradeUI("clickMultMin");
        UpdateUpgradeUI("clickMultMax");
        UpdateUpgradeUI("autoGen");

        void InstantiateUpgrades(List<int> level, Upgrades[] upgradePrefab, Transform uPanel, List<Upgrades> upgrades)
        {
            for (int i = 0; i < level.Count; i++)
            {
                Upgrades upgrade = Instantiate(upgradePrefab[i], uPanel);
                upgrade.upgradeID = i;
                upgrade.gameObject.SetActive(false);
                upgrades.Add(upgrade);
            }
        }
    }

    public void Update()
    {
        UnlockedUpgrades(clickUpgrades, clickUpgradeIsUnlocked, clickUpgradeUnlockVal, GameManager.instance.data.clickUpgradeLevel);
        UnlockedUpgrades(critChanceUpgrades, critChanceUpgradeIsUnlocked, critChanceUpgradeUnlockVal, GameManager.instance.data.critChanceUpgradeLevel);
        UnlockedUpgrades(critBonusUpgrades, critBonusUpgradeIsUnlocked, critBonusUpgradeUnlockVal, GameManager.instance.data.critBonusUpgradeLevel);
        UnlockedUpgrades(clickMultMinUpgrades, clickMultMinUpgradeIsUnlocked, clickMultMinUpgradeUnlockVal, GameManager.instance.data.clickMultMinUpgradeLevel);
        UnlockedUpgrades(clickMultMaxUpgrades, clickMultMaxUpgradeIsUnlocked, clickMultMaxUpgradeUnlockVal, GameManager.instance.data.clickMultMaxUpgradeLevel);

        for (var i = 0; i < autoGenUpgrades.Count; i++)
        {
            if (!autoGenUpgrades[i].gameObject.activeSelf)
                autoGenUpgrades[i].gameObject.SetActive(GameManager.instance.data.chips >= autoGenUpgradesUnlocked[i]);
        }

        void UnlockedUpgrades(List<Upgrades> upgrades, bool[] unlocked, double[] unlockVal, List<int> level)
        {
            for (var i = 0; i < upgrades.Count; i++)
            {
                if (!unlocked[i] && GameManager.instance.data.chips >= unlockVal[i])
                {
                    unlocked[i] = true;
                }

                bool isUnlocked = unlocked[i] && level[i] == 0;
                upgrades[i].gameObject.SetActive(isUnlocked);
            }
        }
    }

    public void UpdateUpgradeUI(string type, int upgradeID = -1)
    {
        var data = GameManager.instance.data;

        switch (type)
        {
            case "click":
                QuickUpdateUI(clickUpgrades, GameManager.instance.data.clickUpgradeLevel, clickUpgradeNames);
                break;

            case "critChance":
                QuickUpdateUI(critChanceUpgrades, GameManager.instance.data.critChanceUpgradeLevel, critChanceUpgradeNames);
                break;

            case "critBonus":
                QuickUpdateUI(critBonusUpgrades, GameManager.instance.data.critBonusUpgradeLevel, critBonusUpgradeNames);
                break;

            case "clickMultMin":
                QuickUpdateUI(clickMultMinUpgrades, GameManager.instance.data.clickMultMinUpgradeLevel, clickMultMinUpgradeNames);
                break;

            case "clickMultMax":
                QuickUpdateUI(clickMultMaxUpgrades, GameManager.instance.data.clickMultMaxUpgradeLevel, clickMultMaxUpgradeNames);
                break;

            case "autoGen":
                QuickUpdateUI(autoGenUpgrades, GameManager.instance.data.autoGenUpgradeLevel, autoGenUpgradeNames);
                break;
        }
        void QuickUpdateUI(List<Upgrades> upgrades, List<int> level, string[] names)
        {
            if (upgradeID == -1)
            {
                for (int i = 0; i < upgrades.Count; i++)
                {
                    UpdateUI(upgrades, level, names, i);
                }
            }
            else
            {
                UpdateUI(upgrades, level, names, upgradeID);
            }

            void UpdateUI(List<Upgrades> upgrade, List<int> upgradeLevels, string[] upgradeNames, int iD)
            {
                if (upgrade == autoGenUpgrades)
                {
                    upgrade[iD].levelText.text = upgradeLevels[iD].ToString();
                }
                upgrade[iD].costText.text = $"{UpgradeCost(type, iD):F0} Chips";
                upgrade[iD].nameText.text = upgradeNames[iD];
            }
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
            case "clickMultMin":
                return clickMultMinUpgradeBaseCost[upgradeID];
            case "clickMultMax":
                return clickMultMaxUpgradeBaseCost[upgradeID];
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
                QuickBuyUpgrade(GameManager.instance.data.clickUpgradeLevel, clickUpgrades);
                break;
            case "critChance":
                QuickBuyUpgrade(GameManager.instance.data.critChanceUpgradeLevel, critChanceUpgrades);
                break;
            case "critBonus":
                QuickBuyUpgrade(GameManager.instance.data.critBonusUpgradeLevel, critBonusUpgrades);
                break;
            case "clickMultMin":
                QuickBuyUpgrade(GameManager.instance.data.clickMultMinUpgradeLevel, clickMultMinUpgrades);
                break;
            case "clickMultMax":
                QuickBuyUpgrade(GameManager.instance.data.clickMultMaxUpgradeLevel, clickMultMaxUpgrades);
                break;
            case "autoGen":
                QuickBuyUpgrade(GameManager.instance.data.autoGenUpgradeLevel, autoGenUpgrades);
                break;
        }

        void QuickBuyUpgrade(List<int> level, List<Upgrades> upgrade)
        {
            if (level[upgradeID] == 0 && data.chips >= UpgradeCost(type, upgradeID))
            {
                data.chips -= UpgradeCost(type, upgradeID);
                level[upgradeID] = 1;

                upgrade[upgradeID].gameObject.SetActive(false);
            }
            UpdateUpgradeUI(type, upgradeID);
        }
    }


}
