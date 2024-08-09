using UnityEngine;
using UnityEngine.UI;

public class ImageSequenceHandler : MonoBehaviour
{
    // 표시할 Image 배열
    public Image[] images;

    // 현재 이미지 인덱스
    private int currentImageIndex = 0;

    // 비활성화할 캔버스
    public Canvas canvasToDeactivate;

    void Start()
    {
        // 모든 Image를 비활성화하고 첫 번째 이미지만 활성화
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
        // 마우스 왼쪽 클릭을 감지
        if (Input.GetMouseButtonDown(0))
        {
            OnImageClick();
        }
    }

    void OnImageClick()
    {
        // 현재 이미지 인덱스가 배열 범위를 초과하지 않도록 체크
        if (currentImageIndex >= images.Length)
        {
            return; // 배열 범위를 초과한 경우, 메서드 종료
        }

        // 현재 이미지 비활성화
        images[currentImageIndex].gameObject.SetActive(false);

        // 다음 이미지로 이동
        currentImageIndex++;

        // 다음 이미지가 있다면 활성화, 없다면 캔버스 비활성화
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
