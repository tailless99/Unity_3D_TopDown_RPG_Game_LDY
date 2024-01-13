using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    enum FrameCaptureDestinationIdentifier
    {
        A,
        B,
        C,
        D,
        E,
    }

    [SerializeField] private int sceneToLoad = -1;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private FrameCaptureDestinationIdentifier destination;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private float fadeWaitTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        if(sceneToLoad < 0)
        {
            Debug.LogError("Scene To load not set.");
            yield break;
        }

        DontDestroyOnLoad(this.gameObject);

        Fader fader = FindObjectOfType<Fader>();
        SaveingWrapper saveingWrapper = FindObjectOfType<SaveingWrapper>();
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.enabled = false;

        // FadeOut 효과가 다 될때까지 대기
        yield return fader.FadeOut(fadeOutTime);

        // 중간저장
        saveingWrapper.Save();

        // Scene이 다 로드가 될때까지 대기
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
                
        if(playerController == null) {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            Debug.Log("PlayserController wsas null");
        }

        saveingWrapper.Load();

        // 다른 목적지의 포탈을 가져와서 캐릭터의 위치를 초기화 시킨다
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        // 현재 오픈된 씬을 저장하기 위해서 다시 한 번 저장
        saveingWrapper.Save();

        yield return new WaitForSeconds(fadeWaitTime);
        // Fader가 없으면 새로 가져오고 FadeIn 효과를 재생한다.
        if(fader == null)
        {
            fader = FindObjectOfType<Fader>();
            Debug.Log("fader was null");
        }
        fader.FadeIn(fadeInTime);
        // PlayserController를 다시 사용하게 한다
        playerController.enabled = true;
        // 현재 포탈을 파괴한다.
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        // NavMeshAgent를 키고 플레이어의 위치를 수정하면 위치오류가 생길 수 있기 때문에
        // 일단 끄고 위치를 옮긴 후에 NavMeshAgent를 키는 것
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = otherPortal.spawnPoint.position;
        player.transform.rotation = otherPortal.spawnPoint.rotation;
        player.GetComponent<NavMeshAgent>().enabled = true;
    }

    private Portal GetOtherPortal()
    {
        foreach(Portal portal in FindObjectsOfType<Portal>())
        {
            if(portal == this) continue;
            if (portal.destination != destination) continue;

            return portal;
        }
        return null;
    }
}
