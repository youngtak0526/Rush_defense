using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int turnCount = 0;
    private static GameManager instance;

    public int playerHP = 100; // 플레이어의 HP
    private PlayerMove player; // PlayerMove 스크립트 참조

    void Awake()
    {
        // Game Manager 인스턴스가 하나만 있는지 확인
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
        Debug.Log("현재 턴: " + turnCount);
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
        Debug.Log("플레이어가 사망했습니다.");
        if (player != null)
        {
            Destroy(player.gameObject); // 플레이어 오브젝트 제거
        }
    }

    public void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Bullet")) // 진짜 총알과 충돌했을 때
        {
            TakeDamage(100); // HP를 0으로 설정하여 플레이어 사망 처리
            Destroy(collidedObject); // 총알 제거
        }
        else if (collidedObject.CompareTag("FakeBullet")) // 가짜 총알과 충돌했을 때
        {
            // 가짜 총알과의 충돌은 플레이어의 HP에 영향을 주지 않음
            Destroy(collidedObject); // 가짜 총알만 제거
        }
    }
}
