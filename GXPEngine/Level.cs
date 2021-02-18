using System;
using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

public class Level : GameObject
{
    public HumanPlayer player1_ref { get; private set; }
    public AlienPlayer player2_ref { get; private set; }
    public Pivot bullet_handler { get; private set; } = new Pivot();       // who the hell calls an empty container/handler class PIVOT??
    public Pivot acid_handler { get; private set; } = new Pivot("acid");
    public Camera viewport { get; }
    public HUD game_hud { get; private set; } = new HUD();
    //------------------------------------
    //          OBJECT LISTS
    //------------------------------------
    private List<SpawnerEnemies> enemies1 = new List<SpawnerEnemies>();     // altered FindObjectsOfType<> to return a List. dunno why it doesn't already do that.
    private List<TileTransmitter> charged = new List<TileTransmitter>();
    private List<TileDoor> doors = new List<TileDoor>();
    private List<TileTarget> targets = new List<TileTarget>();
    private List<TileLaser> lasers = new List<TileLaser>();


    //------------------------------------
    //          RELATED TO SCORE
    //------------------------------------
    public int level_score { get; set; }
    public int distance_score { get; set; }
    public int enemies_score { get; set; }
    public int pickups_score { get; set; }
    public int dist_multiplier { get; private set; }
    //------------------------------------
    //          RELATED TO RESOURCES
    //------------------------------------
    public int number_cells { get; set; }
    public int total_lives { get; set; }
    //------------------------------------
    //          DIMENSION CHECKS
    //------------------------------------
    public int level_width { get; private set; }
    public int level_height { get; private set; }



    //----------------------------------------------------
    //                      Level()
    //----------------------------------------------------
    // As usual, this part also handles MANY references.
    //
    public Level(string filename) : base()
    {
        viewport = new Camera(0, 0, (game as MyGame).width, (game as MyGame).height);
        viewport.x = (game as MyGame).width / 4;
        viewport.y = (game as MyGame).height / 4;   		// / 4 instead of / 2 due to the scale
        viewport.scale = 0.5f;
        AddChild(viewport);

        AddChild(bullet_handler);
        AddChild(acid_handler);
        TiledLoader level_loader = new TiledLoader(filename);
        level_loader.rootObject = this;

        level_loader.autoInstance = true;
        //level_loader.addColliders = false;
        level_loader.LoadObjectGroups(1);   // manual type considered, no thanks, I assume Tiled wants its sprites anyway

        level_loader.addColliders = true;  
        level_loader.LoadObjectGroups(0);
        level_loader.LoadTileLayers();
        level_width = level_loader.map.Width * level_loader.map.TileWidth;
        level_height = level_loader.map.Height * level_loader.map.TileHeight;

        AddChild(game_hud);

        //----------------------------------------------------
        //          FIND OBJECTS AND SET REFERENCES
        //----------------------------------------------------
        player1_ref = FindObjectOfType<HumanPlayer>();
        player2_ref = FindObjectOfType<AlienPlayer>();

        enemies1 = FindObjectsOfType<SpawnerEnemies>();
        charged = FindObjectsOfType<TileTransmitter>();
        doors = FindObjectsOfType<TileDoor>();
        targets = FindObjectsOfType<TileTarget>();
        lasers = FindObjectsOfType<TileLaser>();

        player1_ref.other_player = player2_ref;
        player2_ref.other_player = player1_ref;
        player1_ref.bullet_handler = bullet_handler;
        player2_ref.bullet_handler = acid_handler;

        game_hud.player1_ref = player1_ref;
        game_hud.player2_ref = player2_ref;
        game_hud.SetInitSprites();

        foreach (SpawnerEnemies spawn in enemies1) 
        {
            spawn.player1_ref = player1_ref;
            spawn.player2_ref = player2_ref;
            spawn.parent = this;
        }
    }

    private void triggerSpawners()
    {
        foreach (SpawnerEnemies spawn in enemies1)
        {
            if (spawn.x < viewport.x + MyGame.GAME_WIDTH)   // probably too many checks but the exact number of spawns
            {
                spawn.RollRNGAndSpawn();
                spawn.is_active = false;
            }
        }
    }

    private void checkActiveTransmitters()
    {
        foreach (TileDoor d in doors)
        {
            if (d.is_open == false)
            {
                foreach (TileTransmitter t in charged)
                {
                    if (t.is_charged && t.charge_ID == d.door_ID)
                    {
                        d.openDoor();
                        d.is_open = true;
                        d.open_locked = true;
                    }
                }
            }
        }
    }

    private void checkShotTargets()
    {
        foreach (TileLaser l in lasers)
        {
            if (l.is_deact == false)
            {
                foreach (TileTarget s in targets)
                {
                    if (s.is_shot && s.shoot_ID == l.laser_ID)
                    {
                        l.deactivateLaser();
                        l.is_deact = true;
                        l.deact_locked = true;
                    }
                }
            }
        }
    }

    private void checkActiveButtons()
    {
        if (player1_ref.active_ID != -1 || player2_ref.active_ID != -1) // I hope the conflicting logic is over
        {
            foreach (TileDoor d in doors)
            {
                if (d.open_locked == false)
                {
                    if (player1_ref.active_ID == d.door_ID || player2_ref.active_ID == d.door_ID)
                    {
                        d.openDoor();
                        d.is_open = true;
                    }
                }
            }

            foreach (TileLaser l in lasers)
            {
                if (l.deact_locked == false)
                {
                    if (player1_ref.active_ID == l.laser_ID || player2_ref.active_ID == l.laser_ID)
                    {
                        l.deactivateLaser();
                        l.is_deact = true;
                    }
                }
            }
        }

        else
        {

            foreach (TileDoor d in doors)
            {
                if (d.open_locked == false)
                {
                    d.closeDoor();
                    d.is_open = false;
                }
            }
            foreach (TileLaser l in lasers)
            {
                if (l.deact_locked == false)
                {
                    l.activateLaser();
                    l.is_deact = false;
                }
            }
        }
    }

    private void updateCameraX()
    {
        viewport.x += 0.5f;
        distance_score = ((int)viewport.x - (game as MyGame).width / 2) / 4;
    }

    private void setDistMultiplier()
    {
        if (viewport.x < level_width * 0.33f) dist_multiplier = 1;
        else if (viewport.x >= level_width * 0.33f && viewport.x < level_width * 0.66f) dist_multiplier = 2;
        else dist_multiplier = 3;
    }

    private void destroyOutOfBounds()   // this probably destroys only spawners and not enemies anymore, but screw it
    {
        for (int e = enemies1.Count - 1; e >= 0; e--)
        {
            if (enemies1[e].x < viewport.x - MyGame.GAME_WIDTH)
            {
                enemies1[e].LateDestroy();
                enemies1.Remove(enemies1[e]);
            }
        }
    }

    private void Update()
    {
        updateCameraX();
        triggerSpawners();

        checkActiveButtons();
        checkActiveTransmitters();
        checkShotTargets();

        level_score = distance_score + enemies_score + pickups_score;
        Console.WriteLine(level_score);

        // game over routine, adapt later if/when necessary
        // probably send player to hell I mean game over also for reaching an end
        if ((player1_ref.HP <= 0 || player2_ref.HP <= 0) && (parent as MyGame).total_lives > 0)
        {
            (parent as MyGame).total_lives--;
            (parent as MyGame).LoadLevel();
        }
        else if ((parent as MyGame).total_lives <= 0)
        {
            (parent as MyGame).high_score = level_score;        // still 0 at here somehow
            (parent as MyGame).LoadGameOver();
        }

        setDistMultiplier();

        destroyOutOfBounds();
    }
}