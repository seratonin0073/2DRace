using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontWheel;//������ ������
    public WheelJoint2D backWheel;//���� ������

    private JointMotor2D motor;//��'��� ������
    private bool moveForward = false;//�� ����� �������� ������
    private bool moveBackward = false;//�� ����� �������� �����
    private float speed = 0f;//������� �������� ������
    private bool onGrounded = true;//�� �� ���� �������
    private Rigidbody2D rb2d;//��'��� ������


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//�������� ��������� �� ��'��� ������
        motor.maxMotorTorque = 10000;//����������� ���� ��� ���� ���� ���������� �� ������
    }

    void FixedUpdate()
    {
        if(onGrounded)//���� �� ����
        {
            MoveOnGround();//��������� �� ����
        }
        else//������
        {
            MoveInAir();//��������� � �����
        }
          
    }

    void Update()
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
    }

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
}
