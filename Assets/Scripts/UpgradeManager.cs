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

    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradePrefab;

    public ScrollRect clickUpgradeScroll;
    public Transform clickUpgradePanel;

    public string[] clickUpgradeNames;

    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeCostMult;
    public double[] clickUpgradeBasePow;

    public void StartUpgradeManager()
    {
        MethodManager.UpgradeCheck(ref GameManager.instance.data.clickUpgradeLevel, 5);
        clickUpgradeNames    = new[] { "Chips Per Click +1", "Chips Per Click +5", "Chips Per Click +20", "Chips Per Click +50", "Chips Per Click +100" };
        clickUpgradeBaseCost = new double[] { 50, 100, 500, 2500, 10000 };
        clickUpgradeCostMult = new double[] { 1.9, 1.8, 1.75, 1.6, 1.55 };
        clickUpgradeBasePow  = new double[] { 1, 5, 20, 50, 100 };

        for (int i = 0; i < GameManager.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradePanel);
            upgrade.upgradeID = i;
            clickUpgrades.Add(upgrade);
        }
        clickUpgradeScroll.normalizedPosition = new Vector2 (0, 0);
        UpdateClickUpgradeUI();
    }

    public void UpdateClickUpgradeUI(int upgradeID = -1)
    {
        var data = GameManager.instance.data;
        if(upgradeID == -1)
        {
            for(int i = 0; i < clickUpgrades.Count; i++)
            {
                UpdateUI(i);
            }
        }
        else
        {
            UpdateUI(upgradeID);
        }
        

        void UpdateUI(int iD)
        {
            clickUpgrades[iD].levelText.text = data.clickUpgradeLevel[iD].ToString();
            clickUpgrades[iD].costText.text = $"Cost: {ClickUpgradeCost(iD):F2} Chips";
            clickUpgrades[iD].nameText.text = clickUpgradeNames[iD];
        }
    }

    public double ClickUpgradeCost(int upgradeID) => clickUpgradeBaseCost[upgradeID] 
                                                  * Math.Pow(clickUpgradeCostMult[upgradeID], GameManager.instance.data.clickUpgradeLevel[upgradeID]);

    public void BuyUpgrade(int upgradeID)
    {
        var data = GameManager.instance.data;
        if(data.chips >= ClickUpgradeCost(upgradeID))
        {
            data.chips -= ClickUpgradeCost(upgradeID);
            data.clickUpgradeLevel[upgradeID] += 1;
        }

        UpdateClickUpgradeUI();
    }
}
