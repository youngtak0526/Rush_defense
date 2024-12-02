using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private int turnCount = 0;
    private static GameManager instance;



    


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
    public void ResetTurn()
    {
        turnCount = 0;
        Debug.Log("�� �� �ʱ�ȭ");
        Debug.Log("���� ��: " + turnCount);
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





   
    /// �Ѿ��� �浹��
    public void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Bullet")) // ��¥ �Ѿ˰� �浹���� ��
        {
            PlayerManager.TakeDamage(100); // HP�� 0���� �����Ͽ� �÷��̾� ��� ó��
            Destroy(collidedObject); // �Ѿ� ����
        }
        else if (collidedObject.CompareTag("FakeBullet")) // ��¥ �Ѿ˰� �浹���� ��
        {
            // ��¥ �Ѿ˰��� �浹�� �÷��̾��� HP�� ������ ���� ����
            Destroy(collidedObject); // ��¥ �Ѿ˸� ����
        }
    }
}