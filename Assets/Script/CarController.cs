using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CarController : MonoBehaviour
{
    public WheelJoint2D frontWheel;//������ ������
    public WheelJoint2D backWheel;//���� ������
    public TMP_Text gasText;//����� ���� �������� ������

    [HideInInspector] public bool moveForward = false;
    [HideInInspector] public bool moveBackward = false;

    private JointMotor2D motor;//��'��� ������
    private float speed = 0f;//������� �������� ������
    private bool onGrounded = true;//�� �� ���� �������
    private Rigidbody2D rb2d;//��'��� ������
    private int gas = 100;//������� ������� ������


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//�������� ��������� �� ��'��� ������
        motor.maxMotorTorque = 10000;//����������� ���� ��� ���� ���� ���������� �� ������
        StartCoroutine(GasReducer());//��������� �������� GasReducer
    }

    void FixedUpdate()
    {
        MoveOnGround();//��������� �� ����
        if (!onGrounded)//���� �� �� ����
        {
            MoveInAir();//��������� � �����
        }

        CheckGameOver();


    }

    /*void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))//���� ������ ������ ���������
        {
            moveForward = true;//���������� ��� ������
            moveBackward = false;//����������� ��� �����
        }
        else if(Input.GetKey(KeyCode.LeftArrow))//���� ������ ���� ���������
        {
            moveForward = false;//����������� ��� ������
            moveBackward = true;//���������� ��� �����
        }
        else
        {
            moveForward = false;//����������� ��� ������
            moveBackward = false;//����������� ��� �����
        }

        //���� ������ ����(����) ��������� ���� � ���� ������ ������
        if(frontWheel.GetComponent<Collider2D>().IsTouchingLayers()||
            backWheel.GetComponent<Collider2D>().IsTouchingLayers())
        {
            onGrounded = true;//�� �� ����
        }
        else//������
        {
            onGrounded = false;//�� �� �� ����
        }
    }*/

    private void MoveOnGround()
    {
        if (moveForward)//���� ����� �������� ������
        {//���� ������ �������� ������������ Rigidbody2D �� WheelJoint2D ����� �� -2000
            if (frontWheel.attachedRigidbody.angularVelocity > -2000)
            {
                speed += 40;//�������� ������� �������� �� 40
                motor.motorSpeed = speed;//�������� ������� �������� �� ������
            }
            frontWheel.motor = motor;//���'����� ����� �� ���������� ������
            backWheel.motor = motor;//���'����� ����� �� �������� ������ ������
            frontWheel.useMotor = true;//������ ������ ����������� �����
            backWheel.useMotor = true;//���� ������ ����������� �����
        }
        else if (moveBackward)//���� ����� �������� �����
        {//���� ������ �������� ������������ Rigidbody2D �� WheelJoint2D ����� �� 2000
            if (frontWheel.attachedRigidbody.angularVelocity < 2000)
            {
                speed -= 40;//�������� ������� ������� �� 40
                motor.motorSpeed = speed;//�������� ������� �������� �� ������
            }
            frontWheel.motor = motor;//���'����� ����� �� ���������� ������
            backWheel.motor = motor;//���'����� ����� �� �������� ������ ������
            frontWheel.useMotor = true;//������ ������ ����������� �����
            backWheel.useMotor = true;//���� ������ ����������� �����
        }
        else//������
        {
            speed = -frontWheel.attachedRigidbody.angularVelocity;
            frontWheel.useMotor = false;//�������� ����� �� ���������� �����
            backWheel.useMotor = false;//�������� ����� �� �������� �����
        }
    }

    //����� ���� � �����
    private void MoveInAir()
    {
        frontWheel.useMotor = false;//�������� ����� �� ���������� �����
        backWheel.useMotor = false;//�������� ����� �� �������� �����
        if (moveForward)//���� �������� ������
        {//���� ������ �������� ����� �� 200
            if(rb2d.angularVelocity < 200)
            {
                rb2d.AddTorque(10f);//���������� ������ ���� �� 10
            }
        }
        else if(moveBackward)//���� �������� �����
        {//������ ���� ������ �������� ����� �� -200
            if (rb2d.angularVelocity > -200)
            {
                rb2d.AddTorque(-10f);//���������� ������ ���� �� -10
            }
        }
    }

    private void CheckGameOver()
    {
        Vector2 dir = transform.up;//������� �������� �������
        //�������� ������ � ����� ����� � ������� �������� �� �� ����� ���������
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, 0.7f);
        //��������� ������
        Debug.DrawRay(transform.position, dir * 0.7f, Color.green);
        //���� ��'���� �� �������� ����� �� 1
        if(hit.Length > 1)
        {
            GameOver();//�������� ���
        }
    }
    private void GameOver()//����� ��������� ���
    {
        SceneManager.LoadScene(0);//����������� ����� �� ������� 0
    }

    //���� ��'��� ��������� ������ ������ ��'����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� ��'��� �� ����� �� ���������� �� ��� DeadZone
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            GameOver();//����� ���
        }

        //���� ��������� ������� ������
        if (collision.gameObject.CompareTag("GasRefresher"))
        {
            gas = 100;//�������� ��� �� �����
            Destroy(collision.gameObject);//������� �������
        }

        //���� ���������� �����
        if (collision.gameObject.CompareTag("Finish"))
        {
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel");//�����'����� ��������� � ���'�� �����
            int playedLevel = SceneManager.GetActiveScene().buildIndex;//�����'������� ����� ������� �����
            if(playedLevel >= savedLevel)//���� ����� ������� ����� �� ����� ���������
            {
                PlayerPrefs.SetInt("CurrentLevel", playedLevel + 1);//�������� ���� �������� ����������� ����
            }
            SceneManager.LoadScene("Menu");//����������� ����
        }
    }

    //��������� ��������
    private IEnumerator GasReducer()
    {
        while(gas > 0)//���� � ������
        {
            gas--;//������� ������� �� ������
            gasText.text = gas.ToString();//�������� �� �����
            yield return new WaitForSeconds(0.5f);//������ �� �������
        }
        GameOver();//����� ���
    }

}
