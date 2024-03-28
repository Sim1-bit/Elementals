using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using System.Linq;
using System.Threading;

namespace GameOfYear.Gamemode
{
    abstract class Elemental
    {
        public readonly string name;

        #region Type
        public Type type;

        #region Affinity
        private int affinity;
        public int Affinity
        {
            get => affinity;
            set
            {
                if (value > 0 && value <= 100)
                {
                    affinity = value;
                }
                else if (value <= 0)
                {
                    affinity = 1;
                }
                else
                {
                    affinity = 100;
                }
            }
        }
        #endregion Affinity

        #endregion Type

        #region Stats

        #region Level
        private int level;
        public int Level
        {
            get => level;
            set
            {
                if (value > 0 && value <= 100)
                {
                    level = value;
                }
                else if (value <= 0)
                {
                    level = 1;
                }
                else
                {
                    level = 100;
                }
            }
        }
        #endregion Level

        #region Speed
        private int speed;
        public int Speed
        {
            get => speed;
            set
            {
                if (value > 0)
                {
                    speed = value;
                }
                else if (value <= 0)
                {
                    speed = 1;
                }
            }
        }
        #endregion Speed
        #region Attack
        private int attack;
        public int Attack
        {
            get => attack;
            set
            {
                if (value > 0)
                {
                    attack = value;
                }
                else
                {
                    attack = 1;
                }
            }
        }
        #endregion Attack
        #region Defence
        private int defence;
        public int Defence
        {
            get => defence;
            set
            {
                if (value > 0)
                {
                    defence = value;
                }
                else if (value <= 0)
                {
                    defence = 1;
                }
            }
        }
        #endregion Defence
        #region Life
        private int life;
        public int Life
        {
            get => life;
            set
            {
                if (value > 0)
                {
                    life = value;
                }
                else if (value <= 0)
                {
                    life = 1;
                }
            }
        }
        #endregion Life
        #region Stamina
        private int stamina;

        public int Stamina
        {
            get => stamina;
            set
            {
                if (value > 0)
                {
                    stamina = value;
                }
                else if (value <= 0)
                {
                    stamina = 1;
                }
            }
        }
        #endregion Stamina

        #endregion Stats

        #region LifeRemaing
        private int lifeRemaing;
        public int LifeRemaing
        {
            get => lifeRemaing;
            set
            {
                if (value >= 0 && value <= Life)
                {
                    lifeRemaing = value;
                }
                else if (value < 0)
                {
                    lifeRemaing = 0;
                }
                else
                {
                    lifeRemaing = life;
                }
            }
        }
        #endregion LifeRemaing
        #region StaminaRemaing
        private int staminaRemaing;

        public int StaminaRemaing
        {
            get => staminaRemaing;
            set
            {
                if (value >= 0 && value <= Stamina)
                {
                    staminaRemaing = value;
                }
                else if (value < 0)
                {
                    staminaRemaing = 0;
                }
                else
                {
                    staminaRemaing = life;
                }
            }
        }
        #endregion StaminaRemaing


        public readonly Dictionary<string, int> AllMoves = new Dictionary<string, int>();
        protected Move[] moves = new Move[4];

        public Move this[int index]
        {
            get => moves[index];
        }

        public int IndexMoves(string name)
        {
            for(int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == null)
                    continue;
                else if (moves[i].name == name)
                    return i;
            }

            return -1;
        }

        public Elemental(string name)
        {
            if (!File.Exists(@"..\..\..\File\FileEL\Elementals\" + name + "\\" + name + ".el"))
            {
                name = "Player";
            }

            this.name = name;

            FileStream stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + this.name + "\\" + this.name + ".el", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            Level = 1;

            Speed = br.ReadInt32();
            Attack = br.ReadInt32();
            Defence = br.ReadInt32();
            Life = br.ReadInt32();
            LifeRemaing = Life;
            Stamina = br.ReadInt32();
            StaminaRemaing = Stamina;

            stream.Close();
            br.Close();

            int type = 0;
            for (int i = 0; i <= (int)Type.Types.Electric; i++)
            {
                if (File.Exists(@"..\..\..\File\FileEL\Elementals\" + this.name + "\\" + this.name + i + ".pm"))
                {
                    type = i;
                }
            }
            this.type = new Type(type);
            stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + this.name + "\\" + this.name + type + ".pm", FileMode.Open);
            br = new BinaryReader(stream);

            Affinity = br.ReadInt32();

            for (int i = br.ReadInt32(); i > 0; i--)
            {
                string move = br.ReadString();
                int affinity = br.ReadInt32();

                AllMoves.Add(move, affinity);
                if (affinity <= Affinity)
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

            stream.Close();
            br.Close();
        }

        public bool movesContains(string move)
        {
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] != null && moves[i].name == move)
                {
                    return true;
                }
            }
            return false;
        }
        private int DamageCalculator(in Elemental Sender, in Move move)
        {
            float Effective;
            if (move == null)
                return 0;

            if (move.type.type == type.type || move.type.weaknesses.Contains(type.type))
            {
                Effective = 0.5f;
            }
            else if (type.weaknesses.Contains(move.type.type))
            {
                Effective = 2;
            }
            else
            {
                Effective = 1;
            }

            float stab = Sender.type.type == move.type.type ? 1.5f : 1;

            return (int)Math.Round(((Sender.Affinity * 2) / 7 * Sender.Attack * move.Damage / Defence / 50 + 2) * stab * Effective * new Random().Next(85, 101) / 100);
        }
        public void DamageApplication(in Elemental Sender, in Move move)
        {
            Logger.Logger.Write(name + ": ");
            Logger.Logger.WriteLine(DamageCalculator(Sender, move).ToString());
            LifeRemaing -= DamageCalculator(Sender, move);
        }

        public virtual Move UseMove(int index)
        {
            object aux = AvailableMove(index);
            Move auxMove;

            if (aux == null)
                auxMove = new Move("Scontro");
            else
                auxMove = moves[index];

            if (new Random().Next(1, 101) > moves[index].Precision)
                return null;

            LifeRemaing -= auxMove.Recoil;
            StaminaRemaing -= auxMove.Stamina;
            return auxMove;
        }

        public object AvailableMove(int index)
        {
            bool bootableMove = false;
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == null)
                    continue;

                if (moves[i].Stamina <= StaminaRemaing)
                {
                    bootableMove = true;
                    break;
                }
            }

            if (!bootableMove)
                return null;

            if (StaminaRemaing < moves[index].Stamina)
                return false;

            return true;
        }

        public abstract void ChangeType(int type);

        public void Print()
        {
            Console.WriteLine("Name = {0}", name);
            Console.WriteLine(" TYPE");
            Console.WriteLine("     Type = {0}", type.type);
            Console.WriteLine("     Affinity = {0}", Affinity);
            Console.WriteLine(" STATS");
            Console.WriteLine("     Level = {0}", Level);
            Console.WriteLine("     Attack = {0}", Attack);
            Console.WriteLine("     Defence = {0}", Defence);
            Console.WriteLine("     Speed = {0}", Speed);
            Console.WriteLine("     Stamina = {0}", Stamina);
            Console.WriteLine(" ALL MOVES");
            foreach (var item in AllMoves)
            {
                Console.WriteLine("     {0} = {1}", item.Key, item.Value);
            }
            Console.WriteLine(" MOVES");
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] != null)
                    Console.WriteLine("     Name = {0}", moves[i].name);
            }
        }
    }
}
