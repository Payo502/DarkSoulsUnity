using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSLots = new WeaponItem[2];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    public List<WeaponItem> weaponsInventory;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        rightWeapon = unarmedWeapon;
        leftWeapon = unarmedWeapon;
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);

        }
        else if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1; 
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length -1 )
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon,false);
        }
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSLots != null)
        {
            leftWeapon = weaponsInLeftHandSLots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSLots[currentLeftWeaponIndex], true);

        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSLots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSLots[1] != null)
        {
            leftWeapon = weaponsInLeftHandSLots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSLots[currentLeftWeaponIndex], true);
        }
        else
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSLots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }
    }

/*    rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        leftWeapon = weaponsInLeftHandSLots[currentLeftWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);*/
}

