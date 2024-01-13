using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SaveSystem))]
public class SaveingWrapper : MonoBehaviour
{
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.2f;

    private const string currentSaveKey = "currentSaveName";

    private void Start()
    {
        ContinueGame();
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadLastScene());
    }

    // �������� ����� �����͸� ������� ������ �����Ѵ�.
    private IEnumerator LoadLastScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);
        yield return GetComponent<SaveSystem>().LoadLastScene(currentSaveKey);
        yield return fader.FadeIn(fadeInTime);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        GetComponent<SaveSystem>().Save(currentSaveKey);
    }

    public void Load()
    {
        GetComponent<SaveSystem>().Load(currentSaveKey);
    }
}
