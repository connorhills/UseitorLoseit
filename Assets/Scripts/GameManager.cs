using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Data data;
    public UpgradeManager upgradeManager;

    [SerializeField] private TMP_Text totalChipsText;
    [SerializeField] private TMP_Text chipsPerClickText;

    [SerializeField] AudioSource singleChip;

    public double ChipsPerClick() => 1 + data.clickUpgradeLevel;

    private void Start()
    {
        data = new Data();
        data.chips = 1;

        upgradeManager.StartUpgradeManager();
    }

    private void Update()
    {
        totalChipsText.text = "Chips: " + data.chips;
        chipsPerClickText.text = "$" + ChipsPerClick();
    }

    public void increase_chips()
    {
        data.chips += ChipsPerClick();
        singleChip.PlayOneShot(singleChip.clip);
        upgradeManager.UpdateClickUpgradeUI();
    }
}
