using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyHuman : EnemyGeneric
{
    public EnemyHuman() : base("Enemy Human.png", 1, 1)  // todo: animations
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
        shootWithDistCooldown();
        destroySelfOnNoHP();
    }
}
