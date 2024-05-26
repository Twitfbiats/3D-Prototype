using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool<Weapon> weaponPool {get; set;}
    [SerializeField] private GameObject swordPrefab;
    private Transform swordWeaponParent;

    public override void Awake() 
    {
        base.Awake();
        swordPrefab = Instantiate(Resources.Load("LongSword")) as GameObject;
        swordPrefab.SetActive(false);
        weaponPool ??= new ObjectPool<Weapon>(swordPrefab, 20, ObjectPool<Weapon>.WhereComponent.Child);

        {
            WeaponSubSkills.Add(gameObject.AddComponent<SwordSkillSummonBigSword>()); WeaponSubSkills[0].WeaponPool = weaponPool;
        }
    }

    public override void Attack(Transform target, Vector3 rotationDirection)
    {
        if (CanAttack)
        {
            CanAttack = false;
            ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
            SwordWeapon swordWeapon = (SwordWeapon)objectPoolClass.Component;
            swordWeaponParent = swordWeapon.transform.parent;

            swordWeapon.PlayAttackParticleSystem();
            swordWeapon.CollideAndDamage.ColliderDamage = 10f;
            swordWeaponParent.position = target.position + new Vector3(0, 0.5f, 0);
            swordWeaponParent.rotation = Quaternion.FromToRotation(Vector3.forward, rotationDirection);
            swordWeaponParent.position = swordWeaponParent.transform.TransformPoint(0, 0, -1);
            swordWeaponParent.Rotate(0, 0, Random.Range(0, 360));
            swordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.SkillableObject.CustomMonoBehavior.AllyTags;
            swordWeapon.Attack();
            StartCoroutine(ResetAttack());
        }
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }

    public bool isWaiting = false;
    public Coroutine summonCoroutine;
    public Vector2 skillCastVector;
    public GameObject skillCast;
    public float skillCastAngle;
    public void SummonBigSword(InputAction.CallbackContext callbackContext)
    {
        if (!isWaiting)
        {
            StartCoroutine(HandleSummonSword());
        } else isWaiting = false;
    }

    public IEnumerator HandleSummonSword()
    {
        isWaiting = true;
        while (isWaiting)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            // skillCastVector = (CustomMonoBehavior.PlayerScript.PlayerInputSystem.Control.View.ReadValue<Vector2>() 
            // - (Vector2)Camera.main.WorldToScreenPoint(CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position)).normalized;
            skillCast.transform.position = transform.position;
            skillCast.SetActive(true);
            skillCastAngle = -Vector2.SignedAngle(Vector2.up, skillCastVector);
            skillCast.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        }

        skillCast.SetActive(false);
        CustomMonoBehavior.SkillableObject.UseOnlySkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOneWithoutActive();

        SwordWeapon swordWeapon = (SwordWeapon)objectPoolClass.Component;
        swordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.SkillableObject.CustomMonoBehavior.AllyTags;
        objectPoolClass.GameObject.SetActive(true);
        // we won't use swordWeaponParent variable because it will affect attack logic
        Transform swordWeaponParent1 = swordWeapon.transform.parent;

        swordWeapon.CollideAndDamage.ColliderDamage = 90f;
        swordWeaponParent1.position = CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position;
        swordWeaponParent1.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle - 90, 0));
        swordWeapon.Animator.SetBool("BigSword", true);
        CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", true);
        CustomMonoBehavior.Animator.Play("UpperBody.CastSkillBlowDown", 1, 0);
        StartCoroutine(StopSummon());
        StartCoroutine(StopSword(swordWeapon));
    }

    public IEnumerator StopSword(SwordWeapon swordWeapon)
    {
        yield return new WaitForSeconds(swordWeapon.BigSwordClip.length);
        swordWeapon.Animator.SetBool("BigSword", false);
        CustomMonoBehavior.SkillableObject.StopSkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        
        yield return new WaitForSeconds(1);
        swordWeapon.transform.parent.gameObject.SetActive(false);
        swordWeapon.transform.localScale = Vector3.one;
    }

    IEnumerator StopSummon()
    {
        yield return new WaitForSeconds(CustomMonoBehavior.SkillableObject.CastSkillBlownDown.length);

        CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", false);
    }

    [SerializeField] private Vector3[] thousandSwordOriginalRotation = {new Vector3(-45, -90, 90), new Vector3(-90, 0, 0), new Vector3(-45, 90, -90)};
    [SerializeField] private float startFlyingForce = 30f;
    private Vector3 midPointScale = new Vector3();
    public void ThousandSword(InputAction.CallbackContext callbackContext)
    {
        List<ObjectPoolClass<Weapon>> objectPoolClasses = weaponPool.Pick(3);
        SwordWeapon swordWeapon;
        Transform swordWeaponParent1;
        
        GameObject target = CustomMonoBehavior.SkillableObject.PlayerScript.TargetableObject.TargetChecker.NearestTarget;
        CustomMonoBehavior.Animator.SetBool("HandUpCast", true);
        CustomMonoBehavior.Animator.Play("UpperBody.HandUpCast", 1, 0);
        CustomMonoBehavior.SkillableObject.UseOnlySkillAnimator((int)SkillableObject.SkillID.ThousandSword);
        for (int i=0;i<thousandSwordOriginalRotation.Length;i++)
        {
            swordWeapon = (SwordWeapon)objectPoolClasses[i].Component;
            swordWeapon.CollideAndDamage.ColliderDamage = 20f;
            swordWeaponParent1 = swordWeapon.transform.parent;
            swordWeaponParent1.transform.rotation = Quaternion.Euler(thousandSwordOriginalRotation[i]);
            swordWeaponParent1.position = CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position;
            swordWeapon.FlyingTrail.enabled = true;
            swordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.SkillableObject.CustomMonoBehavior.AllyTags;
            swordWeapon.Animator.SetBool("ThousandSword", true);



            swordWeapon.ParentRigidBody.AddForce(swordWeaponParent1.transform.forward * startFlyingForce, ForceMode.Impulse);
            CoroutineWrapper coroutineWrapper = new CoroutineWrapper();
            IEnumerator thousandSwordCoroutine = ThousandSwordCoroutine(swordWeapon, swordWeaponParent1, target, coroutineWrapper);
            coroutineWrapper.coroutine = StartCoroutine(thousandSwordCoroutine);
        }
    }

    [SerializeField] private float flyingAtTargetSpeed = 1f;
    [SerializeField] private float rotateSpeed = 10;
    private float rotateSpeedPerDeltaTime;
    public IEnumerator ThousandSwordCoroutine(SwordWeapon swordWeapon, Transform swordWeaponParent1, GameObject target, CoroutineWrapper coroutineWrapper)
    {
        yield return new WaitForSeconds(1);

        Vector3 moveDirection = target.transform.position - swordWeaponParent1.transform.position;
        moveDirection = moveDirection.normalized * flyingAtTargetSpeed;
        rotateSpeedPerDeltaTime = rotateSpeed * Time.fixedDeltaTime;
        swordWeapon.ParentRigidBody.velocity = Vector3.zero;
        StartCoroutine(StopThousandSword(swordWeapon, swordWeaponParent1, coroutineWrapper.coroutine));

        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            swordWeaponParent1.transform.position += moveDirection;
            swordWeaponParent1.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards
            (
                swordWeaponParent1.transform.forward
                , target.transform.position - swordWeaponParent1.transform.position, rotateSpeedPerDeltaTime, 0f
            ));
        }
    }

    public IEnumerator StopThousandSword(SwordWeapon swordWeapon, Transform swordWeaponParent1, Coroutine thousandSwordCoroutine)
    {
        yield return new WaitForSeconds(2);

        swordWeapon.Animator.SetBool("ThousandSword", false);
        CustomMonoBehavior.Animator.SetBool("HandUpCast", false);
        CustomMonoBehavior.SkillableObject.StopSkillAnimator((int)SkillableObject.SkillID.ThousandSword);
        StopCoroutine(thousandSwordCoroutine);
        swordWeaponParent1.rotation = Quaternion.Euler(0, 0, 0);
        swordWeapon.transform.localPosition = Vector3.zero;
        swordWeapon.FlyingTrail.enabled = false;
        swordWeaponParent1.gameObject.SetActive(false);
    }
}
