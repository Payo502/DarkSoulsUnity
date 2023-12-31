using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerManager playerManager;
    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDir;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [SerializeField] float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] float groundDirectionRayDistance = -0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    //Movement Stats
    [SerializeField] float movementSpeed = 4;
    [SerializeField] float walkingSpeed = 1;
    [SerializeField] float rotationSpeed = 10;
    [SerializeField] float sprintSpeed = 6;
    [SerializeField] float fallingSpeed = 4;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>(); // player model under the player game object
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }


    //movement code
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;

    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
        {
            return;
        }

        if (playerManager.isInteracting)
        {
            return;
        }

        moveDir = cameraObject.forward * inputHandler.vertical;
        moveDir += cameraObject.right * inputHandler.horizontal;
        moveDir.Normalize();
        moveDir.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDir *= speed;
        }
        else
        {
            if (inputHandler.moveAmount < 0.5)
            {
                moveDir *= walkingSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDir *= speed;
                playerManager.isSprinting = false;
            }
        }


        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDir, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.animator.GetBool("isInteracting"))
        {
            return;
        }
        if (inputHandler.rollFlag)
        {
            moveDir = cameraObject.forward * inputHandler.vertical;
            moveDir += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDir.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDir);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Backstep", true);
            }

        }
    }

    public void HandleFalling(float delta, Vector3 moveDir)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDir = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDir * fallingSpeed / 10f);
        }

        Vector3 dir = moveDir;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    animatorHandler.PlayTargetAnimation("Landing", true);
                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }

                Vector3 velocity = rigidbody.velocity;
                velocity.Normalize();
                rigidbody.velocity = velocity * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }


        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
        }
        else
        {
            myTransform.position = targetPosition;
        }

    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting)
            return;

        if (inputHandler.jump_Input)
        {
            if (inputHandler.moveAmount > 0)
            {
                moveDir = cameraObject.forward * inputHandler.vertical;
                moveDir += cameraObject.right * inputHandler.horizontal;
                animatorHandler.PlayTargetAnimation("Jump", true);
                moveDir.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDir);
                myTransform.rotation = jumpRotation;

            }
        }

       
    }

}
