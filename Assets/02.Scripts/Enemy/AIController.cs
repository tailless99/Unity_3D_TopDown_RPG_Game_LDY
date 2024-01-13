using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float chasDistance = 5f; // �÷��̾ �߰��ϱ� �����ϴ� �Ÿ�
    [SerializeField] private float suspicionTime = 3f; // �÷��̾ �ǽ��ϱ� �����ϴ� �ð�
    [SerializeField] private float agroCooldownTime = 5f; // ��� ���·� ���ư��� ��ٿ� �ð�
    [SerializeField] private PatrolPath patrolPath; // �������
    [SerializeField] private float wayPointToLerance = 1f; // ���� ���� ���� ���� �Ÿ�
    [SerializeField] private float wayPointDwellTime = 3f; // ���� ���� ���� �� ��� �ð�
    [Range(0f, 1f)]
    [SerializeField] private float patrolSpeedFraction = 0.2f; // ���� �ӵ� ����
    [SerializeField] private float shoutDistance = 5f; // �ֺ��� ������ �˷��ִ� ����

    Fighter fighter; // ���� ���
    Health health; // ü�� ���
    Mover mover; // �̵� ���
    GameObject player; // �÷��̾� Ÿ��

    LazyValue<Vector3> guardPosition; // AI ��� ��ġ
    float timeSinceLastSawPlayer = Mathf.Infinity; // ���������� �÷��̾ �� �ð�
    float timeSinceArrivedAtWaypoint = Mathf.Infinity; // ���� ������ ������ �ð�
    float timeSinceAggrevated = Mathf.Infinity; // ���� ���·� ��ȯ�� �� ����� �ð�
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
        guardPosition.ForceInit(); // ��� ��ġ �ʱ�ȭ
    }

    private void Update()
    {
        if(health.IsDead()) return;

        if(IsAggrevated() && fighter.CanAttack(player))
        {
            AttackBehavior(); // ���� ����
        }
        else if(timeSinceLastSawPlayer < suspicionTime)
        {
            SuspicionBehavior(); // �ǽ� ����
        }
        else
        {
            patrolBehavior(); // ���� ����
        }

        UpdateTimers();
    }

    // �ð� ��� Ÿ�̸� ������Ʈ
    private void UpdateTimers()
    {
        timeSinceAggrevated += Time.deltaTime;
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    // ���� ����
    private void patrolBehavior()
    {
        Vector3 nextPosition = guardPosition.value;

        if(patrolPath != null)
        {
            // ��ǥ ������ �����ߴ��� Ȯ��
            if(AtWayPoint())
            {
                // �����ߴٸ� timeSinceArrivedAtWaypoint�� 0���� �ʱ�ȭ
                timeSinceArrivedAtWaypoint = 0;
                CycleWayPoint();
            }
            nextPosition = GetCurrentWayPoint(); // ���� �̵� �������� ����
        }

        // ��ǥ������ ���� �� ����� �ð��� �ٸ� �������� �̵��� �ð����� ũ�ٸ�
        // �ٸ� �������� �̵���Ų��.
        if(timeSinceArrivedAtWaypoint > wayPointDwellTime)
        {
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }
    }

    // ���� ���� �������� ��ȯ
    private void CycleWayPoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }


    // ���� ������ �����ߴ��� Ȯ��
    private bool AtWayPoint()
    {
        float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        return distanceToWayPoint < wayPointToLerance;
    }

    private Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    // �ǽ� ����
    private void SuspicionBehavior()
    {
        GetComponent<ActionScheduler>().CancleCurrentAction();
    }

    // ���� ���� => ����
    private void AttackBehavior()
    {
        timeSinceLastSawPlayer = 0;
        fighter.Attack(player);
    }

    // �ֺ��� �ٸ� AI�� ���� ���·� ��ȯ
    private void AggrevateNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
        foreach(RaycastHit hit in hits) { 
            AIController ai = hit.collider.GetComponent<AIController>();
            if(ai == null) continue;

            ai.Aggrevate();
        }
    }

    // ai�� ���� ���·� ��ȯ�մϴ�.
    private void Aggrevate()
    {   // IsAggrevated ���� ��׷� ������ ������Ű�� ���ؼ� 0���� �ʱ�ȭ�Ѵ�.
        timeSinceAggrevated = 0;
    }

    // �÷��̾�� ��׷� ���� �������� Ȯ��
    private bool IsAggrevated()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < chasDistance || timeSinceAggrevated < agroCooldownTime;
    }
}
