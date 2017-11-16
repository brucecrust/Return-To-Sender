<<<<<<< HEAD
﻿using System;
=======
﻿using System;
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour {

<<<<<<< HEAD
    public float startPosX, startPosY, startPosZ;

    private Animator anim;

    public EnemyState enemyState;
    public enum EnemyState
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        Fleeing,
        Dead
    }
    
    AttackType attackType;
    public enum AttackType
    {
        Swing,
        Thrust
    }

    private RectTransform healthBar;

    /// ///////////ENEMY STATISTICS ///////////////////////
=======
    public float startPosX, startPosY, startPosZ;

    private Animator anim;

    public EnemyState enemyState;
    public enum EnemyState
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        Fleeing,
        Dead
    }
    
    AttackType attackType;
    public enum AttackType
    {
        Swing,
        Thrust
    }

    private RectTransform healthBar;

    /// ///////////ENEMY STATISTICS ///////////////////////
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
    public float maxHP;
    public float currentHP;
    //The base stat values governing the player's skills
    public float baseStrength;
    public float baseDefense;
<<<<<<< HEAD
    public float baseAgility;

    /// //////MOVEMENT/////////////////
    Rigidbody rbody;
    public float maxForce = 100f;

    GameObject target;
    private Vector3 targetPosition;

    //Movement variables
    public float viewDistance = 10f;
    public float minPatrolDistance = 1f;
    public float maxPatrolDistance = 5f;
    public float moveSpeed = 2f;
    public float rotDamping = 1f;
    //For selecting a new position to move to in the patrol state
    private float posTimeDelay = 3.5f;
    private float waitStartTime;

    /// ////////////////////////ATTACKING///////////////////
    public float attackRange = 1.5f;
    public AttackType[] availAttackTypes;

    //Assign the starting stats of the enemy
    void Start() {
        anim = GetComponent<Animator>();

        maxHP = 10f;
        currentHP = maxHP;

        baseStrength = 1;
        baseDefense = 1;
        baseAgility = 1;

        rbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetPosition = new Vector3(0f, transform.position.y, 0f);

        healthBar = transform.Find("HealthBar").GetComponent<RectTransform>();

        enemyState = EnemyState.Patrolling;
        availAttackTypes = new AttackType[System.Enum.GetValues(typeof(AttackType)).Length];
        DetermineAttackTypes();

        waitStartTime = Time.time;
    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                //TODO: move around, use pathfinding AI
                CalculateNextMovement();
                //Debug.Log("Enemy movemment isn't working properly.");
                MoveEnemy();
                break;

            case EnemyState.Chasing:
                //TODO: move around, use pathfinding AI
                CalculateNextMovement();
                MoveEnemy();
                break;

            case EnemyState.Attacking:
                HandleAttacking();
                break;

            case EnemyState.Dead:
                anim.SetInteger("EnemyState", -2);
                break;
        }

        //Show and handle the enemy health bar (visual representation for the player)
        if (Vector3.Distance(transform.position, targetPosition) < 15f) //close enough for the player to see
            ManageHealthBar();
        else if (healthBar.gameObject.activeSelf) //Hide the health bar if it is showing
            healthBar.gameObject.SetActive(false);

        //HandleAnimations();


    }

    private void ManageHealthBar()
    {
        //Always look towards the player's cam
        healthBar.transform.LookAt(Camera.main.transform);

        //Only show the enemy's health bar when it starts losing health
        if (currentHP < maxHP && !healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(true);
        else if (currentHP == maxHP && healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(false);
    }

    //TODO: Based on the equipment the enemy is holding (weapon), and the type of enemy, the list of the attack types it can performed need to be set in availAttackTypes
    private void DetermineAttackTypes()
    {
        //FOR NOW ONLY ALLOW SWING ATTACKS
        availAttackTypes[0] = AttackType.Swing;
    }


    private void HandleAttacking()
    {
        //We can attack the target
        if (Vector3.Distance(targetPosition, transform.position) < attackRange)
        {
            //Choose attack type to use, and play attack animation
        }
    }

    //TODO: Manages the state of the enemy and plays specific animations accordingly. Smooth transitions are needed here (blending) to allow seamless changes between states such as
    // walking to attacking etc.
    private void HandleAnimations()
    {

    }



    private void CalculateNextMovement()
    {
        /*
        EnemyState - the integers corresponding to each of the enemy states:

        Idle - 0
        Walking - 1

        
        */

        //Calculate the next movement to be made by the enemy
        switch (enemyState)
        {
            case EnemyState.Idle:
                anim.SetInteger("EnemyState", 0);
                break;

            case EnemyState.Patrolling:
                // Target not in view range, move to random position
                if (Vector3.Distance(transform.position, target.transform.position) > viewDistance)
                {
                    if (Time.time - waitStartTime > posTimeDelay)
                    {
                        //if (anim.GetInteger("EnemyState") != 1)
                        anim.SetInteger("EnemyState", 1);
                        targetPosition = GetNewRandomPosition();
                        waitStartTime = Time.time;
                    }
                }
                else //Within view range of the target
                {
                    //if (anim.GetInteger("EnemyState") != 1)
                    anim.SetInteger("EnemyState", 1);
                    enemyState = EnemyState.Chasing;
                    targetPosition = target.transform.position;
                }
                break;

            case EnemyState.Chasing:
                //Target isn't in view range, change state to patrol
                if (Vector3.Distance(transform.position, target.transform.position) > viewDistance)
                {
                    enemyState = EnemyState.Patrolling;
                    targetPosition = GetNewRandomPosition();
                    waitStartTime = Time.time;
                }
                else
                    targetPosition = target.transform.position;
                break;

           
        }
        //if (name == "skeleton_animated")
            //Debug.Log(enemyState);
        
    }


    /*Gets a new random x and z position and returns it as a vector3, with the y position being the current transform's position. The random distance can only be between the 
     * minimum and maximum patrol distances from the enemy's current position. */
    private Vector3 GetNewRandomPosition()
    {
        float randXPos = UnityEngine.Random.Range(transform.position.x - maxPatrolDistance, transform.position.x + maxPatrolDistance);
        //Clamp the value so a minimum distance to move is factored in
        if (randXPos < transform.position.x) //Less than X position, clamp to min patrol
            randXPos = Mathf.Clamp(randXPos, transform.position.x - maxPatrolDistance, transform.position.x - minPatrolDistance);
        else
            randXPos = Mathf.Clamp(randXPos, transform.position.x + minPatrolDistance, transform.position.x + maxPatrolDistance);

        float randZPos = UnityEngine.Random.Range(transform.position.z - maxPatrolDistance, transform.position.z + maxPatrolDistance);
        if (randZPos < transform.position.z) //Less than Z position, clamp to min patrol
            randZPos = Mathf.Clamp(randZPos, transform.position.z - maxPatrolDistance, transform.position.z - minPatrolDistance);
        else
            randZPos = Mathf.Clamp(randZPos, transform.position.z + minPatrolDistance, transform.position.z + maxPatrolDistance);

        return new Vector3(randXPos, transform.position.y, randZPos);
    }

    private void MoveEnemy()
    {
        if (target)
        {
            //if (name == "skeleton_animated")
                //Debug.Log(target.name);
            //Move towards the target
            Vector3 targetMoveDirection = (targetPosition - transform.position).normalized;
            targetMoveDirection *= moveSpeed;

            //Now calculate the steering force
            Vector3 steerForce = targetMoveDirection - rbody.velocity;
            steerForce *= rbody.mass * 2f;
            Vector3.ClampMagnitude(steerForce, maxForce);

            

            rbody.AddForce(steerForce);

            //Now rotate the enemy to look in the direction of movement
            Quaternion lookAtRotation;
            if (rbody.velocity.normalized != Vector3.zero)
                lookAtRotation = Quaternion.LookRotation(rbody.velocity.normalized, Vector3.up);
            else
                lookAtRotation = Quaternion.identity;

            // Will assume you mean to divide by damping meanings it will take damping seconds to face target assuming it doesn't move
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime / rotDamping);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        }
    }

    public void TakeDamage(float damageValue)
    {
        anim.SetTrigger("GetHit");
        currentHP -= damageValue;
        if (currentHP <= 0)
        {
            HandleDeath();
            healthBar.localScale = new Vector3(0f, 1f, 1f);
        }
        else healthBar.localScale = new Vector3(currentHP / maxHP, 1f, 1f);
    }

    private void HandleDeath()
    {
        enemyState = EnemyState.Dead;
        //Play the death animation
        anim.SetInteger("EnemyState", -1);
    }
=======
    public float baseAgility;

    /// //////MOVEMENT/////////////////
    Rigidbody rbody;
    public float maxForce = 100f;

    GameObject target;
    private Vector3 targetPosition;

    //Movement variables
    public float viewDistance = 10f;
    public float minPatrolDistance = 1f;
    public float maxPatrolDistance = 5f;
    public float moveSpeed = 2f;
    public float rotDamping = 1f;
    //For selecting a new position to move to in the patrol state
    private float posTimeDelay = 3.5f;
    private float waitStartTime;

    /// ////////////////////////ATTACKING///////////////////
    public float attackRange = 1.5f;
    public AttackType[] availAttackTypes;

    //Assign the starting stats of the enemy
    void Start() {
        anim = GetComponent<Animator>();

        maxHP = 10f;
        currentHP = maxHP;

        baseStrength = 1;
        baseDefense = 1;
        baseAgility = 1;

        rbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetPosition = new Vector3(0f, transform.position.y, 0f);

        healthBar = transform.Find("HealthBar").GetComponent<RectTransform>();

        enemyState = EnemyState.Patrolling;
        availAttackTypes = new AttackType[System.Enum.GetValues(typeof(AttackType)).Length];
        DetermineAttackTypes();

        waitStartTime = Time.time;
    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                //TODO: move around, use pathfinding AI
                CalculateNextMovement();
                //Debug.Log("Enemy movemment isn't working properly.");
                MoveEnemy();
                break;

            case EnemyState.Chasing:
                //TODO: move around, use pathfinding AI
                CalculateNextMovement();
                MoveEnemy();
                break;

            case EnemyState.Attacking:
                HandleAttacking();
                break;

            case EnemyState.Dead:
                anim.SetInteger("EnemyState", -2);
                break;
        }

        //Show and handle the enemy health bar (visual representation for the player)
        if (Vector3.Distance(transform.position, targetPosition) < 15f) //close enough for the player to see
            ManageHealthBar();
        else if (healthBar.gameObject.activeSelf) //Hide the health bar if it is showing
            healthBar.gameObject.SetActive(false);

        //HandleAnimations();


    }

    private void ManageHealthBar()
    {
        //Always look towards the player's cam
        healthBar.transform.LookAt(Camera.main.transform);

        //Only show the enemy's health bar when it starts losing health
        if (currentHP < maxHP && !healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(true);
        else if (currentHP == maxHP && healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(false);
    }

    //TODO: Based on the equipment the enemy is holding (weapon), and the type of enemy, the list of the attack types it can performed need to be set in availAttackTypes
    private void DetermineAttackTypes()
    {
        //FOR NOW ONLY ALLOW SWING ATTACKS
        availAttackTypes[0] = AttackType.Swing;
    }


    private void HandleAttacking()
    {
        //We can attack the target
        if (Vector3.Distance(targetPosition, transform.position) < attackRange)
        {
            //Choose attack type to use, and play attack animation
        }
    }

    //TODO: Manages the state of the enemy and plays specific animations accordingly. Smooth transitions are needed here (blending) to allow seamless changes between states such as
    // walking to attacking etc.
    private void HandleAnimations()
    {

    }



    private void CalculateNextMovement()
    {
        /*
        EnemyState - the integers corresponding to each of the enemy states:

        Idle - 0
        Walking - 1

        
        */

        //Calculate the next movement to be made by the enemy
        switch (enemyState)
        {
            case EnemyState.Idle:
                anim.SetInteger("EnemyState", 0);
                break;

            case EnemyState.Patrolling:
                // Target not in view range, move to random position
                if (Vector3.Distance(transform.position, target.transform.position) > viewDistance)
                {
                    if (Time.time - waitStartTime > posTimeDelay)
                    {
                        //if (anim.GetInteger("EnemyState") != 1)
                        anim.SetInteger("EnemyState", 1);
                        targetPosition = GetNewRandomPosition();
                        waitStartTime = Time.time;
                    }
                }
                else //Within view range of the target
                {
                    //if (anim.GetInteger("EnemyState") != 1)
                    anim.SetInteger("EnemyState", 1);
                    enemyState = EnemyState.Chasing;
                    targetPosition = target.transform.position;
                }
                break;

            case EnemyState.Chasing:
                //Target isn't in view range, change state to patrol
                if (Vector3.Distance(transform.position, target.transform.position) > viewDistance)
                {
                    enemyState = EnemyState.Patrolling;
                    targetPosition = GetNewRandomPosition();
                    waitStartTime = Time.time;
                }
                else
                    targetPosition = target.transform.position;
                break;

           
        }
        //if (name == "skeleton_animated")
            //Debug.Log(enemyState);
        
    }


    /*Gets a new random x and z position and returns it as a vector3, with the y position being the current transform's position. The random distance can only be between the 
     * minimum and maximum patrol distances from the enemy's current position. */
    private Vector3 GetNewRandomPosition()
    {
        float randXPos = UnityEngine.Random.Range(transform.position.x - maxPatrolDistance, transform.position.x + maxPatrolDistance);
        //Clamp the value so a minimum distance to move is factored in
        if (randXPos < transform.position.x) //Less than X position, clamp to min patrol
            randXPos = Mathf.Clamp(randXPos, transform.position.x - maxPatrolDistance, transform.position.x - minPatrolDistance);
        else
            randXPos = Mathf.Clamp(randXPos, transform.position.x + minPatrolDistance, transform.position.x + maxPatrolDistance);

        float randZPos = UnityEngine.Random.Range(transform.position.z - maxPatrolDistance, transform.position.z + maxPatrolDistance);
        if (randZPos < transform.position.z) //Less than Z position, clamp to min patrol
            randZPos = Mathf.Clamp(randZPos, transform.position.z - maxPatrolDistance, transform.position.z - minPatrolDistance);
        else
            randZPos = Mathf.Clamp(randZPos, transform.position.z + minPatrolDistance, transform.position.z + maxPatrolDistance);

        return new Vector3(randXPos, transform.position.y, randZPos);
    }

    private void MoveEnemy()
    {
        if (target)
        {
            //if (name == "skeleton_animated")
                //Debug.Log(target.name);
            //Move towards the target
            Vector3 targetMoveDirection = (targetPosition - transform.position).normalized;
            targetMoveDirection *= moveSpeed;

            //Now calculate the steering force
            Vector3 steerForce = targetMoveDirection - rbody.velocity;
            steerForce *= rbody.mass * 2f;
            Vector3.ClampMagnitude(steerForce, maxForce);

            

            rbody.AddForce(steerForce);

            //Now rotate the enemy to look in the direction of movement
            Quaternion lookAtRotation;
            if (rbody.velocity.normalized != Vector3.zero)
                lookAtRotation = Quaternion.LookRotation(rbody.velocity.normalized, Vector3.up);
            else
                lookAtRotation = Quaternion.identity;

            // Will assume you mean to divide by damping meanings it will take damping seconds to face target assuming it doesn't move
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime / rotDamping);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        }
    }

    public void TakeDamage(float damageValue)
    {
        anim.SetTrigger("GetHit");
        currentHP -= damageValue;
        if (currentHP <= 0)
        {
            HandleDeath();
            healthBar.localScale = new Vector3(0f, 1f, 1f);
        }
        else healthBar.localScale = new Vector3(currentHP / maxHP, 1f, 1f);
    }

    private void HandleDeath()
    {
        enemyState = EnemyState.Dead;
        //Play the death animation
        anim.SetInteger("EnemyState", -1);
    }
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
}


