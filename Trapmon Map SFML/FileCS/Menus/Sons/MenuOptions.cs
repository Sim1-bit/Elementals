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
    class MenuOptions : Menu
    {
        private static MenuOptions instance;
        private static readonly object instanceLock = new object();

        private Text VolumeText;

        public static MenuOptions Instance(RenderWindow window, int proportion)
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new MenuOptions(window, proportion);
                }
            }
            return instance;
        }

        public static MenuOptions Instance()
        {
            return instance;
        }

        private MenuOptions(RenderWindow window,int proportion) : base(Color.Transparent,window, proportion)
        {
            Button Inc = new Button(new Vector2f((window.Size.X + 6 * Proportion - 15) / 2, window.Size.Y - 6.5f * proportion - 15), "+", proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Inc.Click += IncClicked;
            List<Button> list = new List<Button>();
            list.Add(Inc);

            Button Dec = new Button(new Vector2f((window.Size.X - 8 * Proportion - 15) / 2, window.Size.Y - 6.5f * proportion - 15), "-", proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Dec.Click += DecClicked;
            list.Add(Dec);

            Button Menu = new Button(new Vector2f((window.Size.X - 4 * Proportion - 15) / 2, window.Size.Y - 3 * proportion - 15), "Menu", proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Menu.Releas += ExitReleased;
            list.Add(Menu);

            buttons.Add(list);

            VolumeText = new Text(Program.Volume.ToString(), new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Origin = new Vector2f(Program.Volume.ToString().Length / 2 * Proportion, Proportion / 2),
                Position = new Vector2f((window.Size.X - 1.5f * Proportion) / 2, window.Size.Y - 6 * Proportion),
                FillColor = Color.White,
                CharacterSize = (uint)Proportion
            };
        }

        public void IncClicked(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Menu && Menumode.SubScene != Menumode.SubScenes.Options)
                return;
            Program.Volume += 5;
            SaveVolume();
        }

        public void DecClicked(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Menu && Menumode.SubScene != Menumode.SubScenes.Options)
                return;
            Program.Volume -= 5;
            SaveVolume();
        }

        private void SaveVolume()
        {
            VolumeText = new Text(Program.Volume.ToString(), new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"), (uint)Proportion)
            {
                Origin = new Vector2f(Program.Volume.ToString().Length / 2 * Proportion, Proportion / 2),
                Position = new Vector2f((window.Size.X - 1.5f * Proportion) / 2, window.Size.Y - 6 * Proportion),
            };

            Program.BackGroundMusic.Volume = Program.Volume;
        }

        public void ExitReleased(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Menu && Menumode.SubScene != Menumode.SubScenes.Options)
                return;
            Menumode.SubScene = Menumode.SubScenes.Start;
        }

        public override void Draw()
        {
            base.Draw();
            window.Draw(VolumeText);
        }
    }
}
