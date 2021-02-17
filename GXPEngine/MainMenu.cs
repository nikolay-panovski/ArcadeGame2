using System;
using System.Drawing;
using GXPEngine;

public class MainMenu : EasyDraw
{
    private Font font;      // insert font here + include font in files
    private const int FONT_SIZE = 18;
    private const int OPTIONS_X = MyGame.GAME_WIDTH;
    private string[] options = new string[2];
    private Sprite selector = new Sprite("menu_arrow.png", false, false);
    private int selector_position = 0;
    private int y_position;
    public MainMenu() : base(MyGame.GAME_WIDTH * 2, MyGame.GAME_HEIGHT * 2, false)
    {
        options[0] = "START GAME";
        options[1] = "QUIT";

        y_position = game.height / 2 - (options.Length - 1) * FONT_SIZE * 2;
        //TextFont("fontnamehere.file", FONT_SIZE);
        TextSize(FONT_SIZE);
        for (int n = 0; n < options.Length; n++)
        {
            Text(options[n], game.width / 2, y_position + n * FONT_SIZE * 4);
        }

        TextAlign(CenterMode.Min, CenterMode.Center);
        TextSize(Mathf.Round(FONT_SIZE * 0.8f));
        Text("UP/DOWN or W/S\nto pick option", game.width * 0.15f, game.height * 0.85f);

        TextAlign(CenterMode.Max, CenterMode.Center);
        Text("G/NUM1 to select option", game.width * 0.85f, game.height * 0.85f);
        TextAlign(CenterMode.Min, CenterMode.Center);

        AddChild(selector);
        selector.x = OPTIONS_X - FONT_SIZE * 4;
        selector.y = y_position + selector_position * FONT_SIZE * 4 - FONT_SIZE * 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.UP)) selector_position--;
        if (Input.GetKeyDown(Key.S) || Input.GetKeyDown(Key.DOWN)) selector_position++;
        if (selector_position < 0) selector_position = options.Length - 1;
        if (selector_position > options.Length - 1) selector_position = 0;

        selector.y = y_position + selector_position * FONT_SIZE * 4 - FONT_SIZE * 2;

        if (Input.GetKeyDown(Key.G) || Input.GetKeyDown(Key.NUMPAD_1))
        {
            switch(selector_position)
            {
                case 0:
                    (parent as MyGame).LoadLevel();
                    break;
                case 1:
                    game.Destroy();
                    //System.Environment.Exit(-1);
                    break;
            }
        }
    }
}
