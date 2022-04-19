using System;
using System.Collections.Generic;
using Godot;

class Bluescreen : Node2D{
    private Label title;
    private bool titleBlink = false; // should the title blink?
    private Label stats;
    private AnimationPlayer animation;
    private bool interactable = false;
    // when loaded
    public override void _Ready() {
        title = GetNode<Label>("Title");
        stats = GetNode<Label>("Stats");
        // start the animation
        animation = GetNode<AnimationPlayer>("AnimationPlayer");
        animation.Connect("animation_finished", this, nameof(AnimationFinished));
    }
    // happens every frame
    private float blinkDeltaCount = 0;
    public override void _Process(float delta){
        // make the title blink if needed
        if (titleBlink){ 
            blinkDeltaCount += delta;
            if (blinkDeltaCount >= 0.2f){
                title.Modulate = (title.Modulate.Equals(Godot.Colors.White))?Godot.Colors.Transparent:Godot.Colors.White; // alternate colors
                blinkDeltaCount = 0;
            }
        }
        // on enter change go to menu(?)
        if (interactable && Input.IsActionJustPressed("enter"))
            ((Desktop)GetParent()).RestartGame();

    }
    // set the stats and restart the animation
    public void SetStats(Dictionary<String, float> stats){
        // set stats
        string newLine = System.Environment.NewLine; // the newline character(s)
        this.stats.Text = String.Format("Lines typed: {0}" + newLine + "Accuracy: {1}%" + newLine + newLine + "SCORE: {2}",
                                stats["lines typed"],
                                stats["accuracy"]*100,
                                stats["score"]);
        // animation stuff
        animation.Play("start");

        interactable = false;
    }
    // when the AnimationPlayer finished the animation
    private void AnimationFinished(String animationName){
        titleBlink = true;
        interactable = true;
    }
}