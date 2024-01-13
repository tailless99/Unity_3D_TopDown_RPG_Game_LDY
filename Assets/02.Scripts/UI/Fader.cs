using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine currentActiveFade = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOutImmediate()
    {
        canvasGroup.alpha = 1;
    }

    // ���� ��ο����°�
    public Coroutine FadeOut(float time)
    {
        return Fade(1, time);
    }

    // ���� ������°�
    public Coroutine FadeIn(float time)
    {
        return Fade(0, time);
    }

    public Coroutine Fade(float target, float time)
    {
        if(currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }
        currentActiveFade = StartCoroutine(FadeRoutine(target, time));
        return currentActiveFade;
    }

    private IEnumerator FadeRoutine(float target, float time)
    {
        // Approximately �Լ� : �� ���� ������� �� True ��ȯ
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            // deltaTime�� TimeScale�� ������ �޴� �ð��̴�. ���� ���̵忡�� �����ϱ⿣ �������ϴ�.
            // unscaledDeltatime �� TimeScale�� ������ �����ʴ� ������ �ð��� ��ŸŸ���� ��ȯ�ϱ� ������ �̰�쿡�� �̰� ����Ѵ�.
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
            yield return null;
        }
    }
}
