using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject character;        // 플레이어 오브젝트
    public Camera mainCamera;           // 메인 카메라
    public Vector3 offset;              // 카메라의 플레이어와의 거리 오프셋
    private bool shouldFollow = false;  // 카메라가 따라와야 하는지 여부
    private bool hasFollowed = false;   // 카메라가 이미 따라왔는지 여부를 추적



    public GameObject obstaclesGroup;   // 제거할 오브젝트 그룹


    void Start()
    {
        // 초기 카메라 위치 설정
        offset = mainCamera.transform.position - character.transform.position;
    }

 private void FixedUpdate()
{
    if (shouldFollow && !hasFollowed)
    {
        // 목표 위치 계산 (캐릭터 위치 + 오프셋)
        Vector3 targetPosition = character.transform.position + offset;

        // 카메라 위치를 목표 위치로 부드럽게 이동
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPosition, 15 * Time.deltaTime);

        // 캐릭터를 바라보는 방향으로 회전하도록 설정
        Quaternion targetRotation = Quaternion.LookRotation(character.transform.position - mainCamera.transform.position);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, 5 * Time.deltaTime);

        // 카메라가 목표 위치에 거의 도달했는지 확인
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
            // 플레이어가 트리거 영역에 들어가면 한 번만 따라오도록 설정
            shouldFollow = true;

            // 그룹 전체를 제거

            Destroy(obstaclesGroup);
           // GetComponent<Collider>().enabled = false;
        }
    }

}
