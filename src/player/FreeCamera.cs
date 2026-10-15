using Godot;

namespace EconSim.player;

public partial class FreeCamera : Camera2D
{
    private static double cameraSpeed = 100;
    
    public FreeCamera(Node2D parent)
    {
        parent.AddChild(this);
        this.MakeCurrent();
    }

    public override void _PhysicsProcess(double delta)
    {
        handleMovement(delta);
    }

    private void handleMovement(double delta)
    {
        Vector2 direction = Vector2.Zero;
        float distance = (float)(cameraSpeed * delta);
        
        if (Input.IsActionPressed("east"))
            direction.X += distance;
        if (Input.IsActionPressed("west"))
            direction.X -= distance;
        if (Input.IsActionPressed("south"))
            direction.Y += distance;
        if (Input.IsActionPressed("north"))
            direction.Y -= distance;
        
        this.SetPosition(this.GetPosition() + direction);
    }
}