using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public PlayerMove player; // �������� ������ PlayerMove ����
    public GameObject playerPrefab; // �÷��̾� ������ (��Ȱ �� ���)

    public static Vector3 respawnPoint;
    public int maxHealth = 100;

    public RawImage[] deathImages;
    public Canvas deathCanvas;
    public float fadeDuration = 0.5f;
    public float respawnDelay = 3.0f;

    private bool isFirstDeath = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerMove.playerHP = maxHealth;

        // �÷��̾ ������ �����տ��� ����
        if (player == null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, Quaternion.identity);
            player = newPlayer.GetComponent<PlayerMove>();
        }

        respawnPoint = player.transform.position;
    }
    public void RegisterPlayer(PlayerMove player)
    {
        this.player = player;
    }
    public void Die()
    {
        if (player != null)
        {
            Debug.Log("�÷��̾ ����߽��ϴ�.");
            if (isFirstDeath)
                StartCoroutine(ShowDeathImagesAndRespawn());
            else
                HandleDeath(); // ù ����� �ƴ� ��� ��� ó��
                Respawn();
                GameManager.Instance.ResetTurn(); // �� �� ����
        }
    }

    private IEnumerator ShowDeathImagesAndRespawn()
    {
        isFirstDeath = false;
        deathCanvas.gameObject.SetActive(true);

        foreach (RawImage img in deathImages)
        {
            img.gameObject.SetActive(false);
        }

        foreach (RawImage img in deathImages)
        {
            img.gameObject.SetActive(true);
            img.canvasRenderer.SetAlpha(0f);
            StartCoroutine(FadeIn(img));

            yield return new WaitForSeconds(fadeDuration);
            yield return new WaitForSeconds(1f - fadeDuration);

            StartCoroutine(FadeOut(img));
            yield return new WaitForSeconds(fadeDuration);

            img.gameObject.SetActive(false);
        }

        deathCanvas.gameObject.SetActive(false);
        HandleDeath();

        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    private IEnumerator FadeIn(RawImage image)
    {
        image.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeOut(RawImage image)
    {
        image.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    private void HandleDeath()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }

    public void Respawn()
    {
        Debug.Log("Player Respawned!");
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, Quaternion.identity);
        player = newPlayer.GetComponent<PlayerMove>();
        PlayerMove.playerHP = maxHealth;
    }
}