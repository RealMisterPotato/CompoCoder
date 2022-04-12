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
    private Clock clock;
    private TextEdit textbox;
    private AnimatedSprite space;

    private int score = 0; // times typed character correctly in a row
    public override void _Ready(){
        // when loaded
        gameLabel = GetNode<GameLabel>("GameLabel");
        nextToGameLabel = GetNode<Label>("NextToGameLabel");
        codeLabel = GetNode<CodeLabel>("CodeLabel");
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
        // clock.Dance();
    }

    private void TextboxChanged(){
        // basically when a character is pressed
        EmitSignal(nameof(char_pressed));

        char c = textbox.Text[0];
        textbox.Text = string.Empty;

        if (gameLabel.Feed(c)) // is the character correct?
            score++;

        if (gameLabel.Text.Empty())
            nextToGameLabel.AddColorOverride("font_color", COLOR_GREEN); // change to green

    }
    private void Enter(){
        // enter pressed

        if (gameLabel.Text.Empty()){
            nextToGameLabel.AddColorOverride("font_color", COLOR_WHITE); // change to white
            // adds the line to the code label
            codeLabel.AddLine(gameLabel.GetCurrentLineString() + System.Environment.NewLine);
            codeLabel.RefreshText();
            gameLabel.NextLine();
            
            score += 5;
        }else{
            int charactersNotTyped = gameLabel.Text.Length;
            score -= charactersNotTyped;
        }
    }

    public static string LoadSourceCodeString(string path){
        // returns the sourcecode from a file
        string str;
        try{ str = System.IO.File.ReadAllText(path);}
        catch(Exception e){GD.Print(e); str = "exception thrown!"; }
        return str;
    }
}