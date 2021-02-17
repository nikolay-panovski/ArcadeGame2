using System;
using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

public class SpawnerEnemies : AnimationSprite
{
    public HumanPlayer player1_ref { get; set; }
    public AlienPlayer player2_ref { get; set; }

    private int rng;
    private const int MAX_RNG = 100;
    private List<EnemyGeneric> spawn_list = new List<EnemyGeneric>();
    public SpawnerEnemies(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, -1, false, false)
    {

    }

    public void RollRNG()
    {
        rng = Utils.Random(0, MAX_RNG);

        if (rng % 100 < 35) spawnGroup1_1();
        else if (rng % 100 > 35 && rng % 100 < 50) spawnGroup1_2();
        else if (rng % 100 > 50 && rng % 100 < 85) spawnGroup1_3();
        else if (rng % 100 > 85) spawnGroup1_4();
    }

    private void spawnHumanWithRefs(float spawnX, float spawnY)
    {
        EnemyHuman e_human = new EnemyHuman();
        e_human.player1_ref = (parent as Level).player1_ref;
        e_human.player2_ref = (parent as Level).player2_ref;
        e_human.bullet_handler = (parent as Level).bullet_handler;
        parent.AddChild(e_human);
        e_human.x = spawnX;
        e_human.y = spawnY;
    }

    private void spawnAlienWithRefs(float spawnX, float spawnY)
    {
        EnemyAlien e_alien = new EnemyAlien();
        e_alien.player1_ref = (parent as Level).player1_ref;
        e_alien.player2_ref = (parent as Level).player2_ref;
        e_alien.bullet_handler = (parent as Level).acid_handler;
        parent.AddChild(e_alien);
        e_alien.x = spawnX;
        e_alien.y = spawnY;
    }

    private void spawnGroup1_1()
    {
        spawnHumanWithRefs(this.x, this.y);
    }

    private void spawnGroup1_2()
    {
        spawnHumanWithRefs(this.x - this.width * 2, this.y);
        spawnHumanWithRefs(this.x + this.width * 2, this.y);
    }

    private void spawnGroup1_3()
    {
        spawnAlienWithRefs(this.x, this.y);
    }

    private void spawnGroup1_4()
    {
        spawnAlienWithRefs(this.x - this.width * 2, this.y);
        spawnAlienWithRefs(this.x + this.width * 2, this.y);
    }
}
