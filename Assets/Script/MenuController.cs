using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Transform levelButtons;//об'посилоання на об'єкт де знаходяться кнопки рівнів
    private int currentLevel;//зберігає номер максимального поточного рівня

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");//отримуємо з пам'яті збережене значення
        if(currentLevel == 0)//якщо номер рівня дорівнює 0
        {
            currentLevel = 1;//номеру рівня присвоюємо 1
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);//зберігаємо в пам'ять нове значення
        }

        /* для ітератора "і" який на початку = 0 виконувати тіло циклу допоки ітератор менше за
         * значення поточного рівня. Збільшувати ітератор після кожної ітерації на 1
         */
        for (int i = 0; i < currentLevel; i++/*i = i + 1*/)
        {
            //робимо кнопку під певним індексом інтерактивною
            levelButtons.GetChild(i).GetComponent<Button>().interactable = true;
        }
        
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
