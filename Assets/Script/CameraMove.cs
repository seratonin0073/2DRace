using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;//���� �� ���� ����� ��������
    public float speed = 10f;//�������� ���������� ������
    void Update()
    {
        Vector3 tpos = target.position;//�����'������� ������� ���
        tpos.z = -10;// �������� ������� ������ �� z
        transform.position = Vector3.Lerp(transform.position, tpos, Time.deltaTime * speed);//������� ���� ��������
    }
}
