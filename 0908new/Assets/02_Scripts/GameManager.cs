using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int turnCount = 0;
    private static GameManager instance;

    private PlayerMove player; // PlayerMove ��ũ��Ʈ ����
    private bool isFirstDeath = true; // ù ��� ���θ� ����

    public RawImage[] deathImages; // ��� �� ǥ���� �̹��� �迭
    public Canvas deathCanvas; // �̹������� ǥ���� ĵ����
    public float fadeDuration = 0.5f; // ���̵� ȿ���� ���� �ð� (0.5�ʷ� ����)
    public float respawnDelay = 3.0f; // ��Ȱ ���� �ð� (3�ʷ� ����)
    


    void Awake()
    {
        // Game Manager �ν��Ͻ��� �ϳ��� �ִ��� Ȯ��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    public void ResetTurn()
    {
        turnCount = 0;
        Debug.Log("�� �� �ʱ�ȭ");
        Debug.Log("���� ��: " + turnCount);
    }
    public void IncrementTurn()
    {
        turnCount++;
        Debug.Log("���� ��: " + turnCount);
    }
    public int GetTurnCount()
    {
        return turnCount;
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
                ResetTurn(); // �� �� ����

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
        ResetTurn();// �� �� �ʱ�ȭ
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

    ///*IEnumerator Respawn()
    //{
    //    // �÷��̾ �ٽ� �����ϰų� ���� ������ϴ� ����
    //    // ���� ���� ������ϴ� ���
    //    //Scene currentScene = SceneManager.GetActiveScene();
    //    //SceneManager.LoadScene(currentScene.name);

        
    //    // ��Ȱ �� �÷��̾��� �ʱ�ȭ �۾� �� �ʿ�� �߰��� �� ����
    //    yield return null;
    //}*/
    public void HandleDeath()
    {
        // �÷��̾� ������Ʈ ����
        if (player != null)
        {
            Destroy(player.gameObject); // �÷��̾� ������Ʈ ����
        }
    }

    /// �Ѿ��� �浹��
    public void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Bullet")) // ��¥ �Ѿ˰� �浹���� ��
        {
            PlayerMove.TakeDamage(100); // HP�� 0���� �����Ͽ� �÷��̾� ��� ó��
            Destroy(collidedObject); // �Ѿ� ����
        }
        else if (collidedObject.CompareTag("FakeBullet")) // ��¥ �Ѿ˰� �浹���� ��
        {
            // ��¥ �Ѿ˰��� �浹�� �÷��̾��� HP�� ������ ���� ����
            Destroy(collidedObject); // ��¥ �Ѿ˸� ����
        }
    }
}