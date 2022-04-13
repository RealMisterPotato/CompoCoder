using System;
using Godot;

class Desktop : Node2D{

    private Game game;
    private Sprite computer;
    private AnimatedSprite enter;
    private Sprite rightHand;
    private Sprite leftHand;
    private Vector2 rightHandDesiredPosition;
    private Vector2 leftHandDesiredPosition;
    private float rightHandSpeed;
    private float leftHandSpeed;
    private Vector2 RIGHTHAND_DEFAULT_POSITION;
    private Vector2 LEFTHAND_MAXIMUM_POSITION;
    private Random randomizer;

    // when loaded
    public override void _Ready() {
        game = GetNode<Game>("Game");
        computer = GetNode<Sprite>("Computer");
        enter = GetNode<AnimatedSprite>("Enter");
        rightHand = GetNode<Sprite>("RightHand");
        leftHand = GetNode<Sprite>("LeftHand");
        RIGHTHAND_DEFAULT_POSITION = new Vector2(527, 722);
        LEFTHAND_MAXIMUM_POSITION = new Vector2(-118, 76);
        rightHandDesiredPosition = RIGHTHAND_DEFAULT_POSITION;
        leftHandDesiredPosition = LEFTHAND_MAXIMUM_POSITION;
        rightHandSpeed = 1000.0f;
        leftHandSpeed = 1000.0f;

        game.Connect("char_pressed", this, nameof(RightHandClick));
        randomizer = new Random();
    }
    // happens every Frame
    public override void _Process(float delta) {
        
        MoveTowards(rightHand, rightHandDesiredPosition, rightHandSpeed, delta);
        MoveTowards(leftHand, leftHandDesiredPosition, leftHandSpeed, delta);

        if (Input.IsActionJustPressed("enter")) Enter();
    }

    private Vector2[] typingBounds = {new Vector2(444, 561), new Vector2(624, 618)};

    // stuff to do when typing
    private void RightHandClick(){
        float randomPositionX = (float)randomizer.Next((int)typingBounds[0].x, (int)typingBounds[1].x);
        float randomPositionY = (float)randomizer.Next((int)typingBounds[0].y, (int)typingBounds[1].y);
        rightHand.Position = new Vector2(randomPositionX, randomPositionY);
        RightHandReturn();
        LeftHandCharge();
    }
    // move right hand to default position
    private void RightHandReturn(){
        rightHandDesiredPosition = RIGHTHAND_DEFAULT_POSITION;
    }
    // make the left hand change it's height accourding to the percentage of the completed characters
    private Vector2[] chargingBounds = {new Vector2(-180, 81), new Vector2(-84, 371)};
    private void LeftHandCharge(){
        float x = randomizer.Next((int)chargingBounds[0].x, (int)chargingBounds[1].x);
        float y = chargingBounds[1].y - (chargingBounds[1].y - chargingBounds[0].y)*(1.0f * game.GetGameLabel().PercentTyped());
        leftHandDesiredPosition = new Vector2(x, y);
    }
    // stuff to do when pressing enter
    private void Enter(){
        leftHandDesiredPosition = new Vector2(leftHand.Position.x, 460);
        enter.Frame = 0;
        enter.Play();
    }
    private void MoveTowards(Node2D node, Vector2 position, float speed, float delta){
        // move a node to a target position

        // add some randomness
        float random = (float)randomizer.NextDouble() * 2;
        
        double y = position.y - node.Position.y;
        double x = position.x - node.Position.x;
        double angle = Math.Atan2(y, x);
        
        node.Position += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * random * speed * delta;
    }
}