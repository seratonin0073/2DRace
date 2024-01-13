using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CarController : MonoBehaviour
{
    public WheelJoint2D frontWheel;//переднє колесо
    public WheelJoint2D backWheel;//заднє колесо
    public TMP_Text gasText;//текст який відображає паливо

    [HideInInspector] public bool moveForward = false;
    [HideInInspector] public bool moveBackward = false;

    private JointMotor2D motor;//об'єкт мотора
    private float speed = 0f;//поточна швидкість мотору
    private bool onGrounded = true;//чи на землі машинка
    private Rigidbody2D rb2d;//об'єкт фізики
    private int gas = 100;//поточна кількість палива


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//передаємо посилання на об'єкт фізики
        motor.maxMotorTorque = 10000;//максимальна сила яка може бути прикладена до колеса
        StartCoroutine(GasReducer());//викликаємо корутину GasReducer
    }

    void FixedUpdate()
    {
        MoveOnGround();//керування на землі
        if (!onGrounded)//якщо не на землі
        {
            MoveInAir();//керування в повітрі
        }

        CheckGameOver();


    }

    /*void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))//якщо стрілка вправо натиснута
        {
            moveForward = true;//дозволяємо рух вперед
            moveBackward = false;//забороняємо рух назад
        }
        else if(Input.GetKey(KeyCode.LeftArrow))//якщо стрілка вліво натиснута
        {
            moveForward = false;//забороняємо рух вперед
            moveBackward = true;//дозволяємо рух назад
        }
        else
        {
            moveForward = false;//забороняємо рух вперед
            moveBackward = false;//забороняємо рух назад
        }

        //якщо іншого шару(землі) токрається хоча б одна колесо машини
        if(frontWheel.GetComponent<Collider2D>().IsTouchingLayers()||
            backWheel.GetComponent<Collider2D>().IsTouchingLayers())
        {
            onGrounded = true;//ми на землі
        }
        else//інакше
        {
            onGrounded = false;//ми не на землі
        }
    }*/

    private void MoveOnGround()
    {
        if (moveForward)//якщо можна рухатись вперед
        {//якщо кутова швидкість прикладеного Rigidbody2D до WheelJoint2D більша за -2000
            if (frontWheel.attachedRigidbody.angularVelocity > -2000)
            {
                speed += 40;//збільшуємо поточну швидкість на 40
                motor.motorSpeed = speed;//передаємо поточну швидкість до мотора
            }
            frontWheel.motor = motor;//підв'язуємо мотор до переднього колеса
            backWheel.motor = motor;//підв'язуємо мотор до заднього колеса колеса
            frontWheel.useMotor = true;//переднє колесо використовує мотор
            backWheel.useMotor = true;//заднє колесо використовує мотор
        }
        else if (moveBackward)//якщо можна рухатись назад
        {//якщо кутова швидкість прикладеного Rigidbody2D до WheelJoint2D менше за 2000
            if (frontWheel.attachedRigidbody.angularVelocity < 2000)
            {
                speed -= 40;//зменшуємо поточну швідкість на 40
                motor.motorSpeed = speed;//передаємо поточну швидкість до мотора
            }
            frontWheel.motor = motor;//підв'язуємо мотор до переднього колеса
            backWheel.motor = motor;//підв'язуємо мотор до заднього колеса колеса
            frontWheel.useMotor = true;//переднє колесо використовує мотор
            backWheel.useMotor = true;//заднє колесо використовує мотор
        }
        else//інакше
        {
            speed = -frontWheel.attachedRigidbody.angularVelocity;
            frontWheel.useMotor = false;//вимикаємо мотор на передньому колесі
            backWheel.useMotor = false;//вимикаємо мотор на задньому колесі
        }
    }

    //метод руху в повітрі
    private void MoveInAir()
    {
        frontWheel.useMotor = false;//вимикаємо мотор на передньому колесі
        backWheel.useMotor = false;//вимикаємо мотор на задньому колесі
        if (moveForward)//якщо рухаємось вперед
        {//якщо кутова швидкість менша за 200
            if(rb2d.angularVelocity < 200)
            {
                rb2d.AddTorque(10f);//прикладаємо кутову силу на 10
            }
        }
        else if(moveBackward)//якщо рухаємось назад
        {//інакше якщо кутова швидкість більша за -200
            if (rb2d.angularVelocity > -200)
            {
                rb2d.AddTorque(-10f);//прикладаємо кутову силу на -10
            }
        }
    }

    private void CheckGameOver()
    {
        Vector2 dir = transform.up;//задаэмо напрямок променя
        //посилаємо промінь з певної точки в певному напрямку та на певну дистанцію
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, 0.7f);
        //зображаємо промінь
        Debug.DrawRay(transform.position, dir * 0.7f, Color.green);
        //якщо об'єктів які перетнув більше ніж 1
        if(hit.Length > 1)
        {
            GameOver();//закінчити гру
        }
    }
    private void GameOver()//метод закінчення гри
    {
        SceneManager.LoadScene(0);//завантажуємо сцену під номером 0
    }

    //коли об'єкт торкається трігера іншого об'єкта
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //якщо об'єкт до якого ми торкнулись має тег DeadZone
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            GameOver();//кінець гри
        }

        //якщо торкаємось каністри палива
        if (collision.gameObject.CompareTag("GasRefresher"))
        {
            gas = 100;//заливаємо бак на повну
            Destroy(collision.gameObject);//знищуємо каністру
        }

        //якщо торкнулися фінішу
        if (collision.gameObject.CompareTag("Finish"))
        {
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel");//запам'ятаємо збережену в пам'яті сцену
            int playedLevel = SceneManager.GetActiveScene().buildIndex;//запам'ятовуємо номер поточної сцени
            if(playedLevel >= savedLevel)//якщо номер поточної більше ніж номер збереженої
            {
                PlayerPrefs.SetInt("CurrentLevel", playedLevel + 1);//зберігаємо нове значення збереженого рівня
            }
            SceneManager.LoadScene("Menu");//завантажуємо меню
        }
    }

    //створюємо корутину
    private IEnumerator GasReducer()
    {
        while(gas > 0)//поки є паливо
        {
            gas--;//віднімаємо одиницю від палива
            gasText.text = gas.ToString();//виводимо на екран
            yield return new WaitForSeconds(0.5f);//чекаємо пів секунди
        }
        GameOver();//кінець гри
    }

}
