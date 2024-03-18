using GameOfYear;
using GameOfYear.Gamemode;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Mouse;

namespace GameOfYear.Menumode
{
    class Menumode
    {
        public enum SubScenes
        {
            Start,
            Options
        }
        public static SubScenes SubScene;

        private static Menumode instance;
        private static readonly object instanceLock = new object();

        static MenuStart start;
        static MenuOptions options;

        private RenderWindow window;

        private int proportion;
        public int Proportion
        {
            get
            {
                return proportion;
            }
            set
            {
                if (value <= 0)
                {
                    proportion = 1;
                }
                else
                {
                    proportion = value;
                }
            }
        }
        public static Menumode Instance(RenderWindow window, int proportion)
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new Menumode(window, proportion);
                }
            }
            return instance;
        }

        public static Menumode Instance()
        {
            return instance;
        }

        private Menumode(RenderWindow window, int proportion)
        {
            start = MenuStart.Instance(window, (int)proportion);
            options = MenuOptions.Instance(window, (int)proportion);
        }

        public void Draw()
        {
            switch(SubScene)
            {
                case SubScenes.Start:
                    start.Draw();
                    break;
                case SubScenes.Options:
                    options.Draw();
                    break;
            }
        }
    }
}
