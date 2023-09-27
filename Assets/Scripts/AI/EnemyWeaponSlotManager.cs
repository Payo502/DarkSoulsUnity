using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot leftHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
/*        if (isLeft)
        {
            leftHandSlot.currentWeaponModel = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            //load damage collider
        }
        else
        {
            rightHandSlot.currentWeaponModel = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            //load damage collide
        }*/
    }

    public void LoadWeaponsDamageCollider()
    {
    }
}
