using System;
using GXPEngine;
using TiledMapParser;

public class SpawnerEnemies : AnimationSprite
{
    private int rng;
    private const int MAX_RNG = 100;
    public SpawnerEnemies(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {

    }


}
