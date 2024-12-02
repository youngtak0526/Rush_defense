using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToStart : MonoBehaviour
{
    // ��ȯ�� ���� �̸�
    public string sceneToLoad = "StageSelect";

    // ���̵� �ƿ��� ó���� CanvasGroup ����
    public CanvasGroup fadeCanvasGroup;

    // ���̵� �ƿ� ���� �ð�
    public float fadeDuration = 1f;

    // �� �ε� ���θ� üũ�ϱ� ���� �÷���
    private bool isSceneLoading = false;

    // �� �����Ӹ��� ȣ��
    void Update()
    {
        // ��ġ �Ǵ� ���콺 Ŭ���� ����
        if (Input.GetMouseButtonDown(0) && !isSceneLoading)
        {
            // �� �ε带 �� ���� �����ϵ��� �÷��� ����
            isSceneLoading = true;

            // ���̵� �ƿ� �� �� �ε� ����
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    // ���̵� �ƿ� �� �� �ε� �ڷ�ƾ
    private System.Collections.IEnumerator FadeOutAndLoadScene()
    {
        // ���̵� �ƿ� (ȭ���� ������ ���������� ����)
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float elapsedTime = Time.time - startTime;
            float percentageComplete = elapsedTime / fadeDuration;

            // ���� �� ���� (0 -> 1)
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, percentageComplete);

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // ���������� ���� ���� 1�� �����Ͽ� ȭ���� ������ ����

        // �� �ε�
        SceneManager.LoadScene(sceneToLoad);
    }
}
