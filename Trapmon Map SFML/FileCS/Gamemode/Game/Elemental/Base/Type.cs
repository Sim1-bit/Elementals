using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using System.Linq;

namespace GameOfYear.Gamemode
{
    class Type
    {
        public enum Types
        {
            Normal,
            Fire,
            Water,
            Grass,
            Ground,
            Electric
        }

        public readonly Types[] weaknesses;

        public readonly Types type;

        public Type(int type)
        {
            if (File.Exists(@"..\..\..\File\FileTP\type" + type + ".tp"))
            {
                this.type = (Types)type;
            }
            else
            {
                type = 0;
            }

            FileStream stream = File.Open(@"..\..\..\File\FileTP\type" + (int)this.type + ".tp", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            weaknesses = new Types[br.ReadInt32()];

            for (int i = 0; i < weaknesses.Length; i++)
            {
                weaknesses[i] = (Types)br.ReadInt32();
            }

            stream.Close();
            br.Close();
        }
    }
}
