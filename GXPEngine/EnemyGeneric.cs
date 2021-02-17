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
    public Pivot bullet_handler { get; set; }
    protected Vector2 direction = new Vector2(0, 0);
    protected float cooldown;

    public EnemyGeneric(string filename, int columns, int rows) : base(filename, columns, rows, -1, false, true)
    {
        SetOrigin(width / 2, height / 2);
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

    protected void spawnBullet()
    {
        RevolverBullet bullet = new RevolverBullet(this.x + this.width * direction.x, this.y + this.height * direction.y);
        bullet_handler.AddChild(bullet);
        new Sound("Enemy_Action_Rifle_Shot.wav").Play();

        if (direction.x == 1) bullet.rotation = 0;
        else if (direction.x == -1) bullet.rotation = 180;

        bullet.x_speed = bullet_speed * direction.x;
        //bullet.y_speed = 1.4f * direction.y;
    }

    protected void shootWithDistCooldown()
    {
        cooldown += Time.deltaTime / 1000f;
        if (cooldown > 2 && (DistanceTo(player1_ref) < radius_dist || DistanceTo(player2_ref) < radius_dist))
        {
            if (DistanceTo(player1_ref) < DistanceTo(player2_ref)) closer_player = player1_ref;
            else closer_player = player2_ref;
            GetDirectionVector();
            spawnBullet();
            cooldown = 0;
        }

        else if (cooldown < 1 || cooldown > 2) Move(-speed, 0);
    }

    protected void destroySelfOnNoHP()
    {
        if (HP <= 0)
        {
            (parent as Level).enemies_score += points_value;
            this.LateDestroy();
        }
    }

    private void Update()
    {
        /*foreach (Sprite other in GetCollisions())     // does not care
        {
            if (HitTest(other)) speed = -speed;
        }*/
    }
}