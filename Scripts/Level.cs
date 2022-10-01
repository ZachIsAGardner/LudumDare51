using System;
using Godot;

public class Level : Node
{
    public event Action OnComplete;

    public void Complete()
    {
        if (OnComplete != null) OnComplete();
    }
}