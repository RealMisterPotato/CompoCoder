using System; // wew
// wew
using System.Collections.Generic;
using Godot;

class GameLabel : Label{

    public List<int> newLineSpots = new List<int>(); // spots to put a new line so the code will look the same
    private string[] lines;
    public int activeLine;

    public override void _Ready(){
        // when loaded
        LoadSourceCode("code/GameLabel.cs");
        if (lines != null && lines.Length >= 1) UpdateToLine(0);
    }
    public override void _Process(float delta) {
        // happens every frame
    }
    public void LoadSourceCode(string path){
        // load source code from path
        String sourceString = Game.LoadSourceCodeString(path); 
        lines = sourceString.Split(System.Environment.NewLine);
    }

    public int LineCount(){ return (lines != null)?lines.Length:0;}
    public String GetCurrentLineString(){ return lines[activeLine]; }
    public static String StringNoCommentsAndWhiteSpaces(string line){ 
        // returns the string without the comments or leading whitespaces (for code)
        if (line.Empty())
            return "";
        bool foundChar = false;
        for (int i = 1; i < line.Length; i++){
            if (!foundChar && !line[i].Equals(' ')){ // remove leading whitespaces
                    line = line.Remove(0, i-1);
                    foundChar = true;
            }
            if (foundChar && line[i].Equals('/') && line[i-1].Equals('/')){ // remove comments
                line = line.Remove(i-1, line.Length - i + 1);
                break;
            }
        }

        return line; 
    }
    public void UpdateToLine(int line){
        if (lines == null) return;
        activeLine = line % LineCount(); // so the lines will cycle if
        this.Text = StringNoCommentsAndWhiteSpaces(lines[activeLine]);
    }
    public void NextLine(){
        activeLine++;
        UpdateToLine(activeLine);
    }
    public bool Feed(char c){
        // checks if the character provided is the same as the first one in the text
        if (this.Text.Length == 0)
            return false;
        if (this.Text[0].Equals(c)){
            this.Text = this.Text.Remove(0, 1);
            return true;
        }
        return false;
    }
    // gets the percent of the characters typed
    public float PercentTyped(){ return 1.0f-((float)this.Text.Length / (float)lines[activeLine].Length); }
}