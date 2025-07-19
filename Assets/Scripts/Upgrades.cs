using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public int upgradeID;

    public Image upgradeButton;

    public TMP_Text levelText;
    public TMP_Text nameText;
    public TMP_Text costText;

    public void BuyClickUpgrade() => UpgradeManager.instance.BuyUpgrade("click", upgradeID);
    public void BuyAutoGenUpgrade() => UpgradeManager.instance.BuyUpgrade("autoGen", upgradeID);
    public void BuyCriticalChanceUpgrade() => UpgradeManager.instance.BuyUpgrade("critChance", upgradeID);
    public void BuyCriticalBonusUpgrade() => UpgradeManager.instance.BuyUpgrade("critBonus", upgradeID);
    public void BuyClickMultMinUpgrade() => UpgradeManager.instance.BuyUpgrade("clickMultMin", upgradeID);
    public void BuyClickMultMaxUpgrade() => UpgradeManager.instance.BuyUpgrade("clickMultMax", upgradeID);
    public void BuySlotWidthUpgrade() => UpgradeManager.instance.BuyUpgrade("slotWidth", upgradeID);
    public void BuySlotHeightUpgrade() => UpgradeManager.instance.BuyUpgrade("slotHeight", upgradeID);

    
}
