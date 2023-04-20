using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class lastDeviceUsed : MonoBehaviour
{
    [SerializeField] private Sprite controller;
    [SerializeField] private Sprite KM;
    private Image self;
    public static lastDeviceUsed Instance;

    private void Awake()
    {
        Instance = this;
        self = GetComponent<Image>();
    }

    private void Update()
    {
        if (currentDevice().Equals("keyboard"))
        {
            self.sprite = KM;
        }
        else if (currentDevice().Equals("gamepad"))
        {
            self.sprite = controller;
        }
    }

    public string currentDevice()
    {
        if(Gamepad.current != null)
        {
            double a = Keyboard.current.lastUpdateTime;
            double b = Mouse.current.lastUpdateTime;
            double c = Gamepad.current.lastUpdateTime;

            if (a > c || b > c)
            {
                return "keyboard";
            }
            else if (c > a && c > b)
            {
                return "gamepad";
            }
            else
            {
                return "other device connected";
            }
        }
        else
        {
            return "keyboard";
        }
        
    }

}
