using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{
    public GameObject character;        // �÷��̾� ������Ʈ
    public Camera mainCamera;        // ���� ī�޶�
    public Vector3 offset;           // ī�޶��� �÷��̾���� �Ÿ� ������
    private bool shouldFollow = false;

    public GameObject obstaclesGroup;  // ������ ������Ʈ �׷�
    // public string obstacleTag = "Obstacle";  // �±׷� �ĺ��� ��� �� ���� ���

    void Start()
    {
        // �ʱ� ī�޶� ��ġ ����
        offset = mainCamera.transform.position - character.transform.position;
    }

    void Update()
    {
        if (shouldFollow)
        {
            // ī�޶� �÷��̾ ��������� ����
            Vector3 targetPosition = character.transform.position + offset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 2);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == character)
        {
            // �÷��̾ Ʈ���� ������ ���� ��������� ����
            shouldFollow = true;

            // �׷� ��ü�� ����
            Destroy(obstaclesGroup);

            // �±׷� �ĺ��� ������Ʈ�� ����
            /*
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag(obstacleTag);
            foreach (GameObject obstacle in obstacles)
            {
                Destroy(obstacle);
            }
            */
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == character)
        {
            // �÷��̾ Ʈ���� ������ ����� ������� �ʵ��� ����
            shouldFollow = false;
        }
    }
}
