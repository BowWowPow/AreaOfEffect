using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool on;
    public GameObject _obj;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        on = false;
        _obj.SetActive(false);
    }

    
    public void Switch()
    {
        if (on)
        {
            on = false;
            StartCoroutine("TurnOff");
            _obj.SetActive(false);
        }
        else
        {
            on = true;
            _obj.SetActive(true);
            StartCoroutine("TurnOn");

        }
    }


    IEnumerator TurnOn()
    {

        _obj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, speed);
        yield return new WaitForSecondsRealtime(speed);
    }

    IEnumerator TurnOff()
    {
        _obj.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, speed);
        yield return new WaitForSecondsRealtime(speed);
    }

}