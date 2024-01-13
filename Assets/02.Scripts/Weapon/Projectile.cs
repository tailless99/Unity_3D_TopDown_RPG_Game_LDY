using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{   // �߻�ü

    [SerializeField] float speed = 1;
    [SerializeField] float maxLifeTime = 10;    // ��Ÿ�
    [SerializeField] float lifeAfterImpact = 2;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] UnityEvent onHit;

    Health target = null;
    private Vector3 targetPoint;
    GameObject instigator = null;
    float damage = 0;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    private void Update()
    {
        // �߰��� �ؾ��Ѵٸ� Ÿ���� �������� ȸ����Ų��.
        if(target != null && isHoming && !target.IsDead()) 
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    /// <summary>
    /// Health�� ��ȯ�ϴ� ���
    /// </summary>
    /// <param name="target"></param>
    /// <param name="instigator"></param>
    /// <param name="damage"></param>
    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        SetTarget(instigator, damage, target);
    }

    /// <summary>
    /// TargetPoint�� �����ϴ� ���
    /// </summary>
    /// <param name="targetPoint"></param>
    /// <param name="instigator"></param>
    /// <param name="damage"></param>
    public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
    {
        SetTarget(instigator, damage, null, targetPoint);
    }

    /// <summary>
    /// Ÿ�� �����͸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="instigator"></param>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    /// <param name="targetPoint"></param>
    private void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
    { 
        this.target = target;
        this.targetPoint = targetPoint;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation()
    {
        // �⺻ ����
        if (target == null) return targetPoint;

        CharacterController targetCapsule = target.GetComponent<CharacterController>();
        // �ݶ��̴��� ���ٸ�, Ÿ���� �����ǰ��� ��ȯ
        if (targetCapsule == null) return target.transform.position;
        // �ݶ��̴��� ������ �ݶ��̴��� ���̰����� ����ؼ� ĳ������ �߾��� Ÿ�����Ѵ�.
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        
        if (health != null && health != target) return; // Ÿ���� health�� �ƴ϶�� ����
        if (health != null && health.IsDead()) return;  // Ÿ���� �׾��� �� ����
        if (other.gameObject == instigator) return;     // �÷��̾� ĳ���Ͷ� Trigger �Ǿ��ٸ� ����

        health.TakeDamage(instigator, damage);

        speed = 0;
        onHit.Invoke();

        if(hitEffect)
        {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }

        //foreach(GameObject toDestory in destroyOnHit)
        //{
        //    Destroy(toDestory);
        //}
        
        Destroy(gameObject);
    }
}
