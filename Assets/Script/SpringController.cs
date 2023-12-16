using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    public Sprite springUP;
    public Sprite springDown;
    public float springForce = 100;

    private GameObject target;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isActive)
        {
            if(collision.CompareTag("Wheel"))
            {
                target = collision.transform.parent.gameObject;
                StartCoroutine(SpringForce());
            }
            else if(collision.CompareTag("Car"))
            {
                target = collision.gameObject;
                StartCoroutine(SpringForce());
            }
        }
    }

    IEnumerator SpringForce()
    {
        isActive = true;
        yield return new WaitForSeconds(0.1f);
        target.GetComponent<Rigidbody2D>().AddForce(Vector2.up * springForce, 
                                                    ForceMode2D.Impulse);
        GetComponent<SpriteRenderer>().sprite = springUP;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = springDown;
        isActive = false;
        target = null;
    }
}
