using System;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using System.Collections.Generic;

namespace GameOfYear.Gamemode
{
    class Player : Elemental
    {
        private Vector2i animation;

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

        private RenderWindow window;

        public readonly Sprite sprite;

        private World world;

        
        

        private Vector2i posMap;

        public int PosX
        {
            get
            {
                return posMap.X;
            }
        }
        public int PosY
        {
            get
            {
                return posMap.Y;
            }
        }

        private Move[][] MovesForType = new Move[6][];

        public Player(int proportion, RenderWindow window, World world) : base("Player")
        {
            Proportion = proportion;

            this.world = world;
            this.window = window;

            FileStream stream = File.Open(@"..\..\..\File\FileMAP\map" + world.CurrentMap + "\\map" + world.CurrentMap + ".ps", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);
            posMap = new Vector2i(br.ReadInt32(), br.ReadInt32());

            stream.Close();
            br.Close();

            this.sprite = new Sprite(new Texture(@"..\..\..\File\FilePNG\Sprite\Sprite.png"), new IntRect(0, 0, 255, 255));
            
            sprite.Scale = new Vector2f(3 * proportion / 45, 3 * proportion / 45);
            sprite.Position = new Vector2f(PosX * proportion + proportion / 2, PosY * proportion + proportion / 2);
            sprite.TextureRect = new IntRect(0, 4 * (int)type.type * 32, 16, 32);
            sprite.Origin = new Vector2f(sprite.GetLocalBounds().Left + sprite.GetLocalBounds().Width / 2, sprite.GetLocalBounds().Top + sprite.GetLocalBounds().Height / 2);

            animation = new Vector2i(0, 0);
        }

        public Player(Player aux) : this(aux.proportion, aux.window, aux.world)
        {

        }

        public void KeyPressed(KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Enter && world.isNPC(animation.Y, posMap))
                return;

            int frame = 4;

            switch(e.Code)
            {
                case Keyboard.Key.W:
                    animation.Y = 2;
                    if (world.IsWalkable(PosX, PosY - 1) && world.ChangeMap(PosX, PosY - 1) == -1)
                    {
                        posMap.Y--;
                        for (int i = 0; i < frame; i++)
                        {
                            sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y - Proportion / frame);
                        }
                    }
                    else if (world.ChangeMap(PosX, PosY - 1) != -1)
                    {
                        ChangeMap(world.CurrentMap, world.ChangeMap(PosX, PosY - 1));
                    }
                    break;
                case Keyboard.Key.S:
                    animation.Y = 0;
                    if (world.IsWalkable(PosX, PosY + 1) && world.ChangeMap(PosX, PosY + 1) == -1)
                    {
                        posMap.Y++;
                        sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y + Proportion);
                    }
                    else if(world.ChangeMap(PosX, PosY + 1) != -1)
                    {
                        ChangeMap(world.CurrentMap, world.ChangeMap(PosX, PosY + 1));
                    }
                    break;
                case Keyboard.Key.A:
                    animation.Y = 3;
                    if (world.IsWalkable(PosX - 1, PosY) && world.ChangeMap(PosX - 1, PosY) == -1)
                    {
                        posMap.X--;
                        sprite.Position = new Vector2f(sprite.Position.X - Proportion, sprite.Position.Y);
                    }
                    else if (world.ChangeMap(PosX - 1, PosY) != -1)
                    {
                        ChangeMap(world.CurrentMap, world.ChangeMap(PosX - 1, PosY));
                    }
                    break;
                case Keyboard.Key.D:
                    animation.Y = 1;
                    if (world.IsWalkable(PosX + 1, PosY) && world.ChangeMap(PosX + 1, PosY) == -1)
                    {
                        posMap.X++;
                        sprite.Position = new Vector2f(sprite.Position.X + Proportion, sprite.Position.Y);
                    }
                    else if (world.ChangeMap(PosX + 1, PosY) != -1)
                    {
                        ChangeMap(world.CurrentMap, world.ChangeMap(PosX + 1, PosY));
                    }
                    break;
            }
            animation.X = (animation.X + 1) % 4;
            sprite.TextureRect = new IntRect(animation.X * 16, (animation.Y + 4 * (int)type.type) * 32, 16, 32);
        }

        public void TalkToNPC()
        {

        }

        public void ChangeMap(int OLD, int NEW)
        {
            if (NEW == -1)
                return;

            world.CurrentMap = NEW;
            


            FileStream stream = File.Open(@"..\..\..\File\FileMAP\map" + OLD + "\\map" + OLD + ".ps", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(PosX);
            bw.Write(PosY);

            stream.Close();
            bw.Close();

            stream = File.Open(@"..\..\..\File\FileMAP\map" + NEW + "\\map" + NEW + ".ps", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);
            posMap.X = br.ReadInt32();
            posMap.Y = br.ReadInt32();

            stream.Close();
            br.Close();

            sprite.Position = new Vector2f(PosX * proportion + proportion / 2, PosY * proportion + proportion / 2);


            stream = File.Open(@"..\..\..\File\FileMAP\CurrentMap.cmp", FileMode.Create);
            bw = new BinaryWriter(stream);

            bw.Write(NEW);

            stream.Close();
            bw.Close();
        }

        public void DrawnPlayer()
        {
            window.Draw(sprite);
        }

        public bool EditMove(string moveName)
        {
            int nullCount = 0;
            for(int i = 0;i < moves.Length; i++)
            {
                if(moves[i] == null)
                    nullCount++;
            }

            if (nullCount == 3)
                return false;

            for(int i = 0; i < moves.Length; i++)
            {
                if (moves[i] != null && moves[i].name == moveName)
                {
                    moves[i] = null;
                    Print();
                    return true;
                }
            }

            return false;
        }

        public override void ChangeType(int type)
        {
            int aux = (int)this.type.type;
            MovesForType[aux] = moves;
            this.type = new Type(type);
            FileStream stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + this.name + "\\" + this.name + type + ".pm", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            Affinity = br.ReadInt32();

            AllMoves.Clear();
            moves = new Move[4];

            for (int i = br.ReadInt32(); i > 0; i--)
            {
                string move = br.ReadString();
                int affinity = br.ReadInt32();

                AllMoves.Add(move, affinity);

                if (MovesForType[type] != null)
                    continue;

                if (affinity <= this.Affinity)
                {
                    if (moves[moves.Length - 1] != null)
                    {
                        for (int j = 0; j < moves.Length - 1; j++)
                        {
                            moves[j] = moves[j + 1];
                        }
                        moves[moves.Length - 1] = null;
                    }


                    for (int j = 0; j < moves.Length; j++)
                    {
                        if (moves[j] == null)
                        {
                            moves[j] = new Move(move);
                            j = int.MaxValue - 1;
                        }
                    }
                }
            }
            if (moves[0] == null && moves[1] == null && moves[2] == null && moves[3] == null)
                moves = MovesForType[type];

            stream.Close();
            br.Close();

            sprite.TextureRect = new IntRect(animation.X * 16, (animation.Y + 4 * (int)this.type.type) * 32, 16, 32);
        }

        public bool EditMove(Move move)
        {
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == null && AllMoves[move.name] <= Affinity)
                {
                    moves[i] = move;
                    Print();
                    return true;
                }
            }
            return false;
        }
    }
}
