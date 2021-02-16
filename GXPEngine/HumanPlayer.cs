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
        HP = 2;
        ammo = 12;
    }

    //---------------------------------------------------------------
    //                        MOVEMENT SECTION
    //---------------------------------------------------------------
    private void movementHandle()
    {
        if (Input.GetKey(Key.A)) Move(-x_speed, 0);
        if (Input.GetKey(Key.D)) Move(x_speed, 0);

    }

    private void JumpAndGravityHandle()
    {
        velocity_y += 1f;     // warning, The floating rounding error that Bram talked about
        if (!MoveAndCollide(0f, velocity_y))
        {
            if (velocity_y > 0f && Input.GetKey(Key.W))
            {
                velocity_y = -16f;
            }
            else velocity_y = 0f;        // player has landed
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
        /*if (Input.GetKey(Key.W))
        {
            direction.y = -1;
            if (!(Input.GetKey(Key.A)) && !(Input.GetKey(Key.D))) direction.x = 0;
        }*/
        /*if (Input.GetKey(Key.S))
        {
            direction.y = 1;
            if (!(Input.GetKey(Key.A)) && !(Input.GetKey(Key.D))) direction.x = 0;
        }*/
    }

    private void spawnBullet()
    {
        if (ammo > 0)
        {
            RevolverBullet bullet = new RevolverBullet(this.x + this.width * direction.x, this.y + this.height * direction.y);
            bullet_handler.AddChild(bullet);

            if (direction.x == 1) bullet.rotation = 0;
            else if (direction.x == -1) bullet.rotation = 180;
            /*if (direction.x == 1)
            {
                if (direction.y == 0) bullet.rotation = 0;
                else if (direction.y == -1) bullet.rotation = 135;
                else if (direction.y == 1) bullet.rotation = 225;
            }
            else if (direction.x == 0)
            {
                if (direction.y == -1) bullet.rotation = 90;
                else if (direction.y == 1) bullet.rotation = 270;
            }
            else if (direction.x == -1)
            {
                if (direction.y == 0) bullet.rotation = 180;
                else if (direction.y == -1) bullet.rotation = 45;
                else if (direction.y == 1) bullet.rotation = 315;
            }*/

            bullet.x_speed = 1.4f * direction.x;
            bullet.y_speed = 1.4f * direction.y;
            ammo--;
        }
    }

    private void Update()
    {
        GetDirectionVector();
        if (Input.GetKeyDown(Key.G)) spawnBullet();

        movementHandle();
        JumpAndGravityHandle();
        //if (Input.GetKey(Key.W)) Console.WriteLine(velocity_y);
    }
}