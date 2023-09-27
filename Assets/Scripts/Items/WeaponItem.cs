using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    //one hand attack
    public string OH_Light_Attack_01;
    public string OH_Heavy_Attack_01;
    public string OH_Light_Attack_02;

    public string th_idle;

    //stamina draining
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    public void DrainStaminaLightAttack()
    {

    }

    public void DrainStaminaHeavyAttack()
    {

    }

}
