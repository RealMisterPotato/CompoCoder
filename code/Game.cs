using System;
using Godot;

class Game : Node2D{

    [Signal]
    public delegate void char_pressed();


    private Color COLOR_GREEN = new Color(0, 0.75f, 0.1f);
    private Color COLOR_WHITE = new Color(1.0f, 1.0f, 1.0f);

    private GameLabel gameLabel;
    private Label nextToGameLabel;
    private CodeLabel codeLabel;
    private Label scoreLabel;
    private Clock clock;
    private TextEdit textbox;
    private AnimatedSprite space;

    private int score = 0;
    private float scoreMultiplier = 1.0f;

    public override void _Ready(){
        // when loaded
        gameLabel = GetNode<GameLabel>("GameLabel");
        nextToGameLabel = GetNode<Label>("NextToGameLabel");
        codeLabel = GetNode<CodeLabel>("CodeLabel");
        scoreLabel = GetNode<Label>("Score");
        clock = GetNode<Clock>("Clock");
        textbox = GetNode<TextEdit>("TextEdit");

        textbox.Connect("text_changed", this, "TextboxChanged");
        clock.countDown = true;

        codeLabel.Text = "";
    }

    public override void _Process(float delta){
        // called every frame
        // delta -> time elapsed since last frame

        textbox.GrabFocus(); // textbox is always active
        if (Input.IsActionJustPressed("enter")) Enter(); // handle enter
        // change the '>' color accourdingly
        nextToGameLabel.AddColorOverride("font_color", (gameLabel.Text.Empty())?COLOR_GREEN:COLOR_WHITE);
        // display score
        scoreLabel.Text = $"Kewlness Score: {score}{System.Environment.NewLine}Multiplier: {(int)scoreMultiplier}";
    }

    private void TextboxChanged(){
        // basically when a character is pressed

        // grab character typed
        char c = textbox.Text[0];
        textbox.Text = string.Empty;
        if ((int)c == 10) // it's enter
            return;

        EmitSignal(nameof(char_pressed));

        if (gameLabel.Feed(c)){ // is the character correct?
            score += 20 * (int)scoreMultiplier;
            scoreMultiplier += 0.2f;
        }else{
            score -= 10 * (int)scoreMultiplier;
            scoreMultiplier = 1.0f;
        }
    }
    private void Enter(){
        // enter pressed

        if (gameLabel.Text.Empty()){
            // adds the line to the code label
            codeLabel.AddLine(GameLabel.StringNoCommentsAndWhiteSpaces(gameLabel.GetCurrentLineString()) + System.Environment.NewLine);
            codeLabel.RefreshText();
            gameLabel.NextLine();
            
            score += 50*(int)scoreMultiplier;
            scoreMultiplier += 1.2f;
        }else{
            int charactersNotTyped = gameLabel.Text.Length;
            score -= 20*(int)scoreMultiplier;
            scoreMultiplier = 1.0f;
        }
    }

    public static string LoadSourceCodeString(string path){
        // returns the sourcecode from a file
        string str;
        try{ str = System.IO.File.ReadAllText(path);}
        catch(Exception e){GD.Print(e); str = "exception thrown!"; }
        return str;
    }

    public GameLabel GetGameLabel(){ return gameLabel; }
}