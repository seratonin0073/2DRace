using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Transform levelButtons;//��'���������� �� ��'��� �� ����������� ������ ����
    private int currentLevel;//������ ����� ������������� ��������� ����

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");//�������� � ���'�� ��������� ��������
        if(currentLevel == 0)//���� ����� ���� ������� 0
        {
            currentLevel = 1;//������ ���� ���������� 1
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);//�������� � ���'��� ���� ��������
        }

        /* ��� ��������� "�" ���� �� ������� = 0 ���������� ��� ����� ������ �������� ����� ��
         * �������� ��������� ����. ���������� �������� ���� ����� �������� �� 1
         */
        for (int i = 0; i < currentLevel; i++/*i = i + 1*/)
        {
            //������ ������ �� ������ �������� �������������
            levelButtons.GetChild(i).GetComponent<Button>().interactable = true;
        }
        
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
