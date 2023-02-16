using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;
    public bool isSwitch;
    

    private float turnSpeedMultiplier;
    private float speed = 3f;
   
    private Animator anim;
    private Vector3 targetDirection;
    private Vector2 input;
    private Quaternion freeRotation;
    private Camera mainCamera;
    private CharacterStats characterStats;
    private NavMeshAgent agent;
    private float MoveSpeed = 5f;

    private GameObject attackTarget;
    private float lastAttackTime;
    private bool isDead;
    private float stopDistance;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        characterStats = GetComponent<CharacterStats>();
        stopDistance = agent.stoppingDistance;
    }
    void OnEnable()
    {
        MouseManager.Instance.whenMouseClick += MoveToTarget;
        MouseManager.Instance.whenMouseClickEnemy += EventAttack;
        GameManager.Instance.RegisterPlayer(characterStats);
    }
    void Start()
    {
       /* MouseManager.Instance.whenMouseClick += MoveToTarget;
        MouseManager.Instance.whenMouseClickEnemy += EventAttack;*/

       
        SaveManager.Instance.LoadPlayerData();
    }

    void OnDisable()
    {
        if (!MouseManager.IsInitialized) { return; }
        MouseManager.Instance.whenMouseClick -= MoveToTarget;
        MouseManager.Instance.whenMouseClickEnemy -= EventAttack;
    }

    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null) 
        {
            
                attackTarget = target;
                characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
                StartCoroutine(MoveToAttackTarget());
        
        }
    }
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;


        //TODO:ÐÞ¸Ä¹¥»÷²ÎÊý·¶Î§
        

            while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
            {
                agent.destination = attackTarget.transform.position;
                yield return null;
            }
            transform.LookAt(attackTarget.transform);
            agent.isStopped = true;
            //attack


            if (lastAttackTime < 0)
            {
                anim.SetBool("Critical", characterStats.isCritical);
                anim.SetTrigger("Attack");
                lastAttackTime = characterStats.attackData.coolDown;
            }
        
      
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if(isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.destination = target;
        agent.isStopped= false; 
    }
     

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        if (isDead) { GameManager.Instance.NotifyObserver(); }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
       
    }

    void SwitchAnimation()
    {
        anim.SetBool("Death",isDead);
     
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)||(agent.velocity.sqrMagnitude>0))
        {
            anim.SetFloat("Speed", 2.0f);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        if (isDead||isSwitch) return;
        
        Move();
        
        if (useCharacterForward)
            speed = Mathf.Abs(input.x) + input.y;
        else
            speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);

        speed = Mathf.Clamp(speed, 0f, 1f);
        

        
        UpdateTargetDirection();
        if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
        {
            Vector3 lookDirection = targetDirection.normalized;
            freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
            var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;

            if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
            var euler = new Vector3(0, eulerY, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        }

        

    }

    public virtual void UpdateTargetDirection()
    {
        if (!useCharacterForward)
        {
            turnSpeedMultiplier = 1f;
            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            
            var right = mainCamera.transform.TransformDirection(Vector3.right);

            
            targetDirection = input.x * right + input.y * forward;
        }
        else
        {
            turnSpeedMultiplier = 0.2f;
            var forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            
            var right = transform.TransformDirection(Vector3.right);
            targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
        }
    }

    private void Move()
    {

         if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            
            agent.destination = transform.position;
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

            
        }
    }
    //Animation Event
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
    void OpenSwitch()
    { 
       isSwitch= true;
    }
    void CloseSwitch()
    {
       isSwitch= false;
    }

    




}
