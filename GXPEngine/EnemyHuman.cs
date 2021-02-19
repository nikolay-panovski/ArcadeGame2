using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyHuman : EnemyGeneric
{
    public EnemyHuman() : base("enemy_human_anim.png", 6, 2, 11)
    {
        radius_dist = 192;
        points_value = 50;
        HP = 1;
        speed = 0.5f;
        bullet_speed = 2.4f;
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        HandleCollisions();
        ShootWithDistCooldown();
        DestroySelfOnNoHP();

    }
}
