using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "weapons/make new Weapon", order = 1)]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] AnimatorOverrideController animatorOverride = null;
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] float weaponDamage = 5f;
    [SerializeField] float percentageBonus = 0; // 크리티컬 확률
    [SerializeField] float weaponRange = 2f;
    [SerializeField] bool isRightHanded = true;
    [SerializeField] Projectile projectile = null;

    const string weaponName = "Weapon";

    // 무기 프리팹을 생성하는 코드
    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        // 기존에 장착하고 있던 무기를 파괴한다.
        DestroyOldWeapon(rightHand, leftHand);

        // 새로운 무기를 생성한다.
        Weapon weapon = null;
        //생성할 무기 모델이 있다면
        if(equippedPrefab != null)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);
            weapon = Instantiate(equippedPrefab, handTransform);
            weapon.gameObject.name = weaponName;
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if(animatorOverride != null)
        {
            // 설정해놓은 오버라이드애니메이터 컨트롤러가 있는지
            animator.runtimeAnimatorController = animatorOverride;
        }

        return weapon;
    }

    /// <summary>
    /// 오른쪽을 반환할지 왼쪽을 반환할지 체크하는 함수
    /// </summary>
    /// <param name="rightHand"></param>
    /// <param name="leftHand"></param>
    /// <returns></returns>
    private Transform GetTransform(Transform rightHand, Transform leftHand)
    {
        Transform handTransform;
        if (isRightHanded) handTransform = rightHand;
        else handTransform = leftHand;
        return handTransform;
    }


    /// <summary>
    /// 기존에 왼쪽 손이나 오른쪽 손에 있는 무기 모델을 파괴한다.
    /// </summary>
    /// <param name="rightHand"></param>
    /// <param name="leftHand"></param>
    private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon = rightHand.Find(weaponName);

        // 무기가 오른쪽에 없을경우에 왼쪽손도 확인해보는 널체크
        if(oldWeapon == null) oldWeapon = leftHand.Find(weaponName);
        if (oldWeapon == null) return; // 그럼에도 없으면 리턴반환

        // 이름을 바꿔서 중복 호출이 되는 것을 방지한다.
        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    public bool HasProjecttile()
    {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target,
        GameObject instigator, float calculatedDamage)
    {
        Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
        projectileInstance.SetTarget(target, instigator, calculatedDamage);
    }

    public float GetRange()
    {
        return weaponRange;
    }
}
