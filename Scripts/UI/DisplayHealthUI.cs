using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHealthUI : MonoBehaviour
{
    const float SLIDER_SMOOTH_SPEED = .1f;

    private PlayerStats stats;

    public Image healthSlider;
    private float health;
    public TextMeshProUGUI healthTxt;
    public Image healthDownSlider;

    public Image energySlider;
    private float energy;

    void Start()
    {
        stats = GetComponent<PlayerStats>();

        stats.OnHealthChange += UpdateHealth;
        stats.onEnergyChanged += UpdateEnergy;

        healthDownSlider.fillAmount = healthSlider.fillAmount;

        UpdateHealth(stats.health.GetValue(), stats.currentHealth);
        UpdateEnergy(stats.curEnergy, stats.energy.GetValue());
    }

    private void UpdateHealth(int maxHealth, float curHealth)
    {
        float healthPercent = curHealth / maxHealth;
        healthSlider.fillAmount = healthPercent;
        healthTxt.text = ((int)curHealth).ToString();
    }

    private void UpdateEnergy(float curEnergy, float maxEnergy)
    {
        float energyPercent = curEnergy / maxEnergy;
        energy = energyPercent;
    }

    void Update()
    {
        if (healthDownSlider.fillAmount != healthSlider.fillAmount)
        {
            healthDownSlider.fillAmount = Mathf.Lerp(healthDownSlider.fillAmount, healthSlider.fillAmount, SLIDER_SMOOTH_SPEED);
        }
        

        if (energy != energySlider.fillAmount)
        {
            energySlider.fillAmount = Mathf.Lerp(energySlider.fillAmount, energy, SLIDER_SMOOTH_SPEED);
        }
    }
}
