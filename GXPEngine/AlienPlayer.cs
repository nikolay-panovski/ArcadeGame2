using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
public class AlienPlayer : Player
{
    public HumanPlayer other_player { get; set; }

    public AlienPlayer(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, obj)
    {
        HP = 3;
    }

    private void movementHandle()
    {
        if (Input.GetKey(Key.LEFT) ) Move(-x_speed, 0);
        if (Input.GetKey(Key.RIGHT)) Move(x_speed, 0);

        /*if (Input.GetKeyDown(Key.UP))   // for infinite jumps
        {
            jump_frames = 0;
            air_frames = 0;
        }
        if (Input.GetKey(Key.UP)) jumpHandle();*/
    }

    private void JumpAndGravityHandle()
    {
        velocity_y += 0.3f;     // warning, The floating rounding error that Bram talked about
                                 // aka the remaining issue(s) now are ceilings
        if (!MoveAndCollide(0f, velocity_y))
        {
            if (velocity_y > 0f && Input.GetKey(Key.UP))
            {
                velocity_y = -12f;
            }
            else velocity_y = 0f;        // player has landed
        }
    }

    //---------------------------------------------------------------
    //                        BULLET SECTION
    //---------------------------------------------------------------
    private void GetDirectionVector()
    {
        if (Input.GetKey(Key.LEFT))
        {
            direction.x = -1;
            if (!(Input.GetKey(Key.UP)) && !(Input.GetKey(Key.DOWN))) direction.y = 0;
        }
        if (Input.GetKey(Key.RIGHT))
        {
            direction.x = 1;
            if (!(Input.GetKey(Key.UP)) && !(Input.GetKey(Key.DOWN))) direction.y = 0;
        }
        /*if (Input.GetKey(Key.UP))
        {
            direction.y = -1;
            if (!(Input.GetKey(Key.LEFT)) && !(Input.GetKey(Key.RIGHT))) direction.x = 0;
        }
        if (Input.GetKey(Key.DOWN))
        {
            direction.y = 1;
            if (!(Input.GetKey(Key.LEFT)) && !(Input.GetKey(Key.RIGHT))) direction.x = 0;
        }*/
    }

    private void spawnBullet()
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
    }

    private void Update()
    {
        GetDirectionVector();
        if (Input.GetKeyDown(Key.NUMPAD_4)) spawnBullet();

        movementHandle();
        JumpAndGravityHandle();
    }
}
