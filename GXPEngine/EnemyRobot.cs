using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyRobot : EnemyGeneric
{
    public AnimationSprite hurtbox { get; set; }
    public EnemyRobot(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, obj)
    {
        //radius_dist = player1_ref.width;
        radius_dist = 64;   // ... 32 is not enough
        points_value = 100;
        HP = 2;
        speed = 1.2f;
        SetOrigin(width / 2, height / 2);
    }

    private void meleeWithDistCooldown()      // for enemy robot melee
    {
        cooldown += Time.deltaTime / 1000f;
        if (cooldown > 1 && (DistanceTo(player1_ref) < radius_dist || DistanceTo(player2_ref) < radius_dist))
        {
            if (DistanceTo(player1_ref) < DistanceTo(player2_ref)) closer_player = player1_ref;
            else closer_player = player2_ref;
            GetDirectionVector();
            if (hurtbox != null)
            {
                hurtbox.Destroy();
                RemoveChild(hurtbox);
                hurtbox = null;
            }
            else
            {
                hurtbox = new AnimationSprite("pinky.png", 1, 1);
                hurtbox.SetOrigin(hurtbox.width / 2, hurtbox.height / 2);
                hurtbox.x += this.width * direction.x;
                AddChild(hurtbox);
                if (hurtbox.HitTest(closer_player))
                {
                    closer_player.HP--;
                    Console.WriteLine("ow");
                    closer_player.x += hurtbox.width * direction.x;     // rough but works
                }
            }
            cooldown = 0;
        }

        //else Move(-speed, 0);
    }

    private void Update()
    {
        meleeWithDistCooldown();
        destroySelfOnNoHP();
    }
}