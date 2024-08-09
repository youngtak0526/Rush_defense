using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    // 전환할 씬의 이름
    public string sceneToLoad = "Stage";

    // 매 프레임마다 호출
    void Update()
    {
        // 터치 또는 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0))
        {
            LoadScene();
        }
    }

    // 씬 로드 메서드
    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
