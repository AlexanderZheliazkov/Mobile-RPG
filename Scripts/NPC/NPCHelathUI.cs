using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class NPCHelathUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;
    public float visibleTime = 5f;
    public Vector3 offest;

    float lastMadeVisibleTime;

    const float healthDownSLiderSmoothTime = .1f;

    Transform ui;
    Image healthSlider;
    Image healthDownSlider;
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        target = transform;

        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetComponentInChildren<HealthSliderMarker>().GetComponent<Image>();
                healthDownSlider = ui.GetComponentInChildren<DownSliderMarker>().GetComponent<Image>();

                //ui.gameObject.SetActive(false);
                break;
            }
        }

        GetComponent<CharacterStats>().OnHealthChange += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, float currentHealth)
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;
            float healthPurcent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPurcent;
            if (currentHealth <= 0) Destroy(ui.gameObject);
        }
    }

    void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position + offest;
            ui.forward = -cam.forward;

            if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }

            if (healthDownSlider != null && healthSlider != null)
            {
                if (healthDownSlider.fillAmount != healthSlider.fillAmount)
                {
                    healthDownSlider.fillAmount = Mathf.Lerp(healthDownSlider.fillAmount, healthSlider.fillAmount, healthDownSLiderSmoothTime);
                }
            }

        }
    }
}
