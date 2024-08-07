using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class SkeletonSwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool weaponPool {get; set;}

    [SerializeField] private GameObject prefab;
    private Transform weaponParent;

    public override void Attack(Transform location, Vector3 rotateDirection)
    {
        if (CanAttack)
        {
            CanAttack = false;
            PoolObject poolObject = weaponPool.PickOne();
            SkeletonSwordWeapon skeletonSwordWeapon = (SkeletonSwordWeapon)poolObject.Weapon;
            weaponParent = skeletonSwordWeapon.transform.parent;

            weaponParent.transform.position = transform.position;
            weaponParent.rotation = Quaternion.FromToRotation(Vector3.forward, location.position - weaponParent.transform.position + new Vector3(0, 1f, 0));
            skeletonSwordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
            skeletonSwordWeapon.CollideAndDamage.ColliderDamage = 10f;
            
            skeletonSwordWeapon.Attack();
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        prefab = Instantiate(Resources.Load("SkeletonSword")) as GameObject;
        prefab.SetActive(false);
        weaponPool ??= new ObjectPool(prefab, 20, new PoolArgument(typeof(Weapon), PoolArgument.WhereComponent.Child));

        {
            WeaponSubSkills.Add(gameObject.AddComponent<SkeletonSwordSkillCharge>()); WeaponSubSkills[0].WeaponPool = weaponPool;
            WeaponSubSkills.Add(gameObject.AddComponent<SkeletonSwordSkillNonstopThrust>()); WeaponSubSkills[1].WeaponPool = weaponPool;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
    
    }
}
