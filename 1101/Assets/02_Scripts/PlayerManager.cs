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
    public Button UpButton;
    public Button DownButton;
    public Button LeftButton;
    public Button RightButton;

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

        UpButton.onClick.AddListener(MoveUp);
        DownButton.onClick.AddListener(MoveDown);
        LeftButton.onClick.AddListener(MoveLeft);
        RightButton.onClick.AddListener(MoveRight);
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
    public void MoveUp()
    {
        Move(Vector3.forward);
    }
    public void MoveDown()
    {
        Move(Vector3.down);
    }
    public void MoveLeft()
    {
        Move(Vector3.left);
    }
    public void MoveRight()
    {
        Move(Vector3.right);
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
        }
        else
        {
            HandleDeath();
            Respawn();
            GameManager.Instance.ResetTurn();
        }
    }

    IEnumerator ShowDeathImagesAndRespawn()
    {
        isFirstDeath = false;
        deathCanvas.gameObject.SetActive(true); // ĵ���� Ȱ��ȭ

        foreach (RawImage img in deathImages)
        {
            img.gameObject.SetActive(false); // �̹��� �ʱ�ȭ
        }
        Debug.Log("�÷��̾ ����߽��ϴ�.��������");
        // �̹��� ���������� �����ֱ�
        for (int i = 0; i < deathImages.Length; i++)
        {
            RawImage currentImage = deathImages[i];
            Debug.Log("�÷��̾ ����߽��ϴ�.123313232");
            currentImage.gameObject.SetActive(true); // �̹��� Ȱ��ȭ
            currentImage.canvasRenderer.SetAlpha(0f); // �����ϰ� ����
            StartCoroutine(FadeIn(currentImage)); // ���̵���
            Debug.Log("�÷��̾ ����߽��ϴ�.1111111");
            Debug.Log(deathImages.Length);
            //yield return new WaitForSeconds(1f - fadeDuration); // ���� �ð�

           // StartCoroutine(FadeOut(currentImage)); // ���̵�ƿ�
            Debug.Log("�÷��̾ ����߽��ϴ�.2222");
            //yield return new WaitForSeconds(1f);
            Debug.Log("�÷��̾ ����߽��ϴ�.@@@@@@@@@@@@@@@@");

            currentImage.gameObject.SetActive(false); // ��Ȱ��ȭ
        }

        deathCanvas.gameObject.SetActive(false); // ĵ���� �����
        HandleDeath(); // ��� ó��

        yield return new WaitForSeconds(respawnDelay); // ������ ���
        Respawn(); // ������ ó��
        GameManager.Instance.ResetTurn(); // �� �ʱ�ȭ
    }

    private IEnumerator FadeIn(RawImage image)
    {
        image.CrossFadeAlpha(1f, fadeDuration, false); // ���̵���
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeOut(RawImage image)
    {
        image.CrossFadeAlpha(0f, fadeDuration, false); // ���̵�ƿ�
        yield return new WaitForSeconds(fadeDuration);
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
