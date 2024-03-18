using System;
using System.IO;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Gamemode;

namespace GameOfYear
{
    class Map
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

        public Dictionary<int, IntRect> book = new Dictionary<int, IntRect>();

        private int[,] map;

        public int this[int x, int y]
        {
            get
            {
                return map[x, y];
            }
        }

        private List<NPC> NPCs = new List<NPC>();

        public Map(int proportion, int NumberBook)
        {
            FileStream stream = File.Open(@"..\..\..\File\FileMAP\map" + NumberBook + "\\map" + NumberBook + ".mp", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            this.proportion = proportion;
            this.map = new int[br.ReadInt32(), br.ReadInt32()];
            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    this.map[i, j] = br.ReadInt32();
                }
            }

            stream.Close();

            stream = File.Open(@"..\..\..\File\FileMAP\map" + NumberBook + "\\map" + NumberBook + ".bk", FileMode.Open);
            br = new BinaryReader(stream);

            for (int i = 0, j = br.ReadInt32(); i < j; i++)
            {
                book[br.ReadInt32()] = new IntRect(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
            }

            stream.Close();

            stream = File.Open(@"..\..\..\File\FileMAP\map" + NumberBook + "\\map" + NumberBook + ".npc", FileMode.Open);
            br = new BinaryReader(stream);

            for (int i = 0, j = br.ReadInt32(); i < j; i++)
            {
                NPCs.Add(new NPC(proportion, br.ReadInt32(), new Vector2i(br.ReadInt32(), br.ReadInt32())));
            }

            stream.Close();
            br.Close();
        }

        public bool IsWalkable(int x, int y)
        {
            try
            {
                for (int i = 0; i < NPCs.Count; i++)
                {
                    if (NPCs[i].PosX == x && NPCs[i].PosY == y)
                        return false;
                }
                return map[x, y] % 2 == 0 ? true : false;
            }
            catch (IndexOutOfRangeException Error)
            {
                Console.WriteLine(Error.Message);
                return false;
            }
        }

        public bool isNPC(int movPlayer, Vector2i posPlayer)
        {
            foreach(var item in NPCs)
            {
                if (item.isTalking(movPlayer, posPlayer))
                {
                    Fight.Instance().StartFight(item);
                    return true;
                }
                    
            }
            return false;
        }

        public void Draw(RenderWindow window, Sprite sprite)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    sprite.Position = new Vector2f(i * proportion, j * proportion);
                    sprite.TextureRect = book[map[i, j]];
                    window.Draw(sprite);
                }
            }

            if (NPCs.Count <= 0)
                return;
            Vector2f auxOrigin = new Vector2f(sprite.Origin.X, sprite.Origin.Y);
            sprite.Texture.Dispose();
            GC.Collect();
            sprite.Texture = new Texture(@"..\..\..\File\FilePNG\Sprite\Sprite.png");

            sprite.TextureRect = new IntRect(0, 0, 16, 32);
            sprite.Origin = new Vector2f(sprite.GetLocalBounds().Left + sprite.GetLocalBounds().Width / 2, sprite.GetLocalBounds().Top + sprite.GetLocalBounds().Height / 2);
            for (int i = 0; i < NPCs.Count; i++)
            {
                sprite.TextureRect = NPCs[i].Crop;
                sprite.Position = new Vector2f(NPCs[i].PosX * proportion + proportion / 2, NPCs[i].PosY * proportion + proportion / 2);
                window.Draw(sprite);
            }
            sprite.Origin = new Vector2f(auxOrigin.X, auxOrigin.Y);
            sprite.Texture.Dispose();
            GC.Collect();
            sprite.Texture = new Texture(@"..\..\..\File\FilePNG\Map\All.png");
        }
    }
}
