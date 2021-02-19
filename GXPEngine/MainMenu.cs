using System;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;

public class MainMenu : EasyDraw
{
    private const int FONT_SIZE = 22;
    private const int OPTIONS_X = MyGame.GAME_WIDTH;
    private string[] options = new string[2];
    private Sprite selector = new Sprite("menu_arrow.png", false, false);
    private Sprite logo = new Sprite("oo_sizeable.png", false, false);
    private int selector_position = 0;
    private int y_position;
    PrivateFontCollection custom_font = new PrivateFontCollection();
    FontFamily[] family;
    private string font_name = "";
    public Sound main_menu_track;

    public MainMenu() : base(MyGame.GAME_WIDTH * 2, MyGame.GAME_HEIGHT * 2, false)
    {
        options[0] = "START GAME";
        options[1] = "QUIT";

        main_menu_track = new Sound("Song_Main_Menu.mp3", true);
        main_menu_track.Play(false, 0);

        // CUSTOM FONT SECTION - also apply to HUD
        custom_font.AddFontFile("spaceranger.ttf");
        family = custom_font.Families;
        if (family.Length > 0) font_name = family[0].Name;
        TextFont(font_name, FONT_SIZE);

        y_position = game.height / 2 - (options.Length - 1) * FONT_SIZE * 2;
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

        AddChild(logo);
        logo.SetOrigin(logo.width / 2, logo.height / 2);
        logo.x = OPTIONS_X;
        logo.y = MyGame.GAME_HEIGHT / 3;
    }

    private void alphaFadeout()
    {
        if (alpha > 0.1f)
        {
            alpha -= 0.1f;
            selector.alpha -= 0.1f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.UP)) selector_position--;
        if (Input.GetKeyDown(Key.S) || Input.GetKeyDown(Key.DOWN)) selector_position++;
        if (selector_position < 0) selector_position = options.Length - 1;
        if (selector_position > options.Length - 1) selector_position = 0;

        selector.y = y_position + selector_position * FONT_SIZE * 4 - FONT_SIZE * 1.5f;

        if (Input.GetKeyDown(Key.G) || Input.GetKeyDown(Key.NUMPAD_1))
        {
            switch(selector_position)
            {
                case 0:
                    /*if (alpha <= 0.1f)*/ (parent as MyGame).LoadLevel();
                    //else alphaFadeout();
                    break;
                case 1:
                    game.Destroy();
                    //System.Environment.Exit(-1);
                    break;
            }
        }
    }
}
