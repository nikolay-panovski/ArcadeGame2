using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyHuman : AnimationSprite
{
    private const int RADIUS_DIST = 256;
    private float speed = 0.6f;     // keep less than player
    public HumanPlayer player1_ref { get; set; }
    public AlienPlayer player2_ref { get; set; }
    private Player closer_player;
    public Pivot bullet_handler { get; set; }
    private Vector2 direction = new Vector2(0, 0);
    private float cooldown;

    public EnemyHuman(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
    }

    private void GetDirectionVector()   // seems to work fine
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
            RevolverBullet bullet = new RevolverBullet(this.x + this.width * direction.x, this.y + this.height * direction.y);
            bullet_handler.AddChild(bullet);

            if (direction.x == 1) bullet.rotation = 0;
            else if (direction.x == -1) bullet.rotation = 180;

            bullet.x_speed = 1.4f * direction.x;
            //bullet.y_speed = 1.4f * direction.y;
    }

    private void Update()
    {
        cooldown += Time.deltaTime / 1000f;
        if (cooldown > 2 && (DistanceTo(player1_ref) < RADIUS_DIST || DistanceTo(player2_ref) < RADIUS_DIST))
        {
            if (DistanceTo(player1_ref) < DistanceTo(player2_ref)) closer_player = player1_ref;
            else closer_player = player2_ref;
            cooldown = 0;
            GetDirectionVector();
            spawnBullet();
        }

        else if (cooldown < 1 || cooldown > 2) Move(-speed, 0);
    }
}
