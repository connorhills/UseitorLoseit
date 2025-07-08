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

    [SerializeField] private Color clickPanelColor = new Color(0.9f, 0.875f, 0.1f, 0.6f);
    [SerializeField] private Color autoGenPanelColor = new Color(0.1f, 0.56f, 0.8f, 0.6f);

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
                upgradePanelImage.color = clickPanelColor;
                scrollbarHandle.color = clickPanelColor;
                break;
            case "AutoGen":
                UpgradeManager.instance.autoGenUpgradeScroll.gameObject.SetActive(true);
                autoGenUpgradeSelect.SetActive(true);
                autoGenUpgradesTabText.color = Color.white;
                upgradePanelImage.color = autoGenPanelColor;
                scrollbarHandle.color = autoGenPanelColor;
                break;
        }
    }

}
