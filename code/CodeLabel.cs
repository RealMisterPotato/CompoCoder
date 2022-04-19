using System;
using Godot;

class CodeLabel : Label{

    private const int MAX_LINES = 10;
    private string[] lines = new string[MAX_LINES];
    public override void _Ready(){
        // when loaded
        this.MaxLinesVisible = MAX_LINES;
    }

    public void RefreshText(){
        this.Text = "";
        foreach(string line in lines){
            if (line == null) this.Text += System.Environment.NewLine;
            this.Text += line;
        }
    }
    public void AddLine(string str){
        // adds a line

        for (int i = 0; i < MAX_LINES-1; i++){
            // offset lines
            if (lines[i+1] != null)
                lines[i] = lines[i+1];
        }

        lines[MAX_LINES-1] = str;
    }

    public void Restart(){
        lines = new string[MAX_LINES];
        RefreshText();
    }
}