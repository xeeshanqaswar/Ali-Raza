using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventsHandler;

public class EventsHandler : MonoBehaviour
{
    public delegate void EnableOption(List<int> options);
    public static EnableOption _onEnableOption;
    public static void CallOnEnableOption(List<int> options)
    {
        _onEnableOption?.Invoke(options);
    }

    public delegate void DisableOption();
    public static DisableOption _onDisableOption;
    public static void CallOnDisableOption()
    {
        _onDisableOption?.Invoke();
    }
}
