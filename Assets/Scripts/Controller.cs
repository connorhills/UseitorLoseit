using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public TMP_Text total_chips_text;

    [SerializeField] AudioSource single_chip;

    [SerializeField] float total_chips;

    private float cpc = 1;

    void Update()
    {
        total_chips_text.text = "Chips: " + total_chips;
    }

    public void increase_chips()
    {
        total_chips += cpc;
        single_chip.PlayOneShot(single_chip.clip);
    }

    public void increase_chips_per_click()
    {
        if(total_chips >= 100)
        {
            cpc++;
            total_chips -= 100;
        }
        else
        {
            Debug.Log("Insufficient Funds");
        }
    }
}
