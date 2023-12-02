using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontWheel;//переднє колесо
    public WheelJoint2D backWheel;//заднє колесо

    private JointMotor2D motor;//об'єкт мотора

    void Start()
    {
        motor.maxMotorTorque = 10000;//максимальна сила яка може бути прикладена до колеса
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
