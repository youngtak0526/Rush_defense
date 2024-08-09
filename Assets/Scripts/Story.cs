using UnityEngine;
using UnityEngine.UI;

public class ImageSequenceHandler : MonoBehaviour
{
    // ǥ���� Image �迭
    public Image[] images;

    // ���� �̹��� �ε���
    private int currentImageIndex = 0;

    // ��Ȱ��ȭ�� ĵ����
    public Canvas canvasToDeactivate;

    void Start()
    {
        // ��� Image�� ��Ȱ��ȭ�ϰ� ù ��° �̹����� Ȱ��ȭ
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }
        if (images.Length > 0)
        {
            images[0].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // ���콺 ���� Ŭ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            OnImageClick();
        }
    }

    void OnImageClick()
    {
        // ���� �̹��� �ε����� �迭 ������ �ʰ����� �ʵ��� üũ
        if (currentImageIndex >= images.Length)
        {
            return; // �迭 ������ �ʰ��� ���, �޼��� ����
        }

        // ���� �̹��� ��Ȱ��ȭ
        images[currentImageIndex].gameObject.SetActive(false);

        // ���� �̹����� �̵�
        currentImageIndex++;

        // ���� �̹����� �ִٸ� Ȱ��ȭ, ���ٸ� ĵ���� ��Ȱ��ȭ
        if (currentImageIndex < images.Length)
        {
            images[currentImageIndex].gameObject.SetActive(true);
        }
        else
        {
            canvasToDeactivate.gameObject.SetActive(false);
        }
    }
}
