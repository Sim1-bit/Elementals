using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Create;
using GameOfYear.Menumode;

namespace GameOfYear.Gamemode
{
    class Gamemode
    {
        public enum SubScenes
        {
            Movement,
            EditingPlayer,
            Fight
        }
        public static SubScenes SubScene;

        private static Gamemode instance;
        private static readonly object instanceLock = new object();

        public readonly World world;
        public readonly Player player;

        private RenderWindow window;

        static MenuEdit menuEdit;
        public static Fight fight;
        

        public static Gamemode Instance(RenderWindow window, int proportion)
        {
            lock(instanceLock)
            {
                if(instance == null)
                {
                    instance = new Gamemode(window, proportion);
                }
            }
            return instance;
        }

        public static Gamemode Instance()
        {
            return instance;
        }

        private Gamemode(RenderWindow window, int proportion)
        {
            this.window = window;
            world = new World(proportion, window);
            player = new Player(proportion, window, world);
            SubScene = SubScenes.Movement;
            menuEdit = MenuEdit.Instance(proportion, window, player);
            fight = Fight.Instance(window, proportion, player);
        }

        public void Draw()
        {
            switch (SubScene)
            {
                case SubScenes.Movement:
                    View view = new View(new FloatRect(0, 0, window.Size.X, window.Size.Y));
                    view.Center = player.sprite.Position;
                    view.Zoom(0.6f);
                    window.SetView(view);

                    world.DrawMap();
                    player.DrawnPlayer();
                    break;
                case SubScenes.EditingPlayer:
                    menuEdit.Draw();
                    break;
                case SubScenes.Fight:
                    fight.Draw();
                    break;
            }
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            if (Program.Scene != Program.Scenes.Game)
                return;


            switch (SubScene)
            {
                case SubScenes.Movement:

                    if (e.Code == Keyboard.Key.W || e.Code == Keyboard.Key.A || e.Code == Keyboard.Key.S || e.Code == Keyboard.Key.D || e.Code == Keyboard.Key.Enter)
                        player.KeyPressed(e);
                    else if (e.Code == Keyboard.Key.Escape)
                    {
                        Menumode.Menumode.SubScene = Menumode.Menumode.SubScenes.Start;
                        Program.Scene = Program.Scenes.Menu;
                    }
                    else if (e.Code == Keyboard.Key.Tab)
                    {
                        SubScene = SubScenes.EditingPlayer;
                    }

                    break;
                case SubScenes.EditingPlayer:

                    if (e.Code == Keyboard.Key.Tab)
                    {
                        SubScene = SubScenes.Movement;
                    }

                    break;
            }
        }
    }
}
