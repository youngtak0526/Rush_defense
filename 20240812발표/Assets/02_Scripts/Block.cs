using UnityEngine;

public class Block : MonoBehaviour
{
    private int hitCount = 0; // ����� ���� Ƚ��
    public int hitsToDestroy = 2; // ����� ������� ���� �ʿ��� ���� Ƚ��

    public void HitByPlayer()
    {
        hitCount++;
        if (hitCount >= hitsToDestroy)
        {
            Destroy(gameObject); // ��� ����
            Debug.Log("����� ��������ϴ�.");
        }
        else
        {
            Debug.Log("����� �������ϴ�. ���� Ƚ��: " + hitCount);
        }
    }
}
