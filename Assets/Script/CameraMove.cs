using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;//ціль за якою треба слідувати
    public float speed = 10f;//швидкість переміщення камери
    void Update()
    {
        Vector3 tpos = target.position;//запам'ятовуємо позицію цілі
        tpos.z = -10;// зберігаємо позицію камери по z
        transform.position = Vector3.Lerp(transform.position, tpos, Time.deltaTime * speed);//вказуємо куди рухатись
    }
}
