using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float chasDistance = 5f; // 플레이어를 추격하기 시작하는 거리
    [SerializeField] private float suspicionTime = 3f; // 플레이어를 의심하기 시작하는 시간
    [SerializeField] private float agroCooldownTime = 5f; // 평소 상태로 돌아가는 쿨다운 시간
    [SerializeField] private PatrolPath patrolPath; // 순찰경로
    [SerializeField] private float wayPointToLerance = 1f; // 순찰 지점 도달 혀용 거리
    [SerializeField] private float wayPointDwellTime = 3f; // 순찰 지점 도착 후 대기 시간
    [Range(0f, 1f)]
    [SerializeField] private float patrolSpeedFraction = 0.2f; // 순찰 속도 비율
    [SerializeField] private float shoutDistance = 5f; // 주변의 적에게 알려주는 범위

    Fighter fighter; // 공격 담당
    Health health; // 체력 담당
    Mover mover; // 이동 담당
    GameObject player; // 플레이어 타겟

    LazyValue<Vector3> guardPosition; // AI 경계 위치
    float timeSinceLastSawPlayer = Mathf.Infinity; // 마지막으로 플레이어를 본 시간
    float timeSinceArrivedAtWaypoint = Mathf.Infinity; // 순찰 지점에 도착한 시간
    float timeSinceAggrevated = Mathf.Infinity; // 공격 상태로 전환한 후 경과한 시간
    int currentWaypointIndex = 0;

    private void Awake()
    {
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        player = GameObject.FindWithTag("Player");

        guardPosition = new LazyValue<Vector3>(GetGuardPosition);
    }

    private Vector3 GetGuardPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        guardPosition.ForceInit(); // 경계 위치 초기화
    }

    private void Update()
    {
        if(health.IsDead()) return;

        if(IsAggrevated() && fighter.CanAttack(player))
        {
            AttackBehavior(); // 공격 동작
        }
        else if(timeSinceLastSawPlayer < suspicionTime)
        {
            SuspicionBehavior(); // 의심 동작
        }
        else
        {
            patrolBehavior(); // 순찰 동작
        }

        UpdateTimers();
    }

    // 시간 경과 타이머 업데이트
    private void UpdateTimers()
    {
        timeSinceAggrevated += Time.deltaTime;
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    // 순찰 동작
    private void patrolBehavior()
    {
        Vector3 nextPosition = guardPosition.value;

        if(patrolPath != null)
        {
            // 목표 지점에 도달했는지 확인
            if(AtWayPoint())
            {
                // 도달했다면 timeSinceArrivedAtWaypoint를 0으로 초기화
                timeSinceArrivedAtWaypoint = 0;
                CycleWayPoint();
            }
            nextPosition = GetCurrentWayPoint(); // 다음 이동 지점으로 갱신
        }

        // 목표지점에 도착 후 경과한 시간이 다른 지점으로 이동할 시간보다 크다면
        // 다른 지점으로 이동시킨다.
        if(timeSinceArrivedAtWaypoint > wayPointDwellTime)
        {
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }
    }

    // 다음 순찰 지점으로 순환
    private void CycleWayPoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }


    // 순찰 지점에 도달했는지 확인
    private bool AtWayPoint()
    {
        float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        return distanceToWayPoint < wayPointToLerance;
    }

    private Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    // 의심 동작
    private void SuspicionBehavior()
    {
        GetComponent<ActionScheduler>().CancleCurrentAction();
    }

    // 공격 상태 => 공격
    private void AttackBehavior()
    {
        timeSinceLastSawPlayer = 0;
        fighter.Attack(player);
    }

    // 주변의 다른 AI를 공격 상태로 전환
    private void AggrevateNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
        foreach(RaycastHit hit in hits) { 
            AIController ai = hit.collider.GetComponent<AIController>();
            if(ai == null) continue;

            ai.Aggrevate();
        }
    }

    // ai를 공격 상태로 전환합니다.
    private void Aggrevate()
    {   // IsAggrevated 에서 어그로 조건을 충족시키기 위해서 0으로 초기화한다.
        timeSinceAggrevated = 0;
    }

    // 플레이어에게 어그로 끌린 상태인지 확인
    private bool IsAggrevated()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < chasDistance || timeSinceAggrevated < agroCooldownTime;
    }
}
