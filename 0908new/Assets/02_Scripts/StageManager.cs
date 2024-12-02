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

    private void FixedUpdate()
    {
        if (shouldFollow && !hasFollowed)
        {
            Vector3 currentCameraPosition = mainCamera.transform.position;

            Vector3 targetPosition = new Vector3(character.transform.position.x+ 7.189f + offset.x, currentCameraPosition.y, currentCameraPosition.z);

            mainCamera.transform.position = Vector3.MoveTowards(currentCameraPosition, targetPosition, 15 * Time.deltaTime);

            // �� �� �ʱ�ȭ
            // ī�޶� ��ǥ ��ġ�� ���� �����ߴ��� Ȯ��
            Debug.Log("Distance to target: " + Vector3.Distance(mainCamera.transform.position, targetPosition));
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 9)
            {
                // �� �ʱ�ȭ�� �� ���� �����ϰ�, ���� �� �̻� �������� �ʵ��� ����
                GameManager.Instance.ResetTurn();
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

            // ���� ��ġ�� üũ����Ʈ�� ����
            //GameManager.Instance.SetSpawnPosition(character.transform.position);
            //Debug.Log("������");
            // �׷� ��ü�� ����
            // �浹�� ������Ʈ�� ��ġ�� ���ο� ������ ����Ʈ�� ����
            spawn.respawnPoint = other.transform.position;
            Debug.Log("Respawn point updated!");
            Destroy(obstaclesGroup);
           // GetComponent<Collider>().enabled = false;
        }
    }

}
