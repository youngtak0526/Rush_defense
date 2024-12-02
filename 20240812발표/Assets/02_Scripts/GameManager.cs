using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int turnCount = 0;
    private static GameManager instance;

    public int playerHP = 100; // �÷��̾��� HP
    private PlayerMove player; // PlayerMove ��ũ��Ʈ ����

    void Awake()
    {
        // Game Manager �ν��Ͻ��� �ϳ��� �ִ��� Ȯ��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public void IncrementTurn()
    {
        turnCount++;
        Debug.Log("���� ��: " + turnCount);
    }

    public int GetTurnCount()
    {
        return turnCount;
    }

    public void RegisterPlayer(PlayerMove player)
    {
        this.player = player;
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        if (playerHP <= 0)
        {
            playerHP = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾ ����߽��ϴ�.");
        if (player != null)
        {
            Destroy(player.gameObject); // �÷��̾� ������Ʈ ����
        }
    }

    public void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Bullet")) // ��¥ �Ѿ˰� �浹���� ��
        {
            TakeDamage(100); // HP�� 0���� �����Ͽ� �÷��̾� ��� ó��
            Destroy(collidedObject); // �Ѿ� ����
        }
        else if (collidedObject.CompareTag("FakeBullet")) // ��¥ �Ѿ˰� �浹���� ��
        {
            // ��¥ �Ѿ˰��� �浹�� �÷��̾��� HP�� ������ ���� ����
            Destroy(collidedObject); // ��¥ �Ѿ˸� ����
        }
    }
}
