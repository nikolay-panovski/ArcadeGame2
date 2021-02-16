using System;
using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

public class SpawnerEnemies : AnimationSprite
{
    private int rng;
    private const int MAX_RNG = 100;
    private List<EnemyGeneric> spawn_list = new List<EnemyGeneric>();
    public SpawnerEnemies(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
        // enemies cannot be TiledObjects if this will be a TiledObject
        // to separate layer with addColliders = false!
        // whereas singular enemies should still have colliders, of course (by default)
    {
        rng = Utils.Random(0, MAX_RNG);

        if (rng % 100 < 35) spawnGroup1_1();
        else if (rng % 100 > 35 && rng % 100 < 50) spawnGroup1_2();
        else if (rng % 100 > 50 && rng % 100 < 85) spawnGroup1_3();
        else if (rng % 100 > 85) spawnGroup1_4();
    }

    private void spawnGroup1_1()
    {
        //EnemyHuman e_human = new EnemyHuman();
    }

    private void spawnGroup1_2()
    {

    }

    private void spawnGroup1_3()
    {

    }

    private void spawnGroup1_4()
    {

    }
}
