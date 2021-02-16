﻿using System;
using GXPEngine;
using GXPEngine.Core;

public class RevolverEnemyBullet : Sprite   // only doing this so that enemy bullets do not kill enemies(?)
                                            // I'll leave this class dormant for now, full copy of RevolverBullet 16/02
{
    private Collision coll_info = null;
    public float x_speed { get; set; }
    public float y_speed { get; set; }

    private float pull_force;
    private float seconds_after_spawn;
    public RevolverEnemyBullet(float newX, float newY) : base("bullet.png")
    {
        SetOrigin(width / 2, height / 2);
        x = newX;
        y = newY;
    }

    private void handleCollisions()
    {
        if (coll_info != null)
        {  
            if (coll_info.other is HumanPlayer || coll_info.other is AlienPlayer) (coll_info.other as Player).HP--;
            LateDestroy();
        }
    }

    protected void ApplyGravityUntilFloor()             // needs to be adapted (-0.0008f is too little in *this* case)
    {

        pull_force = (float)(-1.2f * (Math.Pow( seconds_after_spawn, 2)));
        coll_info = MoveUntilCollision(0, -pull_force);
    }

    private void Update()
    {
        seconds_after_spawn += Time.deltaTime / 1000f;
        coll_info = MoveUntilCollision(x_speed, y_speed);

        // equivalent to alien player, required due to parents vs references
        if ((parent as Pivot)._marker.Length > 0) ApplyGravityUntilFloor();

        handleCollisions();
    }
}