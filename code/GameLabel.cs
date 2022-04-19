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
        Restart();
    }
    public override void _Process(float delta) {
        // happens every frame
    }
    public void LoadSourceCode(string path){
        // load source code from path
        String sourceString = GameNew.LoadSourceCodeString(path); 
        lines = sourceString.Split(System.Environment.NewLine);
    }

    public int LineCount(){ return (lines != null)?lines.Length:0;}
    public String GetCurrentLineString(){ return lines[activeLine]; }
    public void UpdateToLine(int line){
        if (lines == null) return;
        activeLine = line % LineCount(); // so the lines will cycle if reached "the end"
        this.Text = GetCurrentLineString();
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
    public float PercentTyped(){ 
        float dividend = (float)this.Text.Length;
        float divisor = ((float)lines[activeLine].Length == 0)?1:(float)lines[activeLine].Length; 
        return 1.0f - (dividend/divisor);
    }

    // restart
    public void Restart(){
    newLineSpots = new List<int>(); // spots to put a new line so the code will look the same
    activeLine = 0;
    LoadSourceCode("code/GameLabel.cs");
    if (lines != null && lines.Length >= 1) UpdateToLine(0);
    }
}