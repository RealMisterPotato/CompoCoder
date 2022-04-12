using Godot;
using System;

public class Player : Node2D
{
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(float delta)
    {
        if (!animationPlayer.IsPlaying())
            animationPlayer.Play("not_typing");
    }

    public void SetTypingAnimation(){
        animationPlayer.Play("typing");
    }
}
