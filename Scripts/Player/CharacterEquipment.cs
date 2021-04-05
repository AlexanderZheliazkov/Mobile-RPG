using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public ItemContainerObject equipment;

    public SkinnedMeshRenderer targetMesh;

    public Transform leftHand;
    private WeaponPrefab lWeapon;
    public Transform rightHand;
    private WeaponPrefab rWeapon;

    public WeaponItem weapon;
    public delegate void OnWeaponChanged(WeaponItem weapon);
    public OnWeaponChanged onWeaponChanged;

    private Dictionary<int, SkinnedMeshRenderer> currentMeshes;
    
    void Start()
    {
        currentMeshes = new Dictionary<int, SkinnedMeshRenderer>();

        leftHand = GetComponentInChildren<LeftWeaponHolderMarker>().transform;
        rightHand = GetComponentInChildren<RightWeaponHolderMarker>().transform;

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            OnBeforeSlotUpdate(equipment.GetSlots[i]);
            OnAfterSlotUpdate(equipment.GetSlots[i]);
        }

        //equipment.Load();
        //SearchForWeapon();
    }

    void Update()
    {

    }

    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id < 0 || _slot == null) return;

        if (equipment.database.ItemObjects[_slot.item.Id] is WeaponItem)
        {
            if (weapon != null)
            {
                if (weapon.leftHandPrefab != null && lWeapon != null)
                {
                    Destroy(leftHand.GetComponentInChildren<WeaponPrefab>().gameObject);
                    lWeapon = null;
                }

                if (weapon.rightHandPrefab != null && rWeapon != null)
                {
                    Destroy(rightHand.GetComponentInChildren<WeaponPrefab>().gameObject);
                    rWeapon = null;
                }
            }

            weapon = null;

            if (onWeaponChanged != null)
                onWeaponChanged.Invoke(weapon);
        }
        else if (equipment.database.ItemObjects[_slot.item.Id] is EquipmentItem)
        {
            if ((equipment.database.ItemObjects[_slot.item.Id] as EquipmentItem).renderer != null)
            {
                Destroy(currentMeshes[_slot.item.Id].gameObject);
                if (currentMeshes[_slot.item.Id])
                    currentMeshes.Remove(_slot.item.Id);
            }
        }
    }

    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id < 0 || _slot == null) return;

        if (equipment.database.ItemObjects[_slot.item.Id] is WeaponItem)
        {
            weapon = equipment.database.ItemObjects[_slot.item.Id] as WeaponItem;

            if (weapon.leftHandPrefab != null)
            {
                WeaponPrefab leftWeapon = Instantiate(weapon.leftHandPrefab, leftHand);
                leftWeapon.transform.localPosition = new Vector3(0, 0, 0);
                lWeapon = leftWeapon;
            }
            if (weapon.rightHandPrefab != null)
            {
                WeaponPrefab rightWeapon = Instantiate(weapon.rightHandPrefab, rightHand);
                rightWeapon.transform.localPosition = new Vector3(0, 0, 0);
                rWeapon = rightWeapon;
            }

            if (onWeaponChanged != null)
                onWeaponChanged.Invoke(weapon);
        }
        else if (equipment.database.ItemObjects[_slot.item.Id] is EquipmentItem)
        {
            if ((equipment.database.ItemObjects[_slot.item.Id] as EquipmentItem).renderer != null)
            {
                SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>((equipment.database.ItemObjects[_slot.item.Id] as EquipmentItem).renderer);
                newMesh.transform.parent = targetMesh.transform;
                newMesh.bones = targetMesh.bones;
                newMesh.rootBone = targetMesh.rootBone;
                currentMeshes.Add(_slot.item.Id, newMesh);
            }
        }
    }

    public void SearchForWeapon()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            if (equipment.GetSlots[i].item.Id < 0) continue;
            InventorySlot _slot = equipment.GetSlots[i];
            if (equipment.database.ItemObjects[_slot.item.Id] is WeaponItem)
            {
                weapon = equipment.database.ItemObjects[_slot.item.Id] as WeaponItem;
            }
        }
        if (onWeaponChanged != null)
            onWeaponChanged.Invoke(weapon);
    }

    private void OnApplicationQuit()
    {
        equipment.Clear();
    }

    public void StartLeftWeaponVFX()
    {
        if (lWeapon == null) return;
        if (lWeapon.AttackVFX != null)
            lWeapon.AttackVFX.SetActive(true);
    }
    public void StartRightWeaponVFX()
    {
        if (rWeapon == null) return;
        if (rWeapon.AttackVFX != null)
            rWeapon.AttackVFX.SetActive(true);
    }
    public void StopLeftWeaponVFX()
    {
        if (lWeapon == null) return;
        if (lWeapon.AttackVFX != null)
            lWeapon.AttackVFX.SetActive(false);
    }
    public void StopRightWeaponVFX()
    {
        if (rWeapon == null) return;
        if (rWeapon.AttackVFX != null)
            rWeapon.AttackVFX.SetActive(false);
    }
}
