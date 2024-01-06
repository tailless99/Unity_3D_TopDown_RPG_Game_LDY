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

    // 점차 어두워지는것
    public Coroutine FadeOut(float time)
    {
        return Fade(1, time);
    }

    // 점점 밝아지는것
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
        // Approximately 함수 : 두 값이 비슷했을 때 True 반환
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            // deltaTime은 TimeScale에 영향을 받는 시간이다. 따라서 페이드에서 적용하기엔 부적합하다.
            // unscaledDeltatime 은 TimeScale에 영향을 받지않는 본래의 시간의 델타타임을 반환하기 때문에 이경우에는 이걸 사용한다.
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
            yield return null;
        }
    }
}
