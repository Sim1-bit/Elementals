using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Create;

namespace GameOfYear
{
    abstract class Menu
    {
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

        protected List<List<Button>> buttons;

        protected Color backGround;

        protected RenderWindow window;

        public Menu(Color backGround, RenderWindow window, int proportion) 
        {
            this.backGround = backGround;
            this.window = window;
            Proportion = proportion;
            buttons = new List<List<Button>>();
        }

        public virtual void Draw()
        {
            View view = new View(new FloatRect(0, 0, window.Size.X, window.Size.Y));
            view.Center = new Vector2f(window.Size.X / 2, window.Size.Y / 2);
            view.Zoom(1f);
            window.SetView(view);

            window.Clear(backGround);
            for(int i = 0; i < buttons.Count; i++)
            {
                for(int j = 0; j < buttons[i].Count; j++)
                {
                    buttons[i][j].Update(window);
                    buttons[i][j].Draw(window);
                }
            }
        }
    }
}
