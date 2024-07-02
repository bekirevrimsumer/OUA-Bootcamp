using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoMessageSO", menuName = "Scriptable Objects/Info Message", order = 1)]
public class InfoMessageSO : ScriptableObject
{
    public string Title;
    [TextArea(3, 10)]
    public string Message;
}
