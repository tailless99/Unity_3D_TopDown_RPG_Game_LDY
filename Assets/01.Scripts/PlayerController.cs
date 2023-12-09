using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxNavMeshProjectionDistance = 1;    // NavMesh�� ������ �Ÿ��� ����
    [SerializeField] private Mover mover;
    private Vector3 _inputVec;

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    void Update()
    {
        InteractWithMovement();
    }

    private void InteractWithMovement()
    {
        // ĳ���Ͱ� ������ Ÿ�� ��ġ
        Vector3 target = Vector3.zero;
        // �÷��̾ Ŭ���� ���� ��ǥ�� ��濡 �ִ��� Ȯ���ϴ� ����
        bool hasHit = RaycastNavMesh(out target);
        if (hasHit)
        {
            if (!mover.CanMoveTo(target)) return;
            if (Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ�� Ȯ��
        {
                mover.MoveTo(target, 1f);
            }
        }
    }

    private bool RaycastNavMesh(out Vector3 target)
    {
        target = Vector3.zero;

        // ���� ������Ʈ�� �ִ����� Ȯ���ϴ� �ڵ�
        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        if (!hasHit) return false;

        // NavMesh�� ����ִ� ��ġ(�̵����� ��ġ����)�� ��ȯ�� �� �ִ��� Ȯ���ϴ� �ڵ�
        NavMeshHit navMeshHit;
        bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
        if (!hasCastToNavMesh) return false;

        target = navMeshHit.position;
        return true;
    }

    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
