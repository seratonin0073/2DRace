using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontWheel;//переднє колесо
    public WheelJoint2D backWheel;//заднє колесо

    private JointMotor2D motor;//об'єкт мотора
    private bool moveForward = false;//чи можна рухатись вперед
    private bool moveBackward = false;//чи можна рухатись назад
    private float speed = 0f;//поточна швидкість мотору
    private bool onGrounded = true;//чи на землі машинка
    private Rigidbody2D rb2d;//об'єкт фізики


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//передаємо посилання на об'єкт фізики
        motor.maxMotorTorque = 10000;//максимальна сила яка може бути прикладена до колеса
    }

    void FixedUpdate()
    {
        if(onGrounded)//якщо на землі
        {
            MoveOnGround();//керування на землі
        }
        else//інакше
        {
            MoveInAir();//керування в повітрі
        }
          
    }

    void Update()
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
    }

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
}
