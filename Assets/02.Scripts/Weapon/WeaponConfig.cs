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
    [SerializeField] float percentageBonus = 0; // ũ��Ƽ�� Ȯ��
    [SerializeField] float weaponRange = 2f;
    [SerializeField] bool isRightHanded = true;
    [SerializeField] Projectile projectile = null;

    const string weaponName = "Weapon";

    // ���� �������� �����ϴ� �ڵ�
    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        // ������ �����ϰ� �ִ� ���⸦ �ı��Ѵ�.
        DestroyOldWeapon(rightHand, leftHand);

        // ���ο� ���⸦ �����Ѵ�.
        Weapon weapon = null;
        //������ ���� ���� �ִٸ�
        if(equippedPrefab != null)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);
            weapon = Instantiate(equippedPrefab, handTransform);
            weapon.gameObject.name = weaponName;
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if(animatorOverride != null)
        {
            // �����س��� �������̵�ִϸ����� ��Ʈ�ѷ��� �ִ���
            animator.runtimeAnimatorController = animatorOverride;
        }

        return weapon;
    }

    /// <summary>
    /// �������� ��ȯ���� ������ ��ȯ���� üũ�ϴ� �Լ�
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
    /// ������ ���� ���̳� ������ �տ� �ִ� ���� ���� �ı��Ѵ�.
    /// </summary>
    /// <param name="rightHand"></param>
    /// <param name="leftHand"></param>
    private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon = rightHand.Find(weaponName);

        // ���Ⱑ �����ʿ� ������쿡 ���ʼյ� Ȯ���غ��� ��üũ
        if(oldWeapon == null) oldWeapon = leftHand.Find(weaponName);
        if (oldWeapon == null) return; // �׷����� ������ ���Ϲ�ȯ

        // �̸��� �ٲ㼭 �ߺ� ȣ���� �Ǵ� ���� �����Ѵ�.
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
