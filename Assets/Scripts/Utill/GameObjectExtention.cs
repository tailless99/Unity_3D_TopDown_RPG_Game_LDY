using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtention
{
    public static T GetComponentAroundOrAdd<T>(this GameObject obj) where T : Component
    {
        // ���� ������Ʈ�� �ڽ� ������ �ִ� ������Ʈ�� �����´�.
        T component = obj.GetComponentInChildren<T>(true);
        if (component != null) return component;

        // ���� ������Ʈ���� �θ� ������ �ִ� ������Ʈ�� �����´�.
        component = obj.GetComponentInParent<T>();
        if (component != null) return component;

        // ���� ������Ʈ�� ������Ʈ�� �����´�.
        component = obj.GetComponent<T>();
        if (component != null) return component;

        // ������ �߰��ؼ� ��ȯ�Ѵ�.
        return obj.AddComponent<T>();
    }
}
