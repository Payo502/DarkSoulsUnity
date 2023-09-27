using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool a_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool jump_Input;
    public bool inventoryInput;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UiManager uiManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    [SerializeField] private AudioSource playerAttackSound;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = FindObjectOfType<UiManager>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.Interact.performed += i => a_Input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);

        HandleRollInput(delta);

        HandleAttackInput(delta);

        HandleQuickSlotInput();

        HandleJumpInput();

        HandleInventoryInput();


    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;

    }

    private void HandleRollInput(float delta)
    {
        b_Input = inputActions.PlayerActions.Roll.IsPressed();


        if (b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;

        //handles right hand weapon
        if (rb_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }

            playerAttackSound.Play();
        }

        if (rt_Input)
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;


            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    private void HandleQuickSlotInput()
    {
        inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
        inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
        if (d_Pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if (d_Pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }

    private void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
    }

    private void HandleInventoryInput()
    {
        inputActions.PlayerActions.Inventory.performed += i => inventoryInput = true;

        if (inventoryInput)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uiManager.OpenSelectWindow();
            }
            else
            {
                uiManager.CloseSelectWindow();
            }
        }
    }

}
