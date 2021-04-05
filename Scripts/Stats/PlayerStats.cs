using UnityEngine;
using System.Collections;

public class PlayerStats : CharacterStats
{
    public ItemContainerObject equipment;

    public Stat energy;
    public float curEnergy { get; private set; }
    public delegate void OnEnergyChanged(float _curEnergy, float _maxEnergy);
    public OnEnergyChanged onEnergyChanged;

    [SerializeField] private float energyRegenPerSec;
    [SerializeField] private float energyRegenDelay;
    private float energyRegenTimer;

    public delegate void OnStatsChanged();
    public OnStatsChanged onStatsChanged;

    protected override void Start()
    {
        curEnergy = energy.GetValue();

        energyRegenTimer = energyRegenDelay;

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            //OnBeforeSlotUpdate(equipment.GetSlots[i]);
            OnAfterSlotUpdate(equipment.GetSlots[i]);
        }
        
        if (onEnergyChanged != null)
            onEnergyChanged.Invoke((int)curEnergy, energy.GetValue());

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (curEnergy < energy.GetValue() && energyRegenTimer <= 0)
        {
            curEnergy += energyRegenPerSec * Time.deltaTime;
            if (onEnergyChanged != null)
            {
                onEnergyChanged.Invoke((int)curEnergy, energy.GetValue());
            }

        }
        else if (curEnergy < energy.GetValue() && energyRegenTimer > 0)
        {
            energyRegenTimer -= Time.deltaTime;
        }
        else
        {
            curEnergy = energy.GetValue();
            energyRegenTimer = energyRegenDelay;
        }
    }

    public bool UseEnergy(int amount)
    {
        if (curEnergy - amount < 0) return false;

        curEnergy -= amount;
        energyRegenTimer = energyRegenDelay;

        if (onEnergyChanged != null)
        {
            onEnergyChanged.Invoke((int)curEnergy, energy.GetValue());
        }

        return true;
    }

    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id <= -1 || _slot == null) return;

        for (int i = 0; i < _slot.item.buffs.Length; i++)
        {
            switch (_slot.item.buffs[i].attribute)
            {
                case Attributes.Health:
                    health.RemoveModifier(_slot.item, _slot.item.buffs[i].value);
                    Heal(-_slot.item.buffs[i].value);
                    break;
                case Attributes.Energy:
                    energy.RemoveModifier(_slot.item, _slot.item.buffs[i].value);
                    break;
                case Attributes.Damage:
                    damage.RemoveModifier(_slot.item, _slot.item.buffs[i].value);
                    break;
                case Attributes.Armor:
                    armor.RemoveModifier(_slot.item, _slot.item.buffs[i].value);
                    break;
                default:
                    break;
            }
        }

        if (onStatsChanged != null)
            onStatsChanged.Invoke();

    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id <= -1 || _slot == null) return;

        for (int i = 0; i < _slot.item.buffs.Length; i++)
        {
            switch (_slot.item.buffs[i].attribute)
            {
                case Attributes.Health:
                    health.AddModifier(_slot.item, _slot.item.buffs[i].value);
                    Heal(_slot.item.buffs[i].value);
                    break;
                case Attributes.Energy:
                    energy.AddModifier(_slot.item, _slot.item.buffs[i].value);
                    break;
                case Attributes.Damage:
                    damage.AddModifier(_slot.item, _slot.item.buffs[i].value);
                    break;
                case Attributes.Armor:
                    armor.AddModifier(_slot.item, _slot.item.buffs[i].value);
                    break;
                default:
                    break;
            }
        }

        if (onStatsChanged != null)
            onStatsChanged.Invoke();
    }
}
