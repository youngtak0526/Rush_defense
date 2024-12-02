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

    void Update()
    {
        if (shouldFollow && !hasFollowed)
        {
            // 카메라가 플레이어를 따라오도록 설정
            Vector3 targetPosition = character.transform.position + offset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 2);
            GameManager.Instance.ResetTurn();  // 턴 수 초기화
            // 카메라가 목표 위치에 거의 도달했는지 확인

            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.1f)
            {
                // 턴 초기화를 한 번만 실행하고, 이후 더 이상 실행하지 않도록 설정
                hasFollowed = true;               // 한 번만 따라오도록 설정
                shouldFollow = false;             // 카메라 추적 중지
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
