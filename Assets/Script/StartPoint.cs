using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint; // 맵 전환시 플레이어 위치
    private Player thePlayer;
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();

        if(startPoint == thePlayer.currentMapName)
        {
            thePlayer.transform.position = this.transform.position;
        }
    }
}
