using System;
using System.Collections.Generic;
using Godot;

class Bluescreen : Node2D{
    private Label title;
    private Label stats;
    // when loaded
    public override void _Ready() {
        title = GetNode<Label>("Title");
        stats = GetNode<Label>("Stats");
        // start the animation
        AnimationPlayer animation = GetNode<AnimationPlayer>("Title/AnimationPlayer");
        animation.Play("start");
    }
    // happens every frame
    public override void _Process(float delta){

    }
    // set the stats
    public void SetStats(Dictionary<String, float> stats){
        string newLine = System.Environment.NewLine; // the newline character(s)
        this.stats.Text = String.Format("Lines typed: {0}" + newLine + "Accuracy: {1}%" + newLine + newLine + "SCORE: {2}",
                                stats["lines typed"],
                                stats["accuracy"]*100,
                                stats["score"]);
    }
}