using System;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public static spawn Instance;  // �̱��� �ν��Ͻ�

    public static Vector3 respawnPoint; // ��Ȱ ��ġ

    public GameObject Player;

    public int maxHealth = 100;  // �ִ� ü��
//private int currentHealth;   // ���� ü��
    void Awake()
    {
        // �̱��� �ν��Ͻ� �Ҵ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �� �ı����� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� ������ �ڽ��� �ı�
        }
    }
    void Start()
    {
        PlayerMove.playerHP = maxHealth;  // ������ �� ü���� 100���� ����
        respawnPoint = transform.position; // ���� ��ġ�� ��Ȱ �������� ����
    }
    // �÷��̾ ������ ��ġ�� �̵���Ű�� �Լ�
    public void Respawn()
    {
        //Instantiate(Player, respawnPoint, Quaternion.identity);
        PlayerMove.playerHP = maxHealth;  // ü���� 100���� ����
        Debug.Log("Player Respawned!");
    }
}
