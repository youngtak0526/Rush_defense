using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ImageSequenceHandler : MonoBehaviour
{
    // ǥ���� RawImage �迭
    public RawImage[] images = new RawImage[6]; // RawImage �迭 ũ�⸦ 6���� ����

    // ���� �̹��� �ε���
    private int currentImageIndex = 0;

    // ���̵� ���� �ð� (������ ����)
    public float fadeDuration = 0.1f; // 0.1�ʷ� �����Ͽ� ���� ���̵� ȿ��

    // ��ȯ�� ���� �̸�
    public string sceneToLoad = "stage_1";

    // Ŭ�� ���� �ð�
    private float clickCooldown = 1.0f;
    private bool canClick = true;

    void Start()
    {
        // ��� RawImage�� ��Ȱ��ȭ�ϰ� ù ��° �̹����� Ȱ��ȭ
        foreach (RawImage img in images)
        {
            img.gameObject.SetActive(false);
        }
        if (images.Length > 0)
        {
            images[0].gameObject.SetActive(true);
            images[0].canvasRenderer.SetAlpha(1.0f); // ó�� �̹����� �����ϰ� ���̵��� ����
        }
    }

    void Update()
    {
        // ���콺 ���� Ŭ���� �����ϰ�, Ŭ�� ���� �ð� ���� Ŭ�� �Ұ�
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            StartCoroutine(FadeInAndOutImages());
        }
    }

    IEnumerator FadeInAndOutImages()
    {
        canClick = false; // Ŭ�� �Ұ� ����

        // ���� �̹��� �ε����� �迭 ������ �ʰ����� �ʵ��� üũ
        if (currentImageIndex >= images.Length)
        {
            yield break; // �迭 ������ �ʰ��� ���, �޼��� ����
        }

        // ���� �̹��� (���� �̹���)
        RawImage currentImage = images[currentImageIndex];

        // ���� �̹��� �ε����� �̹���
        int nextIndex = currentImageIndex + 1;
        if (nextIndex < images.Length)
        {
            // ���� �̹��� Ȱ��ȭ �� �ʱ� ����
            RawImage nextImage = images[nextIndex];
            nextImage.gameObject.SetActive(true);
            nextImage.canvasRenderer.SetAlpha(0f); // �����ϰ� ����
            nextImage.CrossFadeAlpha(1f, fadeDuration, false); // ���̵� ��
        }

        // ���̵� �ƿ��� �Ϸ�� ������ ���
        if (nextIndex < images.Length)
        {
            // ���� �̹����� ������ ��Ÿ�� ������ ���
            yield return new WaitForSeconds(fadeDuration);

            // ���� �̹��� ���̵� �ƿ�
            currentImage.CrossFadeAlpha(0f, fadeDuration, false);

            // ���̵� �ƿ��� �Ϸ�� ������ ���
            yield return new WaitForSeconds(fadeDuration);

            // ���� �̹��� ��Ȱ��ȭ
            currentImage.gameObject.SetActive(false);
        }
        else
        {
            // ��� �̹����� ������ ��� �� ��� �̹����� ��Ȱ��ȭ
            yield return new WaitForSeconds(fadeDuration);

            foreach (RawImage img in images)
            {
                img.gameObject.SetActive(false); // ��� �̹����� ��Ȱ��ȭ
            }

            // ������ �̹����� ����� �� �� ��ȯ
            SceneManager.LoadScene(sceneToLoad);
        }

        // �ε��� ����
        currentImageIndex++;

        yield return new WaitForSeconds(clickCooldown); // Ŭ�� ���� �ð� ���
        canClick = true; // Ŭ�� ���� ���·� ����
    }
}
