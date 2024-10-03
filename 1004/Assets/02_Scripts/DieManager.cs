using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DieManager : MonoBehaviour
{
    public static DieManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    private PlayerMove player; // PlayerMove ��ũ��Ʈ ����
    private bool isFirstDeath = true; // ù ��� ���θ� ����

    public RawImage[] deathImages; // ��� �� ǥ���� �̹��� �迭
    public Canvas deathCanvas; // �̹������� ǥ���� ĵ����
    public float fadeDuration = 0.5f; // ���̵� ȿ���� ���� �ð� (0.5�ʷ� ����)
    public float respawnDelay = 3.0f; // ��Ȱ ���� �ð� (3�ʷ� ����)

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ��ȯ�Ǿ �� ��ü�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �ٸ� �ν��Ͻ��� �����ϸ� �ı�
        }
    }
    public void RegisterPlayer(PlayerMove player)
    {
        this.player = player;
    }

    public void Die()
    {
        Debug.Log("�÷��̾ ����߽��ϴ�.");

        if (player != null)
        {
            if (isFirstDeath)
            {
                StartCoroutine(ShowDeathImagesAndRespawn());
            }
            else
            {
                HandleDeath(); // ù ����� �ƴ� ��� ��� ó��
                spawn.Instance.Respawn(); // ��Ȱ ó��
                GameManager.Instance.ResetTurn(); // �� �� ����
            }
        }
    }



    IEnumerator ShowDeathImagesAndRespawn()
    {
        isFirstDeath = false; // ù ��� ���� �ٽ� ������� �ʵ��� ����

        // ĵ������ Ȱ��ȭ�Ͽ� �̹����� ǥ���� �غ�
        deathCanvas.gameObject.SetActive(true);

        // ��� �̹����� ��Ȱ��ȭ
        foreach (RawImage img in deathImages)
        {
            img.gameObject.SetActive(false);
        }

        // �̹��� ���������� ǥ��
        for (int i = 0; i < deathImages.Length; i++)
        {
            RawImage currentImage = deathImages[i];
            currentImage.gameObject.SetActive(true);
            currentImage.canvasRenderer.SetAlpha(0f); // �����ϰ� ����
            StartCoroutine(FadeIn(currentImage)); // ���̵� �� ����

            // ���̵� �� �� ��� ���
            yield return new WaitForSeconds(fadeDuration);

            // ���̵� �ƿ��� ���� ��� ���
            yield return new WaitForSeconds(1f - fadeDuration);

            StartCoroutine(FadeOut(currentImage)); // ���̵� �ƿ� ����
            yield return new WaitForSeconds(fadeDuration); // ���̵� �ƿ� �Ϸ� ���

            currentImage.gameObject.SetActive(false); // �̹��� ��Ȱ��ȭ
        }

        // ��� �̹��� ǥ�ð� ������ ĵ������ ��Ȱ��ȭ�ϰ� ��Ȱ ó��
        deathCanvas.gameObject.SetActive(false);
        HandleDeath(); // �÷��̾� ������Ʈ ����
        yield return new WaitForSeconds(respawnDelay); // ��Ȱ ���� �ð� ���
        spawn.Instance.Respawn(); // �÷��̾� ��Ȱ
        GameManager.Instance.ResetTurn(); // �� �� �ʱ�ȭ
    }

    IEnumerator FadeIn(RawImage image)
    {
        image.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    IEnumerator FadeOut(RawImage image)
    {
        image.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    public void HandleDeath()
    {
        // �÷��̾� ������Ʈ ����
        if (player != null)
        {
            Destroy(player.gameObject); // �÷��̾� ������Ʈ ����
        }
    }
}
