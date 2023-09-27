using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator animator;
    CameraHandler cameraHandler;
    Player player;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;


    public bool isInteracting;

    //player flags
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool shouldJump;

    private void Start()
    {
        cameraHandler = CameraHandler.singleton;
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }
    private void Awake()
    {
        
    }
    private void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        animator.SetBool("isInAir", isInAir);

        inputHandler.TickInput(delta);
        player.HandleMovement(delta);
        player.HandleRollingAndSprinting(delta);
        player.HandleFalling(delta, player.moveDir);
        if(shouldJump)
            player.HandleJumping();

        CheckForInteractableObject(); 

    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.d_Pad_Up = false;
        inputHandler.d_Pad_Down = false;
        inputHandler.d_Pad_Left = false;
        inputHandler.d_Pad_Right = false;
        inputHandler.a_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.inventoryInput = false;

        if (isInAir)
        {
            player.inAirTimer = player.inAirTimer + Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);
                }

                if (inputHandler.a_Input)
                {
                    hit.collider.GetComponent<Interactable>().Interact(this);
                }
            }
        }
        else
        {
            if (interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }

            if (itemInteractableGameObject != null && inputHandler.a_Input )
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }
}
