using System;
using System.Collections.Generic;
using Godot;

class GameNew : Node2D{

    [Signal]
    public delegate void char_pressed();
    [Signal]
    public delegate void finished();


    private Color COLOR_GREEN = new Color(0, 0.75f, 0.1f);
    private Color COLOR_WHITE = new Color(1.0f, 1.0f, 1.0f);

    private GameLabel gameLabel;
    private Label nextToGameLabel;
    private CodeLabel codeLabel;
    private Label scoreLabel;
    private Clock clock;
    private TextEdit textbox;

    private int totalClicks = 0;
    private int correctClicks = 0;
    private int linesTyped = 0;
    private int score = 0;
    private float scoreMultiplier = 1.0f;

    public bool isActive = true;

    // when loaded
    public override void _Ready(){
        gameLabel = GetNode<GameLabel>("GameLabel");
        nextToGameLabel = GetNode<Label>("NextToGameLabel");
        codeLabel = GetNode<CodeLabel>("CodeLabel");
        scoreLabel = GetNode<Label>("Score");
        clock = GetNode<Clock>("Clock");
        textbox = GetNode<TextEdit>("TextEdit");

        textbox.Connect("text_changed", this, "TextboxChanged");
        clock.Connect("finished", this, nameof(OnClockFinished));

        Restart();
    }

    // called every frame
    public override void _Process(float delta){
        // delta -> time elapsed since last frame

        textbox.GrabFocus(); // textbox is always active
        if (Input.IsActionJustPressed("enter")) Enter(); // handle enter
        // change the '>' color accourdingly
        nextToGameLabel.AddColorOverride("font_color", (gameLabel.Text.Empty())?COLOR_GREEN:COLOR_WHITE);
        // display score
        scoreLabel.Text = $"Kewlness Score: {score}{System.Environment.NewLine}Multiplier: {(int)scoreMultiplier}";
    }

    // basically when a character is pressed
    private void TextboxChanged(){
        if (isActive){
            clock.countDown = true;
            totalClicks++;

            // grab character typed
            char c = textbox.Text[0];
            textbox.Text = string.Empty;
            if ((int)c == 10) // it's enter
            return;

            if (!gameLabel.Text.Empty() && gameLabel.Feed(gameLabel.Text[0])){ // the character is correct lol
                score += 2 * (int)scoreMultiplier;
                scoreMultiplier += 0.2f;
                correctClicks++;
            }else{
                score -= 5 * (int)scoreMultiplier;
                scoreMultiplier = 0;
            }
        }
        EmitSignal(nameof(char_pressed));
    }

    // enter pressed
    private void Enter(){
        // totalClicks already increased in "TextboxChanged"

        if (gameLabel.Text.Empty()){
            // adds the line to the code label
            codeLabel.AddLine(gameLabel.GetCurrentLineString() + System.Environment.NewLine);
            codeLabel.RefreshText();
            gameLabel.NextLine();
            
            score += 50*(int)scoreMultiplier;
            scoreMultiplier += 1.2f;
            correctClicks++;
            linesTyped++;
        }else{
            int charactersNotTyped = gameLabel.Text.Length;
            score -= 20*(int)scoreMultiplier;
            scoreMultiplier = 1.0f;
        }
    }
    
    // when the clock is done.
    private void OnClockFinished(){
        EmitSignal(nameof(finished));
        isActive = false;
    }
    // get the stats
    public Dictionary<String, float> GetStats(){
        Dictionary<String, float> stats = new Dictionary<String, float>();
        stats.Add("score", score);
        stats.Add("accuracy", (float)correctClicks/(float)totalClicks);
        stats.Add("lines typed", linesTyped);
        return stats;
    }

    // returns the sourcecode from a file
    public static string LoadSourceCodeString(string path){
        string str;
        try{ str = System.IO.File.ReadAllText(path);}
        catch(Exception e){GD.Print(e); str = "exception thrown!"; }
        return str;
    }
    // restart the game
    public void Restart(){
        totalClicks = 0;
        correctClicks = 0;
        linesTyped = 0;
        score = 0;
        scoreMultiplier = 1.0f;
        isActive = true;
        codeLabel.Restart();
        gameLabel.Restart();
        clock.Restart();
    }

    public GameLabel GetGameLabel(){ return gameLabel; }
}