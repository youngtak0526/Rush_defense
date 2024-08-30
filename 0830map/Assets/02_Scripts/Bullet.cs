using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    public float bulletSpeed = 10f; // 총알 속도

    private bool hasFired = false; // 총알이 발사되었는지 여부


    void Update()
    {
        int turnCount = GameManager.Instance.GetTurnCount(); // GameManager에서 턴 수 가져오기

        if (turnCount % 3 == 0 && turnCount != 0 && !hasFired)
        {
            Fire();
            hasFired = true;
        }

        // 다음 홀수 턴에서 다시 발사 가능하도록 설정
        if (turnCount % 3 != 0)
        {
            hasFired = false;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange); // 총알의 속도를 설정
            Debug.Log("총알 발사!");
        }
        else
        {
            Debug.LogError("발사된 총알에 Rigidbody가 없습니다.");
        }

        // 총알에 BulletMovement 컴포넌트 추가하여 Z축 이동 감시
        BulletMovement bulletMovement = bullet.AddComponent<BulletMovement>();
        bulletMovement.SetInitialPosition(bullet.transform.position);
    }
}

public class BulletMovement : MonoBehaviour
{
    private Vector3 initialPosition;

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }

    void Update()
    {
        // 총알이 Z축을 기준으로 2.5 이상 이동하면 총알 파괴
        if (Mathf.Abs(transform.position.z - initialPosition.z) >= 2.5f)
        {
            Destroy(gameObject);
            Debug.Log("총알이 Z축 2.5만큼 이동하여 파괴됨");
        } // 총알이 Y축을 기준으로 2.5 이상 이동하면 총알 파괴
        else if(Mathf.Abs(transform.position.x - initialPosition.x) >= 2.5f)
        {

            Destroy(gameObject);
            Debug.Log("총알이 X축 2.5만큼 이동하여 파괴됨");
        }
    }
}