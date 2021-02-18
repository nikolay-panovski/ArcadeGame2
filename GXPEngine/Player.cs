using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player : AnimationSprite
{
    protected Collision coll_info = null;
    protected Vector2 direction = new Vector2(0, 0);
    public Pivot bullet_handler { get; set; }
    protected bool is_colliding;
    public int HP { get; set; }
    public int active_ID { get; set; }
    public float shield_timer { get; set; }
    protected int i_frames;
    protected Sprite sprite_shielded = new Sprite("circle.png", false, false);

    protected float x_speed = 0.8f;

    protected float velocity_y = 0.0f;

    public Player(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        sprite_shielded.SetOrigin(sprite_shielded.width / 2, sprite_shielded.height / 2);
    }

    protected bool MoveAndCollide(float deltaX, float deltaY)   // returns true  if move is possible, false if landed on floor (can't move)
    {
        x += deltaX;
        y += deltaY;

        bool isColliding = false;
        foreach (Sprite other in GetCollisions())           // works with everything added as child to the Level
        {
                resolveCollision(this, other, deltaX, deltaY);
                isColliding = true;
        }

        return !isColliding;

    }

    private void resolveCollision(Sprite subject, Sprite collider, float deltaX, float deltaY)
    {
        // maybe useless check but not sure /shrug
        if (collider is RevolverBullet || collider is PickupAmmo || collider is PickupCell || collider is PickupDrug ||
            collider is PickupShield || collider is TileButton || collider is TileTransmitter || collider is TileLaser ||
            collider is TileWater || collider is TileAcid)
        {
            if (collider is RevolverBullet)
            {
                collider.LateDestroy();
                if (shield_timer <= 0)  // shield seems to work now
                {
                    HP--;
                    if (this is HumanPlayer)
                    {
                        (parent as Level).game_hud.RemoveSpriteFromHuman();
                        new Sound("Cowboy_Action_Damage.wav").Play();
                    }
                    if (this is AlienPlayer)
                    {
                        (parent as Level).game_hud.RemoveSpriteFromAlien();
                        new Sound("Alien_Action_Damage.wav").Play();
                    }
                }
            }
            if (collider is PickupAmmo)
            {
                (parent as Level).pickups_score += (collider as PickupAmmo).points_value * (parent as Level).dist_multiplier;
                (parent as Level).player1_ref.ammo += 12;
                new Sound("Global_Action_Ammo_Collect.wav").Play();
                collider.LateDestroy();
            }
            if (collider is PickupCell)
            {
                (parent as Level).game_hud.AddCellSprite();
                (parent as Level).pickups_score += (collider as PickupCell).points_value * (parent as Level).dist_multiplier;
                (parent as Level).number_cells++;
                new Sound("Global_Action_Energy_Cell.wav").Play();
                collider.LateDestroy();
            }
            if (collider is PickupDrug)
            {
                (parent as Level).pickups_score += (collider as PickupDrug).points_value * (parent as Level).dist_multiplier;
                (parent.parent as MyGame).total_lives++;
                new Sound("Global_Action_Killer_Drug.wav").Play();
                collider.LateDestroy();
            }
            if (collider is PickupShield)
            {
                (parent as Level).pickups_score += (collider as PickupShield).points_value * (parent as Level).dist_multiplier;
                shield_timer = 8;
                AddChild(sprite_shielded);
                new Sound("Global_Action_Shield_Active.wav").Play();
                collider.LateDestroy();
            }
            if (collider is TileButton)
            {
                active_ID = (collider as TileButton).button_ID;
                if ((collider as TileButton).currentFrame != 2)
                {
                    for (int f = 0; f < 3; f++)  // yes yes it's literally hardcoded, and happens too fast for human eyes
                    {
                        (collider as TileButton).SetFrame(f);
                    }
                }
                if ((collider as TileButton).sound_played == false)
                {
                    new Sound("Global_Action_Button_Pressed.wav").Play();   // not fixed
                    (collider as TileButton).sound_played = true;
                }
            }
            if (collider is TileTransmitter && (collider as TileTransmitter).is_charged == false)
            {
                if ((parent as Level).number_cells > 0)
                {
                    if ((collider as TileTransmitter).currentFrame != (collider as TileTransmitter).frameCount - 1)
                    {
                        for (int f = 0; f < (collider as TileTransmitter).frameCount; f++) 
                        {
                            (collider as TileTransmitter).SetFrame(f);
                        }
                    }
                    (collider as TileTransmitter).is_charged = true;
                    (parent as Level).game_hud.RemoveCellSprite();
                    (parent as Level).number_cells--;
                    (parent as Level).pickups_score += 100;
                }
            }
            if (collider is TileLaser)
            {
                if ((collider as TileLaser).is_deact == false) {
                    if (shield_timer <= 0 && i_frames == 0)
                    {
                        HP--;
                        if (this is HumanPlayer)
                        {
                            (parent as Level).game_hud.RemoveSpriteFromHuman();
                            new Sound("Cowboy_Action_Damage.wav").Play();
                        }
                        if (this is AlienPlayer)
                        {
                            (parent as Level).game_hud.RemoveSpriteFromAlien();
                            new Sound("Alien_Action_Damage.wav").Play();
                        }
                    }
                    i_frames = 15;
                }
            }
            if (collider is TileWater)
            {
                if (this is AlienPlayer)
                {
                    velocity_y = -3f;
                    if (i_frames == 0)
                    {
                        HP--;
                        new Sound("Alien_Action_Damage.wav").Play();
                        (parent as Level).game_hud.RemoveSpriteFromAlien();
                    }
                        i_frames = 8;
                }
            }
            if (collider is TileAcid)
            {
                if (this is HumanPlayer)
                {
                    velocity_y = -3f;
                    if (i_frames == 0)
                    {
                        HP--;
                        new Sound("Cowboy_Action_Damage.wav").Play();
                        (parent as Level).game_hud.RemoveSpriteFromHuman();
                    }
                    i_frames = 8;
                }
            }
        }

        //else  // ELSE is not any object we want special collision with -> treat as walls etc.
                // seems to run for everything anyway
                // quicksand gone, otherwise water and acid won't work
        //{
            if (deltaX < 0) subject.x = (collider.x + collider.width / 2 + subject.width / 2);  // refactored for origin at middle
            if (deltaX > 0) subject.x = (collider.x - collider.width / 2 - subject.width / 2);
            if (deltaY < 0) subject.y = (collider.y + collider.height / 2 + subject.height / 2) + 1;
            if (deltaY > 0) subject.y = (collider.y - collider.height / 2 - subject.height / 2) - 1;
        //}

        if (collider is TileButton == false)
        {
            active_ID = -1;    // takes collision with something else to reset, but shh
            foreach (TileButton b in (parent as Level).buttons)     // did not work
            {
                b.sound_played = false;
                if (b.currentFrame != b.frameCount - 1) {
                    for (int f = 3; f < b.frameCount; f++)  // yes yes it's literally hardcoded
                    {
                       b.SetFrame(f);
                    }
                }
            }
        }
    }

    protected void handleCollisions()   // do I really have to do this?
    {
        if (coll_info != null)
        {
            /*if (coll_info.other is RevolverBullet)
            {
                if (shield_timer <= 0) HP--;
                (parent as Level).game_hud.RemoveSpriteFromHuman();
                coll_info.other.Destroy();
            }*/
            if (coll_info.other is PickupAmmo)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupAmmo).points_value * (parent as Level).dist_multiplier;
                (parent as Level).player1_ref.ammo += 12;
                new Sound("Global_Action_Ammo_Collect.wav").Play();
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupCell)
            {
                (parent as Level).game_hud.AddCellSprite();
                (parent as Level).pickups_score += (coll_info.other as PickupCell).points_value * (parent as Level).dist_multiplier;
                (parent as Level).number_cells++;
                new Sound("Global_Action_Energy_Cell.wav").Play();
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupDrug)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupDrug).points_value * (parent as Level).dist_multiplier;
                (parent.parent as MyGame).total_lives++;
                new Sound("Global_Action_Killer_Drug.wav").Play();
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupShield)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupShield).points_value * (parent as Level).dist_multiplier;
                shield_timer = 8;
                AddChild(sprite_shielded);
                new Sound("Global_Action_Shield_Active.wav").Play();
                coll_info.other.Destroy();
            }
            if (coll_info.other is TileLaser)   // does not work? ok, still solid, so... cool with me / i-frames if necessary
            {
                if (shield_timer <= 0 && i_frames == 0)
                {
                    HP--;
                    if (this is HumanPlayer)
                    {
                        (parent as Level).game_hud.RemoveSpriteFromHuman();
                        new Sound("Cowboy_Action_Damage.wav").Play();
                    }
                    if (this is AlienPlayer)
                    {
                        (parent as Level).game_hud.RemoveSpriteFromAlien();
                        new Sound("Alien_Action_Damage.wav").Play();
                    }
                }
                i_frames = 15;
            }
            // don't access water and acid from the side, ever
        }
    }

    /*protected void Update()
    {
        
    }*/
}