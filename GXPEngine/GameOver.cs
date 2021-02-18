using System;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;

public class GameOver : EasyDraw
{
    private const int FONT_SIZE = 22;
    private float time_counter;
    PrivateFontCollection custom_font = new PrivateFontCollection();
    FontFamily[] family;
    private string font_name = "";
    public GameOver() : base(MyGame.GAME_WIDTH * 2, MyGame.GAME_HEIGHT * 2, false)
    {
        TextAlign(CenterMode.Center, CenterMode.Center);
        custom_font.AddFontFile("spaceranger.ttf");
        family = custom_font.Families;
        if (family.Length > 0) font_name = family[0].Name;
        TextFont(font_name, FONT_SIZE);
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
