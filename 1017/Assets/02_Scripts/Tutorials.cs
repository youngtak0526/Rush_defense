using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
    public Text tutorialText; // Ʃ�丮�� �ȳ��� ������ UI �ؽ�Ʈ
    private int stepIndex = 0;
    private bool playerMoved = false;

    // �÷��̾� �̵� �Ϸ� �̺�Ʈ ����
    private void OnEnable()
    {
        PlayerManager.OnPlayerMoveComplete += PlayerMoved;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerMoveComplete -= PlayerMoved;
    }

    void Start()
    {
        StartTutorial(); // Ʃ�丮�� ����
    }

    void Update()
    {
        // �÷��̾ �������� �ʾ��� ���� �Է��� ����
        if (!playerMoved)
        {
            switch (stepIndex)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        tutorialText.text = "���ƿ�! ���� S Ű�� �ڷ� �̵��غ�����.";
                        stepIndex++;
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        tutorialText.text = "���߾��! ���� A Ű�� �������� �̵��غ�����.";
                        stepIndex++;
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        tutorialText.text = "�Ǹ��ؿ�! ���� D Ű�� ���������� �̵��غ�����.";
                        stepIndex++;
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        tutorialText.text = "�Ϻ��ؿ�! �̵� ����� ������.";
                        stepIndex++;
                        FinishTutorial();
                    }
                    break;
            }
        }
    }

    // Ʃ�丮�� ����
    void StartTutorial()
    {
        tutorialText.text = "W Ű�� ���� ������ �̵��ϼ���.";
    }

    // �÷��̾ �̵����� �� ȣ��Ǵ� �Լ�
    void PlayerMoved()
    {
        playerMoved = true; // �̵� �Ϸ� �� true ����

        // 0.5�� �� �ٽ� ������ ���� ���·� ����
        Invoke(nameof(ResetMoveFlag), 0.5f);
    }

    void ResetMoveFlag()
    {
        playerMoved = false; // �ٽ� �Է��� ���� �� �ֵ��� ����
    }

    void FinishTutorial()
    {
        tutorialText.text = "Ʃ�丮���� �Ϸ�Ǿ����ϴ�!";
    }
}
