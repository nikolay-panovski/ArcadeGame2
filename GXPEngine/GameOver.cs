using System;
using System.Drawing;
using GXPEngine;

public class GameOver : EasyDraw
{
    private Font font;      // insert font here + include font in files
    private float time_counter;
    public GameOver() : base(MyGame.GAME_WIDTH * 2, MyGame.GAME_HEIGHT * 2, false)
    {
        TextAlign(CenterMode.Center, CenterMode.Center);
        //TextFont("fontnamehere.file", FONT_SIZE);
        Text("GAME OVER", game.width / 2, game.height / 2);     
    }

    public void DrawFinalScore()
    {
        Text("Final score: " + (parent as MyGame).high_score, game.width / 2, game.height * 0.75f);
    }
    private void Update()
    {
        time_counter += Time.deltaTime / 1000f;
        if (time_counter >= 5) (parent as MyGame).LoadMainMenu();
    }
}
