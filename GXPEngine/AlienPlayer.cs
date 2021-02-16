using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
public class AlienPlayer : Player
{
    public HumanPlayer other_player { get; set; }
    private float bullet_cooldown = 0;        // for shooting

    public AlienPlayer(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, obj)
    {
        HP = 3;
    }

    private void movementHandle()
    {
        if (Input.GetKey(Key.LEFT) ) coll_info = MoveUntilCollision(-x_speed, 0);
        if (Input.GetKey(Key.RIGHT)) coll_info = MoveUntilCollision(x_speed, 0);
    }

    private void JumpAndGravityHandle()
    {
        velocity_y += 0.08f;     
        if (!MoveAndCollide(0f, velocity_y))
        {
            if (velocity_y > 0f && Input.GetKey(Key.UP)) // if no velocity_y check here, 
                                                         // does not clip through ceiling, sticks instead, clips on key release
            {
                velocity_y = -4f;
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
    }

    private void spawnBullet()
    {
        if (bullet_cooldown == 0)
        {
            RevolverBullet bullet = new RevolverBullet(this.x + this.width * direction.x, this.y + this.height * direction.y);
            bullet_handler.AddChild(bullet);

            if (direction.x == 1) bullet.rotation = 0;
            else if (direction.x == -1) bullet.rotation = 180;

            bullet.x_speed = 1.6f * direction.x;        // smaller than usual bullet speed because tree
            //bullet.y_speed = 1.4f * direction.y;
            bullet_cooldown = 2;
        }
    }

    private void Update()
    {
        bullet_cooldown -= Time.deltaTime / 1000f;
        if (bullet_cooldown < 0) bullet_cooldown = 0;
        GetDirectionVector();
        if (Input.GetKeyDown(Key.NUMPAD_4)) spawnBullet();

        movementHandle();
        JumpAndGravityHandle();
    }
}
