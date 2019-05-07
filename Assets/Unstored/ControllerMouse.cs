using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMouse : ControllerDefault {

    public int button = 0;
    public override bool IsPressing()
    {
        return Input.GetMouseButton(button);
    }
}
