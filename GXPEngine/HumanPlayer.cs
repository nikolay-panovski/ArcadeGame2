using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class HumanPlayer : Player
{
    public int ammo { get; set; }

    public AlienPlayer other_player { get; set; }

    public HumanPlayer(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, obj)
    {
        HP = 3;
        ammo = 12;
    }

    //---------------------------------------------------------------
    //                        MOVEMENT SECTION
    //---------------------------------------------------------------
    private void movementHandle()
    {
        if (Input.GetKey(Key.A)) coll_info = MoveUntilCollision(-x_speed, 0);
        // MoveAndCollide will work here with the updated resolveCollision, but the snapping from the sides still remains.
        if (Input.GetKey(Key.D)) coll_info = MoveUntilCollision(x_speed, 0);

    }

    private void JumpAndGravityHandle()
    {
        velocity_y += 0.08f;
        if (!MoveAndCollide(0f, velocity_y))
        {
            if (velocity_y > 0f && Input.GetKey(Key.W))
            {
                velocity_y = -4f;
            }
            else
            {
                velocity_y = 0f;        // player has landed
            }

        }
    }

    //---------------------------------------------------------------
    //                        BULLET SECTION
    //---------------------------------------------------------------
    private void GetDirectionVector()     // final version: only 2 shoot directions, left and right
    {
        if (Input.GetKey(Key.A))
        {
            direction.x = -1;
            if (!(Input.GetKey(Key.W)) && !(Input.GetKey(Key.S))) direction.y = 0;      // remove conditions here for 4 instead of 8 directions
        }
        if (Input.GetKey(Key.D))
        {
            direction.x = 1;
            if (!(Input.GetKey(Key.W)) && !(Input.GetKey(Key.S))) direction.y = 0;
        }
    }

    private void spawnBullet()
    {
        if (ammo > 0)
        {
            RevolverBullet bullet = new RevolverBullet(this.x + this.width * direction.x, this.y + this.height * direction.y);
            bullet_handler.AddChild(bullet);
            new Sound("Cowboy_Action_Revolver_Shot.wav").Play();

            if (direction.x == 1) bullet.rotation = 0;
            else if (direction.x == -1) bullet.rotation = 180;

            bullet.x_speed = 2.4f * direction.x;
            //bullet.y_speed = 1.4f * direction.y;
            ammo--;
        }
    }

    private void Update()
    {
        GetDirectionVector();
        if (Input.GetKeyDown(Key.G)) spawnBullet();

        handleCollisions();
        movementHandle();
        JumpAndGravityHandle();
        //if (Input.GetKey(Key.W)) Console.WriteLine(velocity_y);

        // possible failsafe?
        //if (x < MyGame.GAME_HEIGHT * 2) x = MyGame.GAME_HEIGHT * 2;
    }
}