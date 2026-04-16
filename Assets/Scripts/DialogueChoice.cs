using System;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    public string Id;
    public string text;
    public DialogueNode nextNode;
}