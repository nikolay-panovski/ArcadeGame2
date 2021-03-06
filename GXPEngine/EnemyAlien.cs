﻿using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyAlien : EnemyGeneric
{
    public EnemyAlien() : base("enemy_alien_anim.png", 6, 2, 12)
    {
        radius_dist = 256;      // more?
        points_value = 50;
        HP = 1;
        speed = 0.6f;
        bullet_speed = 2.8f;    // 1.6f?
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        HandleCollisions();
        ShootWithDistCooldown();
        DestroySelfOnNoHP();
    }
}