using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UturnButton : buttonBase
{
    public override void buttonAction()
    {
        _playerMove.FowardDir *= -1;
        isBreak = true;
    }
}
