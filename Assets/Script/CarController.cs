using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontWheel;//������ ������
    public WheelJoint2D backWheel;//���� ������

    private JointMotor2D motor;//��'��� ������

    void Start()
    {
        motor.maxMotorTorque = 10000;//����������� ���� ��� ���� ���� ���������� �� ������
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
