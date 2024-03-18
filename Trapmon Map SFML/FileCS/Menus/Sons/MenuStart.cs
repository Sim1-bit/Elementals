using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Create;

namespace GameOfYear.Menumode
{
    class MenuStart : Menu
    {
        private static MenuStart instance;
        private static readonly object instanceLock = new object();

        public static MenuStart Instance(RenderWindow window, int proportion )
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new MenuStart(window, proportion);
                }
            }
            return instance;
        }

        public static MenuStart Instance()
        {
            return instance;
        }

        private MenuStart(RenderWindow window, int proportion) : base(Color.Transparent, window, proportion)
        {
            Button Start = new Button(new Vector2f((window.Size.X - 5 * Proportion - 15) / 2, window.Size.Y - 10 * Proportion - 15), "Start", Proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Start.Click += StartClick;
            List<Button> list = new List<Button>();
            list.Add(Start);

            Button Options = new Button(new Vector2f((window.Size.X - 7 * Proportion - 15) / 2, window.Size.Y - 6.5f * Proportion - 15), "Options", Proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Options.Click += OptionsClick;
            list.Add(Options);

            Button Exit = new Button(new Vector2f((window.Size.X - 4 * Proportion - 15) / 2, window.Size.Y - 3 * Proportion - 15), "Exit", Proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Exit.Click += ExitClick;
            list.Add(Exit);

            buttons.Add(list);
        }

        public void StartClick(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Menu && Menumode.SubScene != Menumode.SubScenes.Start)
                return;
            Program.Scene = Program.Scenes.Game;
        }
        public void OptionsClick(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Menu && Menumode.SubScene != Menumode.SubScenes.Start)
                return;
            Menumode.SubScene = Menumode.SubScenes.Options;
        }
        public void ExitClick(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Menu && Menumode.SubScene != Menumode.SubScenes.Start)
                return;
            Program.Close();
        }
    }
}
