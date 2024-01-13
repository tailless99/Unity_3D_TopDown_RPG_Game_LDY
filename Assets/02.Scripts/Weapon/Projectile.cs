using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{   // 발사체

    [SerializeField] float speed = 1;
    [SerializeField] float maxLifeTime = 10;    // 사거리
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
        // 추격을 해야한다면 타겟의 방향으로 회전시킨다.
        if(target != null && isHoming && !target.IsDead()) 
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    /// <summary>
    /// Health를 반환하는 경우
    /// </summary>
    /// <param name="target"></param>
    /// <param name="instigator"></param>
    /// <param name="damage"></param>
    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        SetTarget(instigator, damage, target);
    }

    /// <summary>
    /// TargetPoint를 설정하는 경우
    /// </summary>
    /// <param name="targetPoint"></param>
    /// <param name="instigator"></param>
    /// <param name="damage"></param>
    public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
    {
        SetTarget(instigator, damage, null, targetPoint);
    }

    /// <summary>
    /// 타겟 데이터를 설정하는 함수
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
        // 기본 방향
        if (target == null) return targetPoint;

        CharacterController targetCapsule = target.GetComponent<CharacterController>();
        // 콜라이더가 없다면, 타겟의 포지션값을 반환
        if (targetCapsule == null) return target.transform.position;
        // 콜라이더가 있으면 콜라이더의 높이값ㅇ르 계산해서 캐릭터의 중앙을 타겟팅한다.
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        
        if (health != null && health != target) return; // 타겟의 health가 아니라면 종료
        if (health != null && health.IsDead()) return;  // 타겟이 죽었을 때 종료
        if (other.gameObject == instigator) return;     // 플레이어 캐릭터랑 Trigger 되었다면 종료

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
