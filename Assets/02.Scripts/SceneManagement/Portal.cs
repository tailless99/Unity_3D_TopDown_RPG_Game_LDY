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

        // FadeOut ȿ���� �� �ɶ����� ���
        yield return fader.FadeOut(fadeOutTime);

        // �߰�����
        saveingWrapper.Save();

        // Scene�� �� �ε尡 �ɶ����� ���
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
                
        if(playerController == null) {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            Debug.Log("PlayserController wsas null");
        }

        saveingWrapper.Load();

        // �ٸ� �������� ��Ż�� �����ͼ� ĳ������ ��ġ�� �ʱ�ȭ ��Ų��
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        // ���� ���µ� ���� �����ϱ� ���ؼ� �ٽ� �� �� ����
        saveingWrapper.Save();

        yield return new WaitForSeconds(fadeWaitTime);
        // Fader�� ������ ���� �������� FadeIn ȿ���� ����Ѵ�.
        if(fader == null)
        {
            fader = FindObjectOfType<Fader>();
            Debug.Log("fader was null");
        }
        fader.FadeIn(fadeInTime);
        // PlayserController�� �ٽ� ����ϰ� �Ѵ�
        playerController.enabled = true;
        // ���� ��Ż�� �ı��Ѵ�.
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        // NavMeshAgent�� Ű�� �÷��̾��� ��ġ�� �����ϸ� ��ġ������ ���� �� �ֱ� ������
        // �ϴ� ���� ��ġ�� �ű� �Ŀ� NavMeshAgent�� Ű�� ��
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
