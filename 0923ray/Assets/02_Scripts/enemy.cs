using UnityEngine;

public class enemy : MonoBehaviour
{
    RaycastHit hit;
    float MaxDistance = 30;
    public GameObject player; // 플레이어 오브젝트를 참조

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 0.3f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            Debug.Log("DDD");
            // 플레이어가 이동 중인지 확인
            if (player.GetComponent<Rigidbody>().velocity.magnitude > 0)
            {
                // 플레이어가 이동 중이면 y축으로 +2 이동
                Vector3 newPosition = player.transform.position;
                newPosition.z += 2;
                player.transform.position = newPosition;

                Debug.Log("Player moved up by 2 units!");
            }
        }
    }
}
