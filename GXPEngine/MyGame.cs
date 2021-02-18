using System;
using System.Drawing;
using GXPEngine;

public class MyGame : Game
{
	public const int GAME_WIDTH = 576;
	public const int GAME_HEIGHT = 432;
	public const int TILE_SIZE = 16;
	private MainMenu start_menu;
	public Level game_level { get; set; }
	private GameOver game_over;
	public int high_score { get; set; }
	public int total_lives { get; set; } = 2;
	public MyGame() : base(GAME_WIDTH * 2, GAME_HEIGHT * 2, false)		// 576x432 aka 36x27 blocks
																		// 1152x864 for PC purposes
	{
		start_menu = new MainMenu();
		AddChild(start_menu);
	}

	public void LoadLevel()		// only call from MainMenu!!
    {
		if (start_menu != null)
		{
			start_menu.LateDestroy();
			RemoveChild(start_menu);
			start_menu = null;
		}
		if (game_level != null)
		{
			game_level.LateDestroy();
			RemoveChild(game_level);		// works, just absolutely no transitions
			game_level = null;
		}
		game_level = new Level("test_map3.tmx");
		AddChild(game_level);
	}

	public void LoadGameOver()  // only call from Level on death!!
	{
		game_level.LateDestroy();
		RemoveChild(game_level);
		game_over = new GameOver();
		AddChild(game_over);
		game_over.DrawFinalScore();
	}

	public void LoadMainMenu()	// only call from GameOver!!
	{
		game_over.LateDestroy();
		RemoveChild(game_over);
		start_menu = new MainMenu();
		AddChild(start_menu);
	}
	void Update()
	{
		/*
			new Sound("ping.wav").Play();
		 */
	}

	static void Main()
	{
		new MyGame().Start();
	}
}