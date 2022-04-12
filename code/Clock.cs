using Godot;
using System;

public class Clock : Label {
    [Signal]
    public delegate void Finished();

    public int minutesPassed = 2880; // 48*60
    public bool countDown = false; // should the clock count down?
    private float tickSpeed = 0.0625f; // 1/16
    public override void _Ready() {
        updateText(minutesPassed);
    }

    private float deltaCount = 0;
    public override void _Process(float delta) {
        if (countDown){
            // update the clock
            updateText(minutesPassed);
            deltaCount += delta;
            if (deltaCount >= tickSpeed){
                minutesPassed--;
                deltaCount = 0;
            }
            if (minutesPassed < 0)
                EmitSignal(nameof(Finished));
        }
        
    }

    public void updateText(int minutesPassed){
        // update the text accourding to the minutes passed.
        int hours = minutesPassed/60;
        int minutes = minutesPassed%60;
        string hoursZero = (hours<10)?"0":"";
        string minutesZero = (minutes<10)?"0":"";
        Text = $"{hoursZero + hours}:{minutesZero + minutes}"; 
    }

    public void Dance(){
        // do a cool animation
    }
}
