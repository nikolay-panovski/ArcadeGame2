using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player : AnimationSprite
{
    protected Collision coll_info = null;
    protected Vector2 direction = new Vector2(0, 0);
    public Pivot bullet_handler { get; set; }
    protected bool is_colliding;
    public int HP { get; set; }               // for HP
    //protected float bullet_cooldown;        // for shooting
    //protected float cooldown;               // for abilities
    //protected float shield_timer;           // for Energy Shield

    protected float x_speed = 1.8f;

    protected float velocity_y = 0.0f;

    public Player(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {

    }

    protected bool MoveAndCollide(float deltaX, float deltaY)   // returns true  if move is possible, false if landed on floor (can't move)
    {
        x += deltaX;
        y += deltaY;

        bool isColliding = false;
        foreach (Sprite other in GetCollisions())           // works with everything added as child to the Level
        {
                resolveCollision(this, other, deltaX, deltaY);
                isColliding = true;
        }

        return !isColliding;

    }

    private void resolveCollision(Sprite subject, Sprite collider, float deltaX, float deltaY)
    {
        if (collider is PickupCoin)
        {
            Console.WriteLine("coin destroyed!");
            collider.LateDestroy();
        }
        if (collider is PickupHeart)
        {
            collider.LateDestroy();
            HP++;           // -- for debug for going to end screen
            Console.WriteLine(HP);
        }
        if (collider is PickupShield)
        {
            Console.WriteLine("shield destroyed!");
            collider.LateDestroy();
        }

        else    // else is not any pickup we want special collision with -> treat as walls etc.
        {
            if (deltaX < 0) subject.x = collider.x + collider.width / 2 + subject.width / 2;  // refactored for origin at middle
            if (deltaX > 0) subject.x = collider.x - collider.width / 2 - subject.width / 2;
            if (deltaY < 0) subject.y = collider.y + collider.height / 2 + subject.height / 2;
            if (deltaY > 0) subject.y = collider.y - collider.height / 2 - subject.height / 2;
        }
    }

    /*protected void Update()
    {
        
    }*/
}