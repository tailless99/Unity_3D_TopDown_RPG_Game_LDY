using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private float experiencePoints = 0;

    // ����ġ�� ����� �� ȣ���� Action
    public event Action onExPerienceGained;

    // ����ġ�� ��� �Լ�
    public void GainExperience(float experience)
    {
        experiencePoints += experience;
        Debug.Log("EXP : " + experiencePoints);
        onExPerienceGained?.Invoke();
    }

    // ���� ����ġ�� ��ȯ�ϴ� �Լ�
    public float GetPoints() {  return experiencePoints; }
}
