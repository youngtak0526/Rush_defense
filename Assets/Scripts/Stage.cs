using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    // ��ȯ�� ���� �̸�
    public string sceneToLoad = "Stage";

    // �� �����Ӹ��� ȣ��
    void Update()
    {
        // ��ġ �Ǵ� ���콺 Ŭ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            LoadScene();
        }
    }

    // �� �ε� �޼���
    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
