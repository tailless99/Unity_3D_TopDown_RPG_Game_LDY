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
        // 네브매쉬 경로
        NavMeshPath path = new NavMeshPath();
        // NavMesh.CalculatePath 함수는 최단 경로를 찾는 함수이다.
        bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        if(!hasPath) return false;

        // 갈 수 없는 경로여서 경로가 만들어지지 않았다면
        if(path.status != NavMeshPathStatus.PathComplete) return false;
        if(GetPathLength(path) > maxNavPathLength) return false;

        return true;
    }

    private float GetPathLength(NavMeshPath path)
    {
        float total = 0;
        // 경로상에 코너가 두개 
        if (path.corners.Length < 2) return total;
        for(int i = 0; i < path.corners.Length - 1; i++)
        {
            // Vector3.Distance함수는 두 포인트 지점 사이의 거리를 계산하는 함수이다.
            total += Vector3.Distance(path.corners[i], path.corners[i+1]);
        }
        return total;
    }

    public void MoveTo(Vector3 destination, float speedFaction) { 
        // 목표 위치 설정
        navMeshAGent.destination = destination;
        // 네브매쉬 에이전트의 속도값 설정
        navMeshAGent.speed = maxMoveSpeed * Mathf.Clamp01(speedFaction); // Mathf.Clamp01 함수는 파라메터의 숫자를 0~1사이의 값으로 바꿔준다.(퍼센트 개념)
        // 움직일 수 있도록 설정
        navMeshAGent.isStopped = false;
    }
}
