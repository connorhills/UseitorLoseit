using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() => instance = this;

    public Data data;

    [SerializeField] private TMP_Text totalChipsText;
    [SerializeField] private TMP_Text chipsPerClickText;

    [SerializeField] AudioSource singleChip;

    public double ChipsPerClick()
    {
        double total = 1;
        for (int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            total += UpgradeManager.instance.clickUpgradeBasePow[i] * data.clickUpgradeLevel[i];
        }
        
        return total;
    }

    private void Start()
    {
        data = new Data();

        UpgradeManager.instance.StartUpgradeManager();
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
        UpgradeManager.instance.UpdateClickUpgradeUI();
    }
}
