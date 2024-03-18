using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GameOfYear.Gamemode
{
    class Move
    {
        public readonly string name;

        #region Damage
        private int damage;
        public int Damage
        {
            get => damage;

            set => damage = value;
        }
        #endregion Damage

        #region Precision
        private int precision;
        public int Precision
        {
            get => precision;

            set
            {
                if (value > 0 && value <= 100)
                {
                    precision = value;
                }
                else
                {
                    precision = 50;
                }
            }
        }
        #endregion Precision

        #region Stamina
        private int stamina;
        public int Stamina
        {
            get => stamina;

            set => stamina = value;
        }
        #endregion Stamina

        #region Priority
        private int priority;
        public int Priority
        {
            get => priority;
            set
            {
                if (value >= -1 && value < 6)
                {
                    priority = value;
                }
                else
                {
                    priority = 0;
                }
            }
        }
        #endregion Priority

        #region Recoil
        private int recoil;
        public int Recoil
        {
            get => recoil;

            set => recoil = value;
        }
        #endregion Recoil

        public readonly Type type;


        public Move(string name)
        {
            if (!File.Exists(@"..\..\..\File\FileEL\Moves\" + name + "\\" + name + ".mv"))
            {
                name = "Calcio";
            }

            this.name = name;

            FileStream stream = File.Open(@"..\..\..\File\FileEL\Moves\" + name + "\\" + name + ".mv", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            type = new Type(br.ReadInt32());

            Damage = br.ReadInt32();
            Precision = br.ReadInt32();
            Stamina = br.ReadInt32();
            Recoil = br.ReadInt32();
            Priority = br.ReadInt32();


            stream.Close();
            br.Close();
        }
    }
}
