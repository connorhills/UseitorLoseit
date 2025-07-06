using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameManager gameManager;

    public Upgrades clickUpgrade;

    public string clickUpgradeName;

    public double clickUpgradeBaseCost;
    public double clickUpgradeCostMult;

    public void StartUpgradeManager()
    {
        clickUpgradeName = "Chips Per Click";
        clickUpgradeBaseCost = 100;
        clickUpgradeCostMult = 1.6;
        UpdateClickUpgradeUI();
    }

    public void UpdateClickUpgradeUI()
    {
        clickUpgrade.levelText.text = gameManager.data.clickUpgradeLevel.ToString();
        clickUpgrade.costText.text = "Cost: " + Cost().ToString(format: "F0") + " Flasks";
        clickUpgrade.nameText.text = "+1 " + clickUpgradeName;

        double chips = gameManager.data.chips;
        double cost = Cost();
        float fill = Mathf.Clamp01((float)((cost - chips) / cost));
        clickUpgrade.fillImage.fillAmount = fill;
    }

    public double Cost() => clickUpgradeBaseCost * Math.Pow(clickUpgradeCostMult, gameManager.data.clickUpgradeLevel);

    public void BuyUpgrade()
    {
        if(gameManager.data.chips >= Cost())
        {
            gameManager.data.chips -= Cost();
            gameManager.data.clickUpgradeLevel += 1;
        }

        UpdateClickUpgradeUI();
    }
}
