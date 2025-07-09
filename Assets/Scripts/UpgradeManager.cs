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

    public bool[] clickUpgradeIsUnlocked;

    public ScrollRect clickUpgradeScroll;
    public Transform clickUpgradePanel;

    public string[] clickUpgradeNames;

    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeCostMult;
    public double[] clickUpgradeBasePow;
    public double[] clickUpgradeUnlockVal;

    [Header("Auto Gen Upgrade Data")]
    public List<Upgrades> autoGenUpgrades;
    public Upgrades autoGenUpgradePrefab;

    public ScrollRect autoGenUpgradeScroll;
    public Transform autoGenUpgradePanel;

    public string[] autoGenUpgradeNames;

    public double[] autoGenUpgradeBaseCost;
    public double[] autoGenUpgradeCostMult;
    public double[] autoGenUpgradeBasePow;
    public double[] autoGenUpgradesUnlocked;

    public void StartUpgradeManager()
    {
        MethodManager.UpgradeCheck(GameManager.instance.data.clickUpgradeLevel, 8);


        clickUpgradeBasePow = new double[GameManager.instance.data.clickUpgradeLevel.Count];
        double basePow = GameManager.instance.ChipsPerClick() * 2;
        for (int i = 0; i < clickUpgradeBasePow.Length; i++)
        {
            clickUpgradeBasePow[i] = basePow;
        }

        clickUpgradeNames = new string[clickUpgradeBasePow.Length];
        for (int i = 0; i < clickUpgradeBasePow.Length; i++)
        {
            clickUpgradeNames[i] = $"Chips per Click x{clickUpgradeBasePow[i]}";
        }

        clickUpgradeBaseCost   = new double[] { 1000, 10000, 25000, 50000, 100000, 250000, 500000, 1000000 };
        clickUpgradeCostMult   = new double[] { 1.4, 1.42, 1.45, 1.38, 1.4, 1.39, 1.43, 1.41 };
        clickUpgradeUnlockVal  = new double[] { 0, 1000, 10000, 25000, 50000, 100000, 250000, 500000 };
        clickUpgradeIsUnlocked = new bool[clickUpgradeBaseCost.Length];


        autoGenUpgradeBasePow = new double[] { 1000, 5, 10, 20, 35, 50, 100, 250, 400, 1000, 5000, 10000};
        autoGenUpgradeNames = new string[autoGenUpgradeBasePow.Length];
        for (int i = 0; i < autoGenUpgradeBasePow.Length; i++)
        {
            autoGenUpgradeNames[i] = $"+{autoGenUpgradeBasePow[i]} Chips/s";
        }

        autoGenUpgradeBaseCost  = new double[] { 50, 100, 750, 1500, 5000, 12500, 23500, 55000, 115000, 225000, 545000, 1000000 };
        autoGenUpgradeCostMult  = new double[] { 1.35, 1.38, 1.45, 1.43, 1.41, 1.4, 1.39, 1.38, 1.37, 1.36, 1.35, 1.35 };
        autoGenUpgradesUnlocked = new double[] { 0, 50, 100, 750, 1500, 5000, 12500, 23500, 55000, 115000, 225000, 545000 };


        for (int i = 0; i < GameManager.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradePanel);
            upgrade.upgradeID = i;
            upgrade.gameObject.SetActive(false);
            clickUpgrades.Add(upgrade);
        }

        for (int i = 0; i < GameManager.instance.data.autoGenUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(autoGenUpgradePrefab, autoGenUpgradePanel);
            upgrade.upgradeID = i;
            upgrade.gameObject.SetActive(false);
            autoGenUpgrades.Add(upgrade);
        }
        clickUpgradeScroll.normalizedPosition = new Vector2(0, 0);
        autoGenUpgradeScroll.normalizedPosition = new Vector2(0, 0);
        UpdateUpgradeUI("click");
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

        for (var i = 0; i < autoGenUpgrades.Count; i++)
        {
            if (!autoGenUpgrades[i].gameObject.activeSelf)
            {
                autoGenUpgrades[i].gameObject.SetActive(GameManager.instance.data.chips >= autoGenUpgradesUnlocked[i]);
            }
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
            case "click":
                if (data.clickUpgradeLevel[upgradeID] == 0 && data.chips >= UpgradeCost(type, upgradeID))
                {
                    data.chips -= UpgradeCost(type, upgradeID);
                    data.clickUpgradeLevel[upgradeID] = 1;

                    clickUpgrades[upgradeID].gameObject.SetActive(false);
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
