using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class startbutton : MonoBehaviour
{
    public void gamestart() //게임 시작씬에서 스타트 버튼누르면 1번째 씬으로 이동하기
    {
        SceneManager.LoadScene("1.part0");
    }
}
