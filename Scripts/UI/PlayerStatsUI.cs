using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerStats stats;

    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI armorTxt;
    public TextMeshProUGUI damageTxt;

    void Start()
    {
        stats.onStatsChanged += UpdataStatsUI;
        UpdataStatsUI();
    }
    
    void Update()
    {
        
    }

    public void UpdataStatsUI()
    {
        healthTxt.text = stats.health.GetValue().ToString();
        armorTxt.text = stats.armor.GetValue().ToString();
        damageTxt.text = stats.damage.GetValue().ToString();
    }
}
