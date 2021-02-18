using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
public class AlienPlayer : Player
{
    public HumanPlayer other_player { get; set; }
    public float bullet_cooldown { get; set; } = 0;        // for shooting

    public AlienPlayer(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, obj)
    {
        HP = 3;
    }

    private void movementHandle()
    {
        if (Input.GetKey(Key.LEFT))
        {
            coll_info = MoveUntilCollision(-x_speed, 0);
            if (!Input.GetKey(Key.UP))
            {
                SetCycle(0, 3, 20);
                Animate();
            }
        }
        if (Input.GetKey(Key.RIGHT))
        {
            coll_info = MoveUntilCollision(x_speed, 0);
            if (!Input.GetKey(Key.UP))
            {
                SetCycle(0, 3, 20);
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
                SetCycle(4, 1);
                Animate();
            }
            if (velocity_y > 0f && Input.GetKey(Key.UP))
            {
                velocity_y = -4f;
                SetCycle(3, 1);
                Animate();
            }
            else if (velocity_y != -3f) velocity_y = 0f;        // player has landed + hack for water/acid
        }
        if (Input.GetKeyDown(Key.UP)) velocity_y = -4f; // infinite jump, seems to not break more than usual...
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
            new Sound("Alien_Action_Acid_Shot.wav").Play();

            if (direction.x == 1) bullet.rotation = 0;
            else if (direction.x == -1) bullet.rotation = 180;

            bullet.x_speed = 1.4f * direction.x;        // smaller than usual bullet speed because tree
            //bullet.y_speed = 1.4f * direction.y;
            bullet_cooldown = 2;
        }
    }

    private void Update()
    {
        bullet_cooldown -= Time.deltaTime / 1000f;
        if (bullet_cooldown < 0) bullet_cooldown = 0;
        GetDirectionVector();
        if (Input.GetKeyDown(Key.NUMPAD_1)) spawnBullet();

        if (shield_timer > 0) shield_timer -= Time.deltaTime / 1000f;
        if (shield_timer < 0) shield_timer = 0;
        if (i_frames > 0) i_frames--;

        movementHandle();
        JumpAndGravityHandle();

        // bounding / failsafe
        if (y < 0) y = 0;
        if (y > MyGame.GAME_HEIGHT * 2) y = MyGame.GAME_HEIGHT * 2;
    }
}
