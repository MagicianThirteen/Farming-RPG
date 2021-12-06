
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNudge : MonoBehaviour
{
    private WaitForSeconds pause;

    private bool isRoate = false;

    private void Awake()
    {
        pause = new WaitForSeconds(0.04f);
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        //判断当前进来的物体在该物体的左还是右，左顺时针，右逆时针
        if (isRoate == false)
        {
            if (other.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                StartCoroutine(RotateClock());
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isRoate == false)
        {
            if (other.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                StartCoroutine(RotateClock());
            }
        }
    }

    IEnumerator RotateClock()
    {
        isRoate = true;
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.Rotate(0,0,-2f);
            yield return pause;
        }
        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Rotate(0,0,2f);
            yield return pause;
        }
        gameObject.transform.Rotate(0,0,-2);
        yield return pause;
        isRoate = false;
    }

    IEnumerator RotateAntiClock()
    {
        isRoate = true;
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.Rotate(0,0,2);
            yield return pause;
        }
        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Rotate(0,0,-2);
            yield return pause;
        }
        gameObject.transform.Rotate(0,0,2);
        yield return pause;
        isRoate = false;
    }
}
