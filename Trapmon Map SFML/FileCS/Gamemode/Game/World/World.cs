using System;
using System.IO;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Reflection;

namespace GameOfYear
{
    class World
    {
        private Sprite sprite;

        protected Map map;

        private int currentMap;
        public int CurrentMap
        {
            get
            {
                return currentMap;
            }
            set
            {
                if (File.Exists(@"..\..\..\File\FileMAP\map" + value + "\\map" + value + ".mp"))
                {
                    currentMap = value;
                    map = new Map(proportion, currentMap);
                }
            }
        }
        protected RenderWindow window;

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

        public World(int proportion, RenderWindow window) 
        {
            Proportion = proportion;
            this.window = window;

            FileStream stream = File.Open(@"..\..\..\File\FileMAP\CurrentMap.cmp", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            this.CurrentMap = br.ReadInt32();

            stream.Close();
            br.Close();

            map = new Map(Proportion, currentMap);

            sprite = new Sprite();
            sprite.Texture = new Texture(@"..\..\..\File\FilePNG\Map\All.png");
            sprite.Scale = new Vector2f(3 * proportion / 45, 3 * proportion / 45);
        }

        //Cambiare la posizione del giocatore nell'ultima che aveva nella mappa indicata dall'indice
        public int ChangeMap( int x, int y)
        {
            try
            {
                switch ((double)map[x, y] / 10)
                {
                    case 0.0f:
                        return 0;
                    case 1.0f:
                        return 1;
                    case 2.0f:
                        return 2;
                    case 3.0f:
                        return 3;
                }
            }
            catch(IndexOutOfRangeException)
            {
                
            }
            return -1;
        }

        public void DrawMap()
        {
            try
            {
                map.Draw(window, sprite);
            }
            catch (IndexOutOfRangeException)
            {
                map.Draw(window, sprite);
            }
        }

        public bool IsWalkable(int x, int y)
        {
            try
            {
                return map.IsWalkable(x, y);
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public bool isNPC(int movPlayer, Vector2i posPlayer)
        {
            return map.isNPC(movPlayer, posPlayer);
        }
    }
}
