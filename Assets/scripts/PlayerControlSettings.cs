using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlSettings : MonoBehaviour
{

    public Joystick fixedJoystick;

    public Joystick notFixedJoystick;

    // Start is called before the first frame update
    void Start()
    {
        switch (CurrentData.instance.joystickFixed)
        {
            case true:
                Destroy(notFixedJoystick.gameObject);
                break;
            case false:
                Destroy(fixedJoystick.gameObject);
                break;
        }
    }
}
