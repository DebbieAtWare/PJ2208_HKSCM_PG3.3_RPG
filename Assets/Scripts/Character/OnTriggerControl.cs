using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerControl : MonoBehaviour
{
    public delegate void OnTriggerEnter();
    public OnTriggerEnter onTriggerEnterCallback;

    public delegate void OnTriggerExit();
    public OnTriggerExit onTriggerExitCallback;

    //Check if player enters trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("OnTriggerEnter2D");
            if (onTriggerEnterCallback != null)
            {
                onTriggerEnterCallback.Invoke();
            }
        }
    }

    //Check if player exits trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("OnTriggerExit2D");
            if (onTriggerExitCallback != null)
            {
                onTriggerExitCallback.Invoke();
            }
        }
    }
}
