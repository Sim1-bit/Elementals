using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Create;
using GameOfYear.Gamemode;

namespace GameOfYear
{
    class Program
    {
        private static int volume;
        public static int Volume 
        {
            get => volume;
            set
            {
                if(value >=0 && value <= 100)
                {
                    volume = value;
                }
                else if(value < 0)
                {
                    volume = 0;
                }
                else
                {
                    volume = 100;
                }
            }
        }

        public static Sound BackGroundMusic;

        static RenderWindow window;
        const int proportion = 48;
        static Menumode.Menumode menumode;
        static Gamemode.Gamemode gamemode;

        public static Scenes Scene;
        public enum Scenes
        {
            Menu,
            Game
        }
        static void Main(string[] args)
        {
            Dictionary<string, int> parco = new Dictionary<string, int>();
            parco["A"] = 1; 
            parco["B"] = 1; 
            parco["C"] = 1; 
            parco["D"] = 1;
            Create.Create.CreateElementalsStats("Mr_Buchetto_per_Popo", 1, 1, 1, 20, 50);
            Create.Create.CreateElementalsMoves("Mr_Buchetto_per_Popo", 1, 1, parco);

            Create.Create.CreateMoves("A", 0, 1, 100, 5, 0, 5);
            Create.Create.CreateMoves("B", 1, 1, 100, 10, 0, -1);
            Create.Create.CreateMoves("C", 1, 1, 100, 10, 0, -1);
            Create.Create.CreateMoves("D", 3, 1, 100, 10, 0, 0);
            Console.WriteLine(new Move("Miss").Damage);

            {
                FileStream stream = File.Open(@"..\..\..\Volume.dat", FileMode.Open);
                BinaryReader br = new BinaryReader(stream);
                Volume = br.ReadInt32();
                stream.Close();
                br.Close();
            }

            BackGroundMusic = new Sound(new SoundBuffer(@"..\..\..\File\FileWAV\background\bg0.wav"))
            {
                Loop = true,
                Volume = volume
            };
            BackGroundMusic.Play();

            Scene = Scenes.Menu;
            Logger.Logger.Grade = 600;
            window = new RenderWindow(new VideoMode(1080, 640), "Elemental");
            window.SetVerticalSyncEnabled(true);
            window.Closed += (sender, args) => Close();

            menumode = Menumode.Menumode.Instance(window, proportion);
            gamemode = Gamemode.Gamemode.Instance(window, proportion);

            window.KeyPressed += gamemode.KeyPressed;

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                switch (Scene)
                {
                    case Scenes.Menu:
                        menumode.Draw();
                        break;
                    case Scenes.Game:
                        gamemode.Draw();
                        break;
                }
                window.Display();
            }
        }


        public static void Close()
        {
            FileStream stream = File.Open(@"..\..\..\Volume.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(Volume);
            stream.Close();
            bw.Close();

            window.Close();
        }
    }
}
