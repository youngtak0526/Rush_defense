using TMPro; // �� ���ӽ����̽��� �߰��ؾ� �մϴ�.
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    public TextMeshProUGUI startText;  // TMP UI ������Ʈ�� ���� �ʵ�

    void Start()
    {
        startText.gameObject.SetActive(true);  // �ؽ�Ʈ Ȱ��ȭ
    }
}
