using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    // ������ �̱��� �ν��Ͻ�
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager �ν��Ͻ��� �����ϴ�!");
            }
            return _instance;
        }
    }

    [Header("�÷��̾� ����")]
    public GameObject playerPrefab; // ������ �� ����� �÷��̾� ������
    public static Vector3 respawnPoint; // ������ ��ġ
    public int maxHealth = 100;
    public static int playerHP;

    [Header("�̵� ����")]
    public float moveDistance = 1f; // �� ���� �̵��� �Ÿ�
    public float moveSpeed = 5f; // �̵� �ӵ�

    [Header("UI ����")]
    public RawImage[] deathImages;
    public Canvas deathCanvas;
    public float fadeDuration = 0.5f;
    public float respawnDelay = 3.0f;

    private Vector3 targetPosition; // �̵� ��ǥ ��ġ
    private bool isMoving = false;  // �̵� ������ ����
    private bool isFirstDeath = true;

    private Animator animator;

    public static event Action OnPlayerMoveComplete; // �̵� �Ϸ� �̺�Ʈ

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
        }
    }

    private void Start()
    {
        playerHP = maxHealth;
        animator = GetComponent<Animator>();
        targetPosition = transform.position;

        if (respawnPoint == Vector3.zero)
            respawnPoint = transform.position; // �ʱ� ������ ��ġ ����
    }

    private void Update()
    {
        if (!isMoving && playerHP > 0) // ������� �ʾ��� ���� �̵� �Է� ó��
        {
            HandleMovementInput();
        }

        if (playerHP <= 0) // ü���� 0 �����̸� ��� ó��
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
        Debug.Log("�÷��̾ ����߽��ϴ�.");
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
            img.gameObject.SetActive(true); // �̹��� Ȱ��ȭ
            img.canvasRenderer.SetAlpha(0f); // ���İ� �ʱ�ȭ

            // �̹��� ���̵���
            yield return FadeIn(img);

            // ���̵��� �Ϸ� �� ���� �ð� ���
            yield return new WaitForSeconds(1f);

            // �̹��� ���̵�ƿ�
            yield return FadeOut(img);

            img.gameObject.SetActive(false); // ���̵�ƿ� �� ��Ȱ��ȭ
        }

        deathCanvas.gameObject.SetActive(false); // ĵ���� �����
        HandleDeath(); // ��� ó��

        // ������ ��� �ð� �� ������
        yield return new WaitForSeconds(respawnDelay);
        Respawn();

        GameManager.Instance.ResetTurn();
    }


    private IEnumerator FadeIn(RawImage image)
    {
        image.CrossFadeAlpha(1f, fadeDuration, false); // ���̵���
        yield return new WaitForSeconds(fadeDuration); // ���̵��� �Ϸ���� ���
    }

    private IEnumerator FadeOut(RawImage image)
    {
        image.CrossFadeAlpha(0f, fadeDuration, false); // ���̵�ƿ�
        yield return new WaitForSeconds(fadeDuration); // ���̵�ƿ� �Ϸ���� ���
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
