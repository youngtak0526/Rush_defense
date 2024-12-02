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
        // ��ǥ ��ġ ��� (ĳ���� ��ġ + ������)
        Vector3 targetPosition = character.transform.position + offset;

        // ī�޶� ��ġ�� ��ǥ ��ġ�� �ε巴�� �̵�
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPosition, 15 * Time.deltaTime);

        // ĳ���͸� �ٶ󺸴� �������� ȸ���ϵ��� ����
        Quaternion targetRotation = Quaternion.LookRotation(character.transform.position - mainCamera.transform.position);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, 5 * Time.deltaTime);

        // ī�޶� ��ǥ ��ġ�� ���� �����ߴ��� Ȯ��
        Debug.Log("Distance to target: " + Vector3.Distance(mainCamera.transform.position, targetPosition));
        if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.1f)
        {
            GameManager.Instance.ResetTurn();
            hasFollowed = true;
            shouldFollow = false;
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
