using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    [SerializeField] private Image upgradePanelImage;
    [SerializeField] private Image scrollbarHandle; 

    public GameObject upgradesSelect;
    public TMP_Text upgradesTabText;
    
    public TMP_Text autoGenUpgradesTabText;
    public GameObject autoGenUpgradeSelect;

    public void SwitchUpgradesUI(string location)
    {
        UpgradeManager.instance.upgradeScroll.gameObject.SetActive(false);
        UpgradeManager.instance.autoGenUpgradeScroll.gameObject.SetActive(false);

        upgradesSelect.SetActive(false);
        autoGenUpgradeSelect.SetActive(false);

        upgradesTabText.color = Color.gray;
        autoGenUpgradesTabText.color = Color.gray;

        switch (location)
        {
            case "Click":
                UpgradeManager.instance.upgradeScroll.gameObject.SetActive(true);
                upgradesSelect.SetActive(true);
                upgradesTabText.color = Color.white;
                break;
            case "AutoGen":
                UpgradeManager.instance.autoGenUpgradeScroll.gameObject.SetActive(true);
                autoGenUpgradeSelect.SetActive(true);
                autoGenUpgradesTabText.color = Color.white;
                break;
        }
    }

}
