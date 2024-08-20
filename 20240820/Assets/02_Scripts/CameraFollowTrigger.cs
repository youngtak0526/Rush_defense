using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{
    public GameObject character;        // 플레이어 오브젝트
    public Camera mainCamera;        // 메인 카메라
    public Vector3 offset;           // 카메라의 플레이어와의 거리 오프셋
    private bool shouldFollow = false;

    public GameObject obstaclesGroup;  // 제거할 오브젝트 그룹
    // public string obstacleTag = "Obstacle";  // 태그로 식별할 경우 이 줄을 사용

    void Start()
    {
        // 초기 카메라 위치 설정
        offset = mainCamera.transform.position - character.transform.position;
    }

    void Update()
    {
        if (shouldFollow)
        {
            // 카메라가 플레이어를 따라오도록 설정
            Vector3 targetPosition = character.transform.position + offset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 2);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == character)
        {
            // 플레이어가 트리거 영역에 들어가면 따라오도록 설정
            shouldFollow = true;

            // 그룹 전체를 제거
            Destroy(obstaclesGroup);

            // 태그로 식별된 오브젝트들 제거
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
            // 플레이어가 트리거 영역을 벗어나면 따라오지 않도록 설정
            shouldFollow = false;
        }
    }
}
