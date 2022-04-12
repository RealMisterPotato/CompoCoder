using System;
using Godot;

class Desktop : Node2D{

    private Game game;
    private Sprite computer;
    private Sprite enter;
    private Sprite rightHand;
    private Sprite leftHand;
    private Vector2 RIGHTHAND_DEFAULT_POSITION;
    private Vector2 LEFTHAND_MAXIMUM_POSITION;
    private Random randomizer;

    // when loaded
    public override void _Ready() {
        game = GetNode<Game>("Game");
        computer = GetNode<Sprite>("Computer");
        enter = GetNode<Sprite>("Enter");
        rightHand = GetNode<Sprite>("RightHand");
        leftHand = GetNode<Sprite>("LeftHand");
        RIGHTHAND_DEFAULT_POSITION = new Vector2(840, 570);
        LEFTHAND_MAXIMUM_POSITION = new Vector2(-118, 76);

        game.Connect("char_pressed", this, nameof(RightHandClick));
        randomizer = new Random();
    }
    // happens every Frame
    public override void _Process(float delta) {
        
        if (Input.IsActionJustPressed("enter")) Enter();

    }

    private Vector2[] typingBounds = {new Vector2(326, 563), new Vector2(725, 660)};

    // stuff to do when typing
    private void RightHandClick(){
        float randomPositionX = (float)randomizer.Next((int)typingBounds[0].x, (int)typingBounds[1].x);
        float randomPositionY = (float)randomizer.Next((int)typingBounds[0].y, (int)typingBounds[1].y);
        rightHand.Position = new Vector2(randomPositionX, randomPositionY);
    }
    // move right hand to default position
    private void RightHandReturn(){

    }
    // stuff to do when pressing enter
    private void Enter(){
        leftHand.Position = new Vector2(leftHand.Position.x, 460);
        // keep the right hand from "typing" when pressing enter
        rightHand.Position = RIGHTHAND_DEFAULT_POSITION;
    }
}