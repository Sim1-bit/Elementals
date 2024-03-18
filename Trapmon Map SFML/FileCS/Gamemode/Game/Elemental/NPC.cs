using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Gamemode;
using GameOfYear.Logger;
using GameOfYear.Create;
using GameOfYear.Menumode;
using GameOfYear;
using static SFML.Window.Mouse;
using System.Threading;

namespace GameOfYear.Gamemode
{
    class NPC : Elemental
    {
        public enum NPCs
        {
            Mr_Buchetto_per_Popo,
            Mr_Mi_Guardi,
            Cetriolo_Rick,
            Gamba_Rick,
            Mr_Frundles,
            This_is_a_gun__to_shoot_bad_people
        }

        private Vector2i animation;

        private int proportion;

        private IntRect crop;
        public IntRect Crop
        {
            get => crop;
        }

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

        public NPC(int proportion, int numBot, Vector2i pos) : base(((NPCs)numBot).ToString())
        {
            Proportion = proportion;
            posMap = pos;

            crop = new IntRect(0, 4 * (int)type.type * 32, 16, 32);

            animation = new Vector2i(0, 0);
        }

        public bool isTalking(int movPlayer, Vector2i posPlayer)
        {
            Vector2i aux = new Vector2i(0, 0);
            switch ((movPlayer + 2) % 4)
            {
                case 0:
                    aux.Y -= 1;
                    break;
                case 1:
                    aux.X -= 1;
                    break;
                case 2:
                    aux.Y += 1;
                    break;
                case 3:
                    aux.X += 1;
                    break;
            }
            if (posPlayer.X + aux.X == PosX && posPlayer.Y + aux.Y == PosY)
            {
                Talk((movPlayer + 2) % 4);
                return true;
            }
            return false;
        }
        public void Talk(int mov)
        {
            animation.Y = mov;
            crop = new IntRect(animation.X * 16, (animation.Y + 4 * (int)type.type) * 32, 16, 32);
            
            Gamemode.SubScene = Gamemode.SubScenes.Fight;
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
        }
    }
}
