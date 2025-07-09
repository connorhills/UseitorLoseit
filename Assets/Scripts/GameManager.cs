using System;
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
    [SerializeField] private TMP_Text chipsPerSecond;
    [SerializeField] private TMP_Text chipsPerClickText;

    [SerializeField] AudioSource singleChip;

    public double ChipsPerClick()
    {
        double total = 1;
        for (int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            total *= (data.clickUpgradeLevel[i] > 0) ? Math.Pow(2, data.clickUpgradeLevel[i]) : 1;
        }
        
        return total;
    }

    public double ChipsPerSecond()
    {
        double total = 0;
        for (int i = 0; i < data.autoGenUpgradeLevel.Count; i++)
        {
            total += UpgradeManager.instance.autoGenUpgradeBasePow[i] * data.autoGenUpgradeLevel[i];
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
        totalChipsText.text    = $"{data.chips:F0}";
        chipsPerSecond.text    = $"{ChipsPerSecond():F2}/s";
        chipsPerClickText.text = "$" + ChipsPerClick();

        data.chips += ChipsPerSecond() * Time.deltaTime;
    }

    public void increase_chips()
    {
        data.chips += ChipsPerClick();
        singleChip.PlayOneShot(singleChip.clip);
    }
}
