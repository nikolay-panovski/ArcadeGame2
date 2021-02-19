using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyGeneric : AnimationSprite
{
    protected int radius_dist { get; set; }
    protected int points_value { get; set; }
    public int HP { get; set; }
    protected float speed { get; set; }     // keep less than player
    protected float bullet_speed { get; set; }
    public HumanPlayer player1_ref { get; set; }
    public AlienPlayer player2_ref { get; set; }
    protected Player closer_player;
    protected Collision coll { get; set; }
    protected Sprite floor_check { get; set; }
    public Pivot bullet_handler { get; set; }
    protected Vector2 direction = new Vector2(0, 0);
    protected float cooldown;

    public EnemyGeneric(string filename, int columns, int rows, int frames) : base(filename, columns, rows, frames, false, true)
    {
        SetOrigin(width / 2, height / 2);
        floor_check = new Sprite("empty_16x16.png");
        floor_check.SetOrigin(floor_check.width / 2, floor_check.height / 2);
        AddChild(floor_check);
        // do not ask.
        floor_check.SetXY(this.x /*- this.width / 2 - (floor_check.width / 2 + 3)*/, this.y + this.height / 2 + floor_check.height / 2 );
    }

    protected void GetDirectionVector()   // seems to work fine
    {
        if (closer_player == player1_ref)
        {
            if (player1_ref.x < this.x) direction.x = -1;
            else direction.x = 1;
        }
        if (closer_player == player2_ref)
        {
            if (player2_ref.x < this.x) direction.x = -1;
            else direction.x = 1;
        }
    }

    private void spawnBullet()
    {
        SetCycle(6, 1);
        Animate();
        RevolverBullet bullet = new RevolverBullet(this.x + this.width * direction.x, this.y + this.height * direction.y);
        bullet_handler.AddChild(bullet);
        new Sound("Enemy_Action_Rifle_Shot.wav").Play();

        if (direction.x == 1) bullet.rotation = 0;
        else if (direction.x == -1) bullet.rotation = 180;

        bullet.x_speed = bullet_speed * direction.x;
        //bullet.y_speed = 1.4f * direction.y;
    }

    protected void ShootWithDistCooldown()
    {
        SetCycle(0, 6, 12);
        cooldown += Time.deltaTime / 1000f;
        if (cooldown > 2 && (DistanceTo(player1_ref) < radius_dist || DistanceTo(player2_ref) < radius_dist))
        {
            if (DistanceTo(player1_ref) < DistanceTo(player2_ref)) closer_player = player1_ref;
            else closer_player = player2_ref;
            GetDirectionVector();
            spawnBullet();
            cooldown = 0;
        }

        else if (cooldown < 1 || cooldown > 2)
        {
            coll = MoveUntilCollision(-speed, 0);
            Animate();
        }
    }

    protected void HandleCollisions()
    {
        if (coll != null)
        {
            if (coll.other is RevolverBullet == false) speed = -speed;  // looks meh but surprisingly works!
        }

        if (floor_check.GetCollisions().Length == 0)
        { 
            //if (speed < 0) floor_check.x = this.width / 2 + floor_check.width / 2;
            //else floor_check.x = - this.width / 2 - floor_check.width / 2;
            speed = -speed;
        }
    }

    protected void DestroySelfOnNoHP()
    {
        if (HP <= 0)
        {
            (parent as Level).enemies_score += points_value;
            this.LateDestroy();
        }
    }

    private void Update()
    {

    }
}