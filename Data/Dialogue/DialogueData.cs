using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class DialogueData : Resource
{
    [Export]
    public string npcName;
    [Export]
    public Texture2D npcIcon;
    //声明对话内容的数组，每个元素表示一行对话
    [Export(PropertyHint.MultilineText)]
    public Array<string> dialogueLines;

}