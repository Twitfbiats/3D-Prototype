using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(SkillableObject), typeof(RotatableObject), typeof(TargetableObject))]
public class PlayerScript : CustomMonoBehavior
{
    [Header("General")]
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float jumpVelocity = 1f;
    [SerializeField] private GameObject cameraOfPlayer;
    [SerializeField] private Vector3 directionVector;
    [SerializeField] private Vector2 moveVector;
    // [SerializeField] private GameObject viewRotationPoint;
    private TargetableObject targetableObject;
    MultiAimConstraint multiAimConstraint;
    public MultiAimConstraintData multiAimConstraintData;
    public RigBuilder rigBuilder;
    public delegate void CameraDelegate(GameObject target);
    public static CameraDelegate cameraDelegate;
    [SerializeField] private Transform attackPosition;
    UtilObject utilObject = new UtilObject();
    private PlayerInputSystem playerInputSystem;
    // Start is called before the first frame update
    new void Awake()
    {
        //playerInput = gameObject.GetComponent<PlayerInput>();
        base.Awake();
        EntityType = "Player";
        PlayerInputSystem = new PlayerInputSystem();
        PlayerInputSystemBool = true;

        utilObject.BindKey(PlayerInputSystem, "Attack", "Attack", GetType(), this);
        //MultiAimConstraintData multiAimConstraint = GetComponentInChildren<MultiAimConstraintData>();
    }

    private void Start() 
    {
        cameraOfPlayer = GameObject.Find("Main Camera");
        RotatableObject = GetComponent<RotatableObject>();
        Animator = GetComponent<Animator>();
        targetableObject = GetComponent<TargetableObject>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        multiAimConstraintData = multiAimConstraint.data;
        rigBuilder = GetComponentInChildren<RigBuilder>();
        SkillableObject = GetComponent<SkillableObject>();
        attackPosition = GameObject.Find("AttackPosition").transform;
        attackCoroutine = StartCoroutine(NullCoroutine());
        //instantiate movementByCamera and set parent to this
        cameraLookPoint = Instantiate(new GameObject(), transform);
        
    }

    private Coroutine attackCoroutine;
    private IEnumerator NullCoroutine() {yield return new WaitForSeconds(0);}
    [SerializeField] private bool isAttack = false;
    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(AttackHandler());
        }
        else isAttack = false;
    }

    IEnumerator AttackHandler()
    {
        while (isAttack)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            if (SkillableObject.CanAttack)
            {
                Vector3 rotateDirection = targetableObject.TargetChecker.NearestTarget.transform.position - transform.position;
                SkillableObject.PerformAttack(targetableObject.TargetChecker.NearestTarget.transform, rotateDirection);
                if (UnityRandom.Range(0, 2) == 0) Animator.SetBool("Attack_Mirror", true);
                else Animator.SetBool("Attack_Mirror", false);
                if (!targetableObject.IsTarget)
                {
                    targetableObject.Target();
                }
                Animator.SetBool("Attack", true);
                if (SkillableObject.AnimatorIsUsingSkill == 0) Animator.Play("UpperBody.Attack", 1, 0);
                StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(StopAttack());
            }
        }
    }


    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(3);

        targetableObject.Reset();
    }

    public void StopAttackAnimation()
    {
        Animator.SetBool("Attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        Move();
    }

    [SerializeField] private bool canMove = true;

    public TargetableObject TargetableObject { get => targetableObject; set => targetableObject = value; }
    public GameObject CameraLookPoint { get => cameraLookPoint; set => cameraLookPoint = value; }
    public PlayerInputSystem PlayerInputSystem { get => playerInputSystem; set => playerInputSystem = value; }

    private GameObject cameraLookPoint;
    private Vector3 movementByCameraDirectionVector;

    public void Move()
    {
        // var moveVector = move.ReadValue<Vector2>();
        moveVector = PlayerInputSystem.Control.Move.ReadValue<Vector2>();
        Animator.SetFloat("Speed", moveVector.magnitude);
        Animator.SetFloat("MoveVectorX", moveVector.x);
        Animator.SetFloat("MoveVectorY", moveVector.y);

        movementByCameraDirectionVector = new Vector3(transform.position.x, 0, transform.position.z)
         -  new Vector3(cameraOfPlayer.transform.position.x, 0, cameraOfPlayer.transform.position.z);
        cameraLookPoint.transform.rotation = Quaternion.LookRotation(movementByCameraDirectionVector);
        movementByCameraDirectionVector = cameraLookPoint.transform.TransformPoint(new Vector3(moveVector.x, 0, moveVector.y));
        movementByCameraDirectionVector -= new Vector3(transform.position.x, 0, transform.position.z); movementByCameraDirectionVector.Normalize();

        if (moveVector != Vector2.zero && canMove)
        {
            transform.position +=  movementByCameraDirectionVector * moveSpeed;
            
            RotatableObject.RotateY(movementByCameraDirectionVector);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("this");
        rigidbody.velocity += new Vector3(0, jumpVelocity);
    }


    public IEnumerator test(object value)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(value);
    }

    void OnEnable()
    {
        PlayerInputSystem.Control.Enable();
    }

    void OnDisable()
    {
        PlayerInputSystem.Control.Disable();
    }
}

// public bool isViewDirection = false;
    // public Vector3 viewDirection;
    // public IEnumerator ViewDirectionHandler(InputAction viewDirection)
    // {
    //     isViewDirection = true;
    //     Animator.SetBool("ViewDirection", isViewDirection);
    //     while (viewDirection.IsPressed())
    //     {
    //         yield return new WaitForSeconds(Time.fixedDeltaTime);

    //         var rawPose = Camera.main.WorldToScreenPoint(viewRotationPoint.transform.position);
    //         rawPose.Scale(new Vector3(1 / GlobalObject.Instance.screenResolution.x, 1 / GlobalObject.Instance.screenResolution.y, 1f));
    //         this.viewDirection = new Vector3(GlobalObject.Instance.mouse.x - rawPose.x, 0, GlobalObject.Instance.mouse.y - rawPose.y);
            
    //         RotatableObject.RotateToDirectionAxisXZ(this.viewDirection);
    //     }
    //     isViewDirection = false;
    //     Animator.SetBool("ViewDirection", isViewDirection);
    // }
