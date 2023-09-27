using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : Interactable
{
    public WeaponItem weapon;
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        //pick up item and add it to players inventory
        PickupItem(playerManager);
    }

    private void PickupItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        Player player;
        AnimatorHandler animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        player = playerManager.GetComponent<Player>();
        animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

        player.rigidbody.velocity = Vector3.zero; //stops player from moving while picking up item
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName; ;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);


    }
}
