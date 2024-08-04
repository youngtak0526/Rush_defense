using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveDistance = 1f; // 한 번 이동할 거리
    public float moveSpeed = 5f; // 이동 속도

    private Vector3 targetPosition; // 이동할 목표 위치
    private bool isMoving = false; // 캐릭터가 이동 중인지 여부

    void Start()
    {
        targetPosition = transform.position; // 시작 위치를 현재 위치로 설정
    }

    void Update()
    {
        if (!isMoving) // 캐릭터가 이동 중이 아닐 때만 입력을 받음
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector3.right);
            }
        }

        // 캐릭터를 목표 위치로 부드럽게 이동
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false; // 이동 완료
            }
        }
    }

    void Move(Vector3 direction)
    {
        targetPosition += direction * moveDistance;
        isMoving = true;
    }
}
