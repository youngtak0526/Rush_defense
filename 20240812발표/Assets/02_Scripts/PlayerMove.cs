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
        GameManager.Instance.RegisterPlayer(this); // GameManager에 플레이어 등록
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if (block != null)
            {
                block.HitByPlayer(); // 블록이 플레이어에 의해 밟혔음을 알림
            }
        }

        GameManager.Instance.HandleCollision(collision.gameObject); // 기타 충돌 처리 (총알 등)
    }

    void Move(Vector3 direction)
    {
        targetPosition += direction * moveDistance;
        isMoving = true;
        GameManager.Instance.IncrementTurn(); // GameManager를 통해 턴 수 증가
    }
}
