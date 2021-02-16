using System;
using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

class Level : GameObject
{
    public HumanPlayer player1_ref { get; private set; }
    public AlienPlayer player2_ref { get; private set; }
    private DebugPlayer2 debug_ref;
    private Pivot bullet_handler = new Pivot();       // who the hell calls an empty container/handler class PIVOT??
    private Pivot acid_handler = new Pivot("acid");
    private SpawnManager spawner = new SpawnManager();
    private Camera viewport;
    private EnemyHuman[] enemies1;
    public Level(string filename) : base()
    {
        viewport = new Camera(0, 0, (game as MyGame).width, (game as MyGame).height);
        viewport.x = (game as MyGame).width / 2;
        viewport.y = (game as MyGame).height / 2;   		// / 4 instead of / 2 due to the scale
        viewport.scale = 0.5f;
        AddChild(viewport);

        AddChild(spawner);
        AddChild(bullet_handler);
        AddChild(acid_handler);
        TiledLoader level_loader = new TiledLoader(filename);
        level_loader.rootObject = this;

        level_loader.addColliders = true;
        level_loader.autoInstance = true;
        level_loader.LoadObjectGroups();
        level_loader.LoadTileLayers();

        player1_ref = FindObjectOfType<HumanPlayer>();
        player2_ref = FindObjectOfType<AlienPlayer>();
        debug_ref = FindObjectOfType<DebugPlayer2>();
        enemies1 = FindObjectsOfType<EnemyHuman>();

        player1_ref.other_player = player2_ref;
        player2_ref.other_player = player1_ref;
        player1_ref.bullet_handler = bullet_handler;
        player2_ref.bullet_handler = acid_handler;

        foreach (EnemyHuman e in enemies1) 
        {
            e.player1_ref = player1_ref;
            e.player2_ref = player2_ref;
        }
    }

    private void updateCameraX()
    {
        viewport.x += 0.4f;
    }

    private void Update()
    {
        updateCameraX();
        
        // game over routine, adapt later if/when necessary
        // probably send player to hell I mean game over also for reaching an end
        if (player1_ref.HP <= 0 || player2_ref.HP <= 0) (parent as MyGame).LoadGameOver();

        foreach (EnemyHuman e in enemies1)
        {
            if (e.x < viewport.x - MyGame.GAME_WIDTH)
            {
                e.LateDestroy();
                Console.WriteLine("enemy destroyed");
            }
        }
}