using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    Player player;
    int vertical;
    int horizontal;
    public bool canRotate;

    PlayerStats playerStats;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        player = GetComponentInParent<Player>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

        playerStats = GetComponent<PlayerStats>();
    }
    private void OnEnable()
    {
        CountdownTimer.OnTimerEnd += DisablePlayerMovement;
    }

    private void OnDisable()
    {
        CountdownTimer.OnTimerEnd -= DisablePlayerMovement;
    }

    private void Start()
    {
        EnablePlayerMovement();
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f; // clamping values
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f; // clamping values
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        player.rigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        player.rigidbody.velocity = velocity;
    }

    private void DisablePlayerMovement()
    {
        animator.enabled = false;
        player.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void EnablePlayerMovement()
    {
        animator.enabled = true;
        player.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
