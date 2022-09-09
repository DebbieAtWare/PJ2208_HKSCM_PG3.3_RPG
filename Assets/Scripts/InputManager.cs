using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [Header("Vertical")]
    public bool canInput_Vertical;
    public int vertical;
    public delegate void OnValueChanged_Vertical(int val);
    public OnValueChanged_Vertical onValueChanged_VerticalCallback;

    [Header("Horizontal")]
    public bool canInput_Horizontal;
    public int horizontal;
    public delegate void OnValueChanged_Horizontal(int val);
    public OnValueChanged_Horizontal onValueChanged_HorizontalCallback;

    [Header("Confirm")]
    public bool canInput_Confirm;
    public delegate void OnValueChanged_Confirm();
    public OnValueChanged_Confirm onValueChanged_ConfirmCallback;

    bool isAxisInUse_Vertical = false;
    bool isAxisInUse_Horizontal = false;
    bool isAxisInUse_Confirm = false;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("InputManager Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (canInput_Vertical)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (!isAxisInUse_Vertical)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        vertical = 1;
                        if (onValueChanged_VerticalCallback != null)
                        {
                            onValueChanged_VerticalCallback.Invoke(vertical);
                        }
                    }
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        vertical = -1;
                        if (onValueChanged_VerticalCallback != null)
                        {
                            onValueChanged_VerticalCallback.Invoke(vertical);
                        }
                    }
                    isAxisInUse_Vertical = true;
                }
            }
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                isAxisInUse_Vertical = false;
                vertical = 0;
            }
        }


        //-----

        if (canInput_Horizontal)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (!isAxisInUse_Horizontal)
                {
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        horizontal = 1;
                        if (onValueChanged_HorizontalCallback != null)
                        {
                            onValueChanged_HorizontalCallback.Invoke(horizontal);
                        }
                    }
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        horizontal = -1;
                        if (onValueChanged_HorizontalCallback != null)
                        {
                            onValueChanged_HorizontalCallback.Invoke(horizontal);
                        }
                    }
                    isAxisInUse_Horizontal = true;
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                isAxisInUse_Horizontal = false;
                horizontal = 0;
            }
        }


        //-----

        if (canInput_Confirm)
        {
            if (Input.GetAxisRaw("RPGConfirmPC") != 0)
            {
                if (!isAxisInUse_Confirm)
                {
                    if (Input.GetAxisRaw("RPGConfirmPC") > 0)
                    {
                        if (onValueChanged_ConfirmCallback != null)
                        {
                            onValueChanged_ConfirmCallback.Invoke();
                        }
                    }
                    isAxisInUse_Confirm = true;
                }
            }
            if (Input.GetAxisRaw("RPGConfirmPC") == 0)
            {
                isAxisInUse_Confirm = false;
            }
        }

    }
}
