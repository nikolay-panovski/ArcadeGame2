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
        if (Input.GetKey(Key.A))
        {
            coll_info = MoveUntilCollision(-x_speed, 0);
            if (!Input.GetKey(Key.W))
            {
                SetCycle(0, 6, 10);
                Animate();
            }
        }
        if (Input.GetKey(Key.D))
        {
            coll_info = MoveUntilCollision(x_speed, 0);
            if (!Input.GetKey(Key.W))
            {
                SetCycle(0, 6, 10);
                Animate();
            }
        }

    }

    private void JumpAndGravityHandle()
    {
        velocity_y += 0.08f;
        if (!MoveAndCollide(0f, velocity_y))
        {
            if (velocity_y < 0f)
            {
                SetCycle(13, 1);
                Animate();
            }
            if (velocity_y > 0f && Input.GetKey(Key.W))
            {
                velocity_y = -4f;
                SetCycle(12, 1);
                Animate();
            }
            else if (velocity_y != -3f)
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

            bullet.x_speed = 2.8f * direction.x;
            //bullet.y_speed = 1.4f * direction.y;
            ammo--;
        }
    }

    private void Update()
    {
        GetDirectionVector();
        if (Input.GetKeyDown(Key.G))
        {
            SetCycle(6, 6, 10);
            spawnBullet();
            //for (int i = 0; i < frameCount * 10; i++)
            //{
                Animate();
            //} 
        }

        handleCollisions();
        movementHandle();
        JumpAndGravityHandle();

        if (shield_timer > 0) shield_timer -= Time.deltaTime / 1000f;
        if (shield_timer < 0)
        {
            shield_timer = 0;
            RemoveChild(sprite_shielded);
        }
        if (i_frames > 0) i_frames--;

        // bounding / failsafe
        if (y < 0) y = 0;
        if (y > MyGame.GAME_HEIGHT * 2) y = MyGame.GAME_HEIGHT * 2;
    }
}