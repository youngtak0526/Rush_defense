using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject character;        // �÷��̾� ������Ʈ
    public Camera mainCamera;           // ���� ī�޶�
    public Vector3 offset;              // ī�޶��� �÷��̾���� �Ÿ� ������
    private bool shouldFollow = false;  // ī�޶� ����;� �ϴ��� ����
    private bool hasFollowed = false;   // ī�޶� �̹� ����Դ��� ���θ� ����

    public GameObject obstaclesGroup;   // ������ ������Ʈ �׷�

    void Start()
    {
        // �ʱ� ī�޶� ��ġ ����
        offset = mainCamera.transform.position - character.transform.position;
    }

    void Update()
    {
        if (shouldFollow && !hasFollowed)
        {
            // ī�޶� �÷��̾ ��������� ����
            Vector3 targetPosition = character.transform.position + offset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 2);
            GameManager.Instance.ResetTurn();  // �� �� �ʱ�ȭ
            // ī�޶� ��ǥ ��ġ�� ���� �����ߴ��� Ȯ��

            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.1f)
            {
                // �� �ʱ�ȭ�� �� ���� �����ϰ�, ���� �� �̻� �������� �ʵ��� ����
                hasFollowed = true;               // �� ���� ��������� ����
                shouldFollow = false;             // ī�޶� ���� ����
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == character && !hasFollowed)
        {
            // �÷��̾ Ʈ���� ������ ���� �� ���� ��������� ����
            shouldFollow = true;

            // �׷� ��ü�� ����
            Destroy(obstaclesGroup);
           // GetComponent<Collider>().enabled = false;
        }
    }

}
