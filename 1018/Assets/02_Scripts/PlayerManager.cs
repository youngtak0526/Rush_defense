using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    // 안전한 싱글톤 인스턴스
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager 인스턴스가 없습니다!");
            }
            return _instance;
        }
    }

    [Header("플레이어 설정")]
    public GameObject playerPrefab; // 리스폰 시 사용할 플레이어 프리팹
    public static Vector3 respawnPoint; // 리스폰 위치
    public int maxHealth = 100;
    public static int playerHP;

    [Header("이동 설정")]
    public float moveDistance = 1f; // 한 번에 이동할 거리
    public float moveSpeed = 5f; // 이동 속도

    [Header("UI 설정")]
    public RawImage[] deathImages;
    public Canvas deathCanvas;
    public float fadeDuration = 0.5f;
    public float respawnDelay = 3.0f;

    private Vector3 targetPosition; // 이동 목표 위치
    private bool isMoving = false;  // 이동 중인지 여부
    private bool isFirstDeath = true;

    private Animator animator;

    public static event Action OnPlayerMoveComplete; // 이동 완료 이벤트

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    private void Start()
    {
        playerHP = maxHealth;
        animator = GetComponent<Animator>();
        targetPosition = transform.position;

        if (respawnPoint == Vector3.zero)
            respawnPoint = transform.position; // 초기 리스폰 위치 설정
    }

    private void Update()
    {
        if (!isMoving && playerHP > 0) // 사망하지 않았을 때만 이동 입력 처리
        {
            HandleMovementInput();
        }

        if (playerHP <= 0) // 체력이 0 이하이면 사망 처리
        {
            Die();
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void HandleMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.back);
        else if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right);
    }

    private void Move(Vector3 direction)
    {
        targetPosition = transform.position + direction * moveDistance;
        transform.rotation = Quaternion.LookRotation(direction);

        if (!CheckCollisionAtTargetPosition())
        {
            isMoving = true;
            GameManager.Instance.IncrementTurn();
        }
    }

    private void MoveTowardsTarget()
    {
        animator.SetBool("IsRun", true);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            if (!CheckCollisionAtTargetPosition())
            {
                transform.position = targetPosition;
                isMoving = false;

                animator.SetBool("IsRun", false);
                OnPlayerMoveComplete?.Invoke();
            }
        }
    }

    private bool CheckCollisionAtTargetPosition()
    {
        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("TransparentBlock"))
                return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block?.HitByPlayer();
        }

        GameManager.Instance.HandleCollision(collision.gameObject);
    }

    public static void TakeDamage(int damage)
    {
        playerHP -= damage;
    }

    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");
        if (isFirstDeath)
        {
            StartCoroutine(ShowDeathImagesAndRespawn());
            isFirstDeath = false;
        }
        else
        {
            HandleDeath();
            Respawn();
        }
        GameManager.Instance.ResetTurn();
    }

    private IEnumerator ShowDeathImagesAndRespawn()
    {
        deathCanvas.gameObject.SetActive(true);

        foreach (RawImage img in deathImages)
        {
            img.gameObject.SetActive(true); // 이미지 활성화
            img.canvasRenderer.SetAlpha(0f); // 알파값 초기화

            // 이미지 페이드인
            yield return FadeIn(img);

            // 페이드인 완료 후 일정 시간 대기
            yield return new WaitForSeconds(1f);

            // 이미지 페이드아웃
            yield return FadeOut(img);

            img.gameObject.SetActive(false); // 페이드아웃 후 비활성화
        }

        deathCanvas.gameObject.SetActive(false); // 캔버스 숨기기
        HandleDeath(); // 사망 처리

        // 리스폰 대기 시간 후 리스폰
        yield return new WaitForSeconds(respawnDelay);
        Respawn();

        GameManager.Instance.ResetTurn();
    }


    private IEnumerator FadeIn(RawImage image)
    {
        image.CrossFadeAlpha(1f, fadeDuration, false); // 페이드인
        yield return new WaitForSeconds(fadeDuration); // 페이드인 완료까지 대기
    }

    private IEnumerator FadeOut(RawImage image)
    {
        image.CrossFadeAlpha(0f, fadeDuration, false); // 페이드아웃
        yield return new WaitForSeconds(fadeDuration); // 페이드아웃 완료까지 대기
    }


    private void HandleDeath()
    {
        Destroy(gameObject);
    }

    public void Respawn()
    {
        Debug.Log("Player Respawned!");
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, Quaternion.identity);
        PlayerManager playerController = newPlayer.GetComponent<PlayerManager>();
        playerController.animator = newPlayer.GetComponent<Animator>();
        playerHP = maxHealth;
    }
}
