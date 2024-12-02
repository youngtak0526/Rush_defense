using TMPro; // 이 네임스페이스를 추가해야 합니다.
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    public TextMeshProUGUI startText;  // TMP UI 오브젝트를 위한 필드

    void Start()
    {
        startText.gameObject.SetActive(true);  // 텍스트 활성화
    }
}
