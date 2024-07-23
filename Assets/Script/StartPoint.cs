using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint; // �� ��ȯ�� �÷��̾� ��ġ
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
