using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour, ISaveable
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

    public object CaptureState() { return experiencePoints; }

    public void RestoreState(object state)
    {
        experiencePoints = (float)state;
        Debug.Log("experiencePoints : " + experiencePoints);
    }
}
