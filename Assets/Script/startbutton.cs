using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class startbutton : MonoBehaviour
{
    public void gamestart() //���� ���۾����� ��ŸƮ ��ư������ 1��° ������ �̵��ϱ�
    {
        SceneManager.LoadScene("1.part0");
    }
}
