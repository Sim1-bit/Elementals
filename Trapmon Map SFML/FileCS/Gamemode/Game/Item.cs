using System;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;

namespace GameOfYear.Gamemode
{
    class Item
    {
        public readonly string name;
        public readonly string path;

        public readonly IntRect crop;

        public Item(string name)
        {
            FileStream stream = File.Open(@"..\..\..\File\FileITEM\" + name + "\\" + name + ".item", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            this.name = name;
            path = br.ReadString();
            crop = new IntRect(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());

            stream.Close();
            br.Close();
        }
    }
}
