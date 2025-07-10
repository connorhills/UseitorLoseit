using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    [SerializeField] private Image upgradePanelImage;
    [SerializeField] private Image scrollbarHandle; 

    public GameObject clickUpgradesSelect;
    public GameObject autoGenUpgradeSelect;

    public TMP_Text clickUpgradesTabText;
    public TMP_Text autoGenUpgradesTabText;

    public void SwitchUpgradesUI(string location)
    {
        UpgradeManager.instance.clickUpgradeScroll.gameObject.SetActive(false);
        UpgradeManager.instance.autoGenUpgradeScroll.gameObject.SetActive(false);

        clickUpgradesSelect.SetActive(false);
        autoGenUpgradeSelect.SetActive(false);

        clickUpgradesTabText.color = Color.gray;
        autoGenUpgradesTabText.color = Color.gray;

        switch (location)
        {
            case "Click":
                UpgradeManager.instance.clickUpgradeScroll.gameObject.SetActive(true);
                clickUpgradesSelect.SetActive(true);
                clickUpgradesTabText.color = Color.white;
                break;
            case "AutoGen":
                UpgradeManager.instance.autoGenUpgradeScroll.gameObject.SetActive(true);
                autoGenUpgradeSelect.SetActive(true);
                autoGenUpgradesTabText.color = Color.white;
                break;
        }
    }

}
