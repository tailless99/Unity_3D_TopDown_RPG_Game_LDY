using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    // ������ �� ������ �����͸� �Ľ��ؼ� ��ȯ�Ѵ�.
    object CaptureState();

    // �ε��� �� ����� �����͸� �����ͼ� �Ľ��Ѵ�.
    void RestoreState(object state);
}
