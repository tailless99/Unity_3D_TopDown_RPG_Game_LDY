using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private const string SceneSaveName = "LastSceneBuildIndex";
    public IEnumerator LoadLastScene(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (state.ContainsKey(SceneSaveName)) {
            buildIndex = (int)state[SceneSaveName];
        }
        yield return SceneManager.LoadSceneAsync(buildIndex);
        RestoreState(state);
        yield break;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Load();
        }
    }

    public void Save(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        CaptureState(state);
        SaveFile(saveFile, state);
    }

    public void Load(string saveFile)
    {
        RestoreState(LoadFile(saveFile));
    }
    
    public void Delete()
    {

    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        if(!File.Exists(path))
        {
            return new Dictionary<string, object>();
        }

        Debug.Log("Loading To " + path);

        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void SaveFile(string saveFile, object state)
    {
        string path = GetPathFromSaveFile(saveFile);
        Debug.Log("Saving To " + path);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream,state);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
        }
        state[SceneSaveName] = SceneManager.GetActiveScene().buildIndex;
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            string id = saveable.GetUniqueIdentifier();
            if(state.ContainsKey(id))
            {
                saveable.RestoreState(state[id]);
            }
        }
    }

    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }

    
}
