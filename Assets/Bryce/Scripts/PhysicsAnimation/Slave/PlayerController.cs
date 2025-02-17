﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public static Animator masterAnimator;
    //Stores the length of each attack animation (to ensure a new attack can occur only after the current attack duration has been reached)
    public float[] attackAnimLengths;
    private int curAttackAnimIndex;
    private PlayerStatistics playerStats;

    Transform cameraT;
    public Transform slavePlayerT;
    public Rigidbody slaveCOG;

    //This variable indicates how is the current state of character.
    private int playerState;

    private bool isGrounded;
    private bool isAttacking = false;
    private float attackStartTime;

    //Player Attributes
    public Slider healthSlider;
    public Slider staminaSlider;
    public float staminaLoss = 2.5f;
    private float sprintDelay = 2f;
    private float noStaminaStartTime;
    //Movement variables
    public float moveSpeed = 3f;
    public float runSpeed = 6f;
    public float turnSmoothVelocity = 0.5f;
    public float turnSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    //Jumping
    public float jumpHeight;
    [Range(0, 1)]
    public float airControlPercent;
    public static bool isWalking, attacking, isJumping, hasJumped;
    private bool toggleWeapon;

    private int currentClick;

    public GameObject[] weaponsList;
    private GameObject currentWeapon;
    private int currentWeaponIndex, maxWeaponIndexes;

    void Start()
    {
        maxWeaponIndexes = weaponsList.Length;
        currentWeapon = weaponsList[currentWeaponIndex];
        currentWeaponIndex = 0;
        //staminaSlider.value = playerStats.maxStamina;
        // Adding a check for the Component `PlayerStatistics`, as it was throwing NullReferenceException errors.
        if (GetComponent<PlayerStatistics>()) {
            playerStats = GetComponent<PlayerStatistics>();
        } else  {
            Debug.LogWarning("You must attach a `PlayerStatistics` component.", transform);
        }
        cameraT = Camera.main.transform;
        masterAnimator = GetComponent<Animator>();
        slaveCOG = slavePlayerT.GetComponent<Rigidbody>();

        playerState = 0;

        curAttackAnimIndex = 0;
        attackStartTime = Time.time;

        noStaminaStartTime = Time.time;
    }

    //Focus the mouse on the game until Escape is pressed
    private void SetViewControl()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetMouseButtonDown(0))
            Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("n")) {
            ChangeWeapon();
        }

        FocusRaycast();

        //Handles locking the cursor to the center of the screen and making it invisible
        SetViewControl();

        //Initiating an attack
        if (CheckAttacking())
        {
            HandleAttack(currentClick);
        }

        //Movement and Jumping
        if (!isAttacking)
        {
            MoveCharacter();

            if (Input.GetKeyDown(KeyCode.Space) && GroundCollisionController.onGround) {
                isJumping = true;
                Jump();
                isJumping = false;
                hasJumped = true;
            }
        }

        if (Input.GetKeyDown("h")) {
            toggleWeapon = !toggleWeapon;
            ToggleWeapon(toggleWeapon);
        }
    }

    private void SetAnimState()
    {
        masterAnimator.SetInteger("PlayerState", playerState);
    }

    //returns if the user is pressing any keys to move the player
    private bool CheckMoving()
    {
        Vector2 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input != Vector2.zero)
            return true;
        return false;
    }

    //Return if the user has pressed to start an attack or currently attacking
    private bool CheckAttacking()
    {
        //Set isAttacking to false when the duration of the attack anim clip has been reached
        if (isAttacking && Time.time - attackStartTime > attackAnimLengths[curAttackAnimIndex])
            isAttacking = false;

        //Can start an attack if we aren't already attacking
        if (!isAttacking && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if (Input.GetMouseButtonDown(0)) {
                currentClick = 0;
            } else if (Input.GetMouseButtonDown(1)) {
                currentClick = 1;
            }
            attacking = true;
            isAttacking = true;
            attackStartTime = Time.time;

            return true;
        }
        else return false;
    }

    private void HandleAttack(int attackButton)
    {
        if (attackButton == 0) {
            masterAnimator.SetTrigger("Attack");
        } else if (attackButton == 1) {
            masterAnimator.SetTrigger("SecondAttack");
        }
    }

    private void ChangeWeapon() {
        if (currentWeaponIndex < maxWeaponIndexes) {
            currentWeaponIndex += 1;
        } else {
            currentWeaponIndex = 0;
        }
        
        currentWeapon.SetActive(false);
        currentWeapon = weaponsList[currentWeaponIndex];
        print(currentWeapon.name);
        print(currentWeaponIndex);
        currentWeapon.SetActive(true);
    }

    public void RegisterUnarmedCollision(GameObject other)
    {
        //Hit an enemy
        if (other.GetComponent<EnemyController>())
            other.GetComponent<EnemyController>().TakeDamage(playerStats.curStrength);
    }

    private IEnumerator WaitForAnimation(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
    }

    private void FocusRaycast()
    {
        RaycastHit hitInfo;
        int playerLayer = LayerMask.NameToLayer("Player");
        int layermask = ~(1 << playerLayer);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 10f, layermask))
        {
            //Hit an object
            //Debug.Log("raycast hit: " + hitInfo.collider.name);
            InteractableObject interObj = hitInfo.collider.GetComponent<InteractableObject>();
            if (interObj && Input.GetKeyDown(KeyCode.E))
            {
                interObj.Interact();
            }
        }


    }

    /* Determines the rotation and forward direction of the player. It works by getting the forward direction based off the direction the camera is looking at.
     * 
     * Stamina is accounted for here, which is depleted when the player sprints (L-Shift), and once it reaches 0, starts to replenish
     * 
     * Based on the speed of the player calculated previously, the player's movement speed is calculated and applied for movement. The animation related to the player's
     * Idle/Walk/Run value is determined from this speed, and the animation 'speedPercent' is applied here
     */
    private void MoveCharacter()
    {
        Vector2 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        //Set the player rotation based on the forward/sideward input
        if (inputDir != Vector2.zero)
        {
            isWalking = true;
            masterAnimator.SetBool("Walk", true);
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            //slavePlayerT.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(slavePlayerT.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            slavePlayerT.eulerAngles = new Vector3(slavePlayerT.eulerAngles.x, Mathf.SmoothDampAngle(slavePlayerT.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime),
                slavePlayerT.eulerAngles.z);
        } else {
            isWalking = false;
            masterAnimator.SetBool("Walk", false);
        }

        bool running = false;
        //Determine what to do with stamina
        if (staminaSlider.value > 0)
        {
            running = Input.GetKey(KeyCode.LeftShift);
            //Reduce player stamina
            if (running && inputDir != Vector2.zero)
            {
                masterAnimator.SetBool("Sprint", true);
                playerStats.currentStamina -= Time.deltaTime * staminaLoss;
                staminaSlider.value = playerStats.currentStamina / playerStats.maxStamina;
            }
        }
        else //No stamina remaining, save time to start replenishing
            noStaminaStartTime = Time.time;

        if (!running && playerStats.currentStamina < playerStats.maxStamina)
        {
            masterAnimator.SetBool("Sprint", false);
            playerStats.currentStamina += Time.deltaTime * staminaLoss * 0.75f;
            staminaSlider.value = playerStats.currentStamina / playerStats.maxStamina;
        }

        float targetSpeed = ((running) ? runSpeed : moveSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        Vector3 velocity = slaveCOG.transform.forward * currentSpeed;

        slaveCOG.transform.Translate(velocity * Time.deltaTime, Space.World);

        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / moveSpeed * 0.5f);
        masterAnimator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        
    }

    void ToggleWeapon(bool toggle) {
        currentWeapon.SetActive(toggle);
    }

    void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(jumpHeight);
        velocityY = jumpVelocity;
        slaveCOG.AddForce(Vector3.up * jumpHeight);
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (GroundCollisionController.onGround)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    private void DamagePlayer(float damageAmount)
    {
        playerStats.currentHP -= damageAmount;
        healthSlider.value = playerStats.currentHP / playerStats.maxHP;

        if (playerStats.currentHP <= 0f)
            HandleDeath();
    }

    private void HandleDeath()
    {
        //TODO: Player is dead, reload last save.
    }

  
}
