using System;
using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

public class SpawnerEnemies : AnimationSprite
{
    public HumanPlayer player1_ref { get; set; }
    public AlienPlayer player2_ref { get; set; }
    public bool is_active { get; set; }

    private int rng;
    private const int MAX_RNG = 100;
    //private List<EnemyGeneric> spawn_list = new List<EnemyGeneric>();
    public SpawnerEnemies(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows, -1, false, false)
    {
        is_active = true;
    }

    public void RollRNGAndSpawn()
    {
        if (is_active)
        {
            rng = Utils.Random(0, MAX_RNG);

            if ((parent as Level).viewport.x < (parent as Level).level_width * 0.33f)
            {
                if (rng % 100 < 55) spawnHumanWithRefs();
                else if (rng % 100 > 55 && rng % 100 < 90) spawnAlienWithRefs();
                else if (rng % 100 > 90) spawnRobotWithRefs();
            }
            else if ((parent as Level).viewport.x > (parent as Level).level_width * 0.33f && (parent as Level).viewport.x < (parent as Level).level_width * 0.66f)
            {
                if (rng % 100 < 40) spawnHumanWithRefs();
                else if (rng % 100 > 40 && rng % 100 < 80) spawnAlienWithRefs();
                else if (rng % 100 > 80) spawnRobotWithRefs();
            }
            else
            {
                if (rng % 100 < 25) spawnHumanWithRefs();
                else if (rng % 100 > 25 && rng % 100 < 70) spawnAlienWithRefs();
                else if (rng % 100 > 70) spawnRobotWithRefs();
            }
        }
    }

    private void spawnHumanWithRefs()
    {
        EnemyHuman e_human = new EnemyHuman();
        e_human.player1_ref = (parent as Level).player1_ref;
        e_human.player2_ref = (parent as Level).player2_ref;
        e_human.bullet_handler = (parent as Level).bullet_handler;
        parent.AddChild(e_human);
        e_human.x = this.x;
        e_human.y = this.y;
    }

    private void spawnAlienWithRefs()
    {
        EnemyAlien e_alien = new EnemyAlien();
        e_alien.player1_ref = (parent as Level).player1_ref;
        e_alien.player2_ref = (parent as Level).player2_ref;
        e_alien.bullet_handler = (parent as Level).acid_handler;
        parent.AddChild(e_alien);
        e_alien.x = this.x;
        e_alien.y = this.y;
    }

    private void spawnRobotWithRefs()
    {
        EnemyRobot e_robot = new EnemyRobot();
        e_robot.player1_ref = (parent as Level).player1_ref;
        e_robot.player2_ref = (parent as Level).player2_ref;
        parent.AddChild(e_robot);
        e_robot.x = this.x;
        e_robot.y = this.y;
    }
}