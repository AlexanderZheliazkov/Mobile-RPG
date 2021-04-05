using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat health;
    public float currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;

    public Stat vamp;

    [SerializeField] private int healthRegenPerSec;
    [SerializeField] private float healthRegenDelay;
    private float healthRegenTimer;

    public event System.Action<int, float> OnHealthChange;
    public delegate void HealthBelowZero();
    public HealthBelowZero healthBelowZero;

    public delegate void OnBeingHit();
    public OnBeingHit onBeingHit;

    protected virtual void Start()
    {
        currentHealth = health.GetValue();

        if (OnHealthChange != null)
            OnHealthChange.Invoke(health.GetValue(), currentHealth);
    }

    protected virtual void Update()
    {
        if (currentHealth < health.GetValue() && healthRegenTimer <= 0)
        {
            Heal(healthRegenPerSec * Time.deltaTime);
            if (currentHealth >= health.GetValue()) currentHealth = health.GetValue();
        }
        else if (currentHealth < health.GetValue() && healthRegenTimer > 0)
        {
            healthRegenTimer -= Time.deltaTime;
        }
        else
        {
            currentHealth = health.GetValue();
            healthRegenTimer = healthRegenDelay;
        }
    }

    public int TakeDamage(int damage, bool hit = true)
    {
        if (currentHealth <= 0) return 0;

        float armorPercent = armor.GetValue();
        if (armorPercent >= 80) armorPercent = 80;

        damage = (int)(damage - damage * armorPercent / 100);
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (healthBelowZero != null)
                healthBelowZero.Invoke();
        }
        
        if (hit && damage > 0 && currentHealth > 0)
        {
            if (onBeingHit != null)
                onBeingHit.Invoke();
        }

        if (OnHealthChange != null)
            OnHealthChange.Invoke(health.GetValue(), currentHealth);

        healthRegenTimer = healthRegenDelay;

        return damage;
    }

    public int DealDamage(CharacterStats enemy)
    {
        int damageDealed = enemy.TakeDamage(damage.GetValue());
        Heal((int)(damageDealed * (vamp.GetValue() / 100.0)));
        return damageDealed;
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
        Destroy(gameObject);
    }

    public void Heal(float healthAmount)
    {
        if (currentHealth < health.GetValue())
        {
            currentHealth += healthAmount;
            if (currentHealth >= health.GetValue()) currentHealth = health.GetValue();
        }
        else
        {
            currentHealth = health.GetValue();
        }

        if (OnHealthChange != null)
            OnHealthChange.Invoke(health.GetValue(), currentHealth);
    }
}
