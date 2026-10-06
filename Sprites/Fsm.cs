using Godot;
using System;

public partial class Fsm : Node
{
    // 定义信号
    [Signal]
    public delegate void OnStateTransitionedEventHandler(string stateName);

    [Export]
    public NodePath InitialState {get; private set;}

    public State currentState;

    public override async void _Ready()
    {
        //等待所有者就绪
        await ToSignal(Owner, Node.SignalName.Ready);
        //遍历所有子节点，将它们的fsm属性设置为当前的Fsm实例
        foreach (State state in GetChildren())
            state.fsm = this;
        //设置初始状态
        currentState = GetNode<State>(InitialState);
        //调用初始状态的EnterState方法
        currentState.EnterState();
    }
    //切换状态
    public void ChangeState(string stateName)
    {
        if(!HasNode(stateName))
            return;

        //调用当前状态的ExitState方法
        currentState.ExitState();
        //切换到新状态
        currentState = GetNode<State>(stateName);
        //调用新状态的EnterState方法
        currentState.EnterState();
        //发射状态切换信号
        EmitSignal(SignalName.OnStateTransitioned, currentState.Name);
    }
}
