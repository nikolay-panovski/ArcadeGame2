using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class EnemyRobot : EnemyGeneric
{
    public AnimationSprite hurtbox { get; set; }
    public EnemyRobot() : base("enemy_robot_anim.png", 6, 2, 9)
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
        if (cooldown > 1)
        {
            if (hurtbox != null)
            {
                hurtbox.Destroy();
                RemoveChild(hurtbox);
                hurtbox = null;
            }
            else if ((DistanceTo(player1_ref) < radius_dist || DistanceTo(player2_ref) < radius_dist))
            {
                if (DistanceTo(player1_ref) < DistanceTo(player2_ref)) closer_player = player1_ref;
                else closer_player = player2_ref;
                GetDirectionVector();

                if (hurtbox == null)
                {
                    hurtbox = new AnimationSprite("pinky.png", 1, 1);   // must have collider
                    hurtbox.SetOrigin(hurtbox.width / 2, hurtbox.height / 2);
                    hurtbox.x += this.width * direction.x;
                    AddChild(hurtbox);
                    SetCycle(6, 1);
                    Animate();
                    new Sound("Enemy_Action_Damage.wav").Play();
                    if (hurtbox.HitTest(closer_player))
                    {
                        closer_player.HP--;
                        closer_player.x += hurtbox.width * direction.x;     // rough but works
                        if (closer_player is HumanPlayer) 
                        {
                            (closer_player.parent as Level).game_hud.RemoveSpriteFromHuman();
                            new Sound("Cowboy_Action_Damage.wav").Play();
                        }
                        if (closer_player is AlienPlayer)
                        {
                            (closer_player.parent as Level).game_hud.RemoveSpriteFromAlien();
                            new Sound("Alien_Action_Damage.wav").Play();
                        }
                    }
                }
                cooldown = 0;
            }
        }

        else
        {
            SetCycle(0, 6, 12);
            Animate();
            coll = MoveUntilCollision(-speed, 0);
        }
    }

    private void Update()
    {
        HandleCollisions();
        meleeWithDistCooldown();
        DestroySelfOnNoHP();
    }
}