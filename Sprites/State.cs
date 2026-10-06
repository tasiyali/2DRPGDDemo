using Godot;
using System;

public partial class State : Node
{
    public Fsm fsm;

    public virtual void EnterState()
    {
    }

    public virtual void UpdateState(double delta)
    {
    }
    
    public virtual void ExitState()
    {

    }
}
