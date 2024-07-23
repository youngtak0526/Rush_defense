using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;

    private Player thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
    }
    void OnTriggerEnter2D(Collider2D collision)//충돌되었을 때
    {
        if (collision.gameObject.name == "Player") //오브젝트의 이름이 플레이어라면
        {
            thePlayer.currentMapName = transferMapName;
            SceneManager.LoadScene(transferMapName); //파트 0으로 이동하기
        }

    }
    
}
