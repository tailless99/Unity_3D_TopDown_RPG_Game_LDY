using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxMoveSpeed = 6f;
    [SerializeField] private float maxNavPathLength = 40;

    NavMeshAgent navMeshAGent;
    Rigidbody rb;

    private void Awake()
    {
        navMeshAGent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public bool CanMoveTo(Vector3 destination)
    {
        // �׺�Ž� ���
        NavMeshPath path = new NavMeshPath();
        // NavMesh.CalculatePath �Լ��� �ִ� ��θ� ã�� �Լ��̴�.
        bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        if(!hasPath) return false;

        // �� �� ���� ��ο��� ��ΰ� ��������� �ʾҴٸ�
        if(path.status != NavMeshPathStatus.PathComplete) return false;
        if(GetPathLength(path) > maxNavPathLength) return false;

        return true;
    }

    private float GetPathLength(NavMeshPath path)
    {
        float total = 0;
        // ��λ� �ڳʰ� �ΰ� 
        if (path.corners.Length < 2) return total;
        for(int i = 0; i < path.corners.Length - 1; i++)
        {
            // Vector3.Distance�Լ��� �� ����Ʈ ���� ������ �Ÿ��� ����ϴ� �Լ��̴�.
            total += Vector3.Distance(path.corners[i], path.corners[i+1]);
        }
        return total;
    }

    public void MoveTo(Vector3 destination, float speedFaction) { 
        // ��ǥ ��ġ ����
        navMeshAGent.destination = destination;
        // �׺�Ž� ������Ʈ�� �ӵ��� ����
        navMeshAGent.speed = maxMoveSpeed * Mathf.Clamp01(speedFaction); // Mathf.Clamp01 �Լ��� �Ķ������ ���ڸ� 0~1������ ������ �ٲ��ش�.(�ۼ�Ʈ ����)
        // ������ �� �ֵ��� ����
        navMeshAGent.isStopped = false;
    }
}
