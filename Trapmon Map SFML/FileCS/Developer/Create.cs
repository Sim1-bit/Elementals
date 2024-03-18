using GameOfYear.Gamemode;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameOfYear.Create
{
    static class Create
    {
        public static void CreateMap(int Number, int[,] map, Dictionary<int, IntRect> Aux, string path, Vector2i LastPos)
        {
            Directory.CreateDirectory(@"..\..\..\File\FileMAP\map" + Number);
            FileStream stream = File.Open(@"..\..\..\File\FileMAP\map" + Number + "\\map" + Number + ".mp", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(map.GetLength(0));
            bw.Write(map.GetLength(1));
            foreach (int item in map)
            {
                bw.Write(item);
            }

            stream.Close();

            stream = File.Open(@"..\..\..\File\FileMAP\map" + Number + "\\map" + Number + ".bk", FileMode.Create);
            bw = new BinaryWriter(stream);

            bw.Write(path);

            bw.Write(Aux.Count);
            foreach (var item in Aux)
            {
                bw.Write(item.Key);
                bw.Write(item.Value.Left);
                bw.Write(item.Value.Top);
                bw.Write(item.Value.Width);
                bw.Write(item.Value.Height);
            }
            stream.Close();
            stream = File.Open(@"..\..\..\File\FileMAP\map" + Number + "\\map" + Number + ".ps", FileMode.Create);
            bw = new BinaryWriter(stream);
            bw.Write(LastPos.X);
            bw.Write(LastPos.Y);

            stream.Close();
            bw.Close();
        }

        public static void CreateItem(string name, string path,IntRect crop)
        {
            Directory.CreateDirectory(@"..\..\..\File\FileITEM\" + name);
            FileStream stream = File.Open(@"..\..\..\File\FileITEM\" + name + "\\" + name + ".item", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(path);
            bw.Write(crop.Left);
            bw.Write(crop.Top);
            bw.Write(crop.Width);
            bw.Write(crop.Height);
        }

        public static void CreateBook(int mapNumber,Dictionary<int,IntRect> Aux) 
        {
            FileStream stream = File.Open(@"..\..\..\File\FileMAP\map" + mapNumber + "\\map" + mapNumber + ".bk", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(Aux.Count);
            foreach (var item in Aux)
            {
                bw.Write(item.Key);
                bw.Write(item.Value.Left);
                bw.Write(item.Value.Top);
                bw.Write(item.Value.Width);
                bw.Write(item.Value.Height);
            }
            stream.Close();
            bw.Close();
        }


        public struct npcElement
        {
            public Gamemode.NPC.NPCs id;
            public Vector2i pos;

            public npcElement(int id, Vector2i pos)
            {
                this.id = (Gamemode.NPC.NPCs)id;
                this.pos = pos;
            }
        }
        public static void CreateNPC_List(List<npcElement> npcs, int mapNumber)
        {
            FileStream stream = File.Open(@"..\..\..\File\FileMAP\map" + mapNumber + "\\map" + mapNumber + ".npc", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(npcs.Count);
            foreach(var item in npcs)
            {
                bw.Write((int)item.id);
                bw.Write(item.pos.X);
                bw.Write(item.pos.Y);
            }

            stream.Close();
            bw.Close();
        }

        public static void CreateType(int Number, int[] weaknesses)
        {
            Directory.CreateDirectory(@"..\..\..\File\FileTP\");

            FileStream stream = File.Open(@"..\..\..\File\FileTP\type" + Number + ".tp", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(weaknesses.Length);

            foreach (var item in weaknesses)
            {
                bw.Write((int)item);
            }

            stream.Close();
            bw.Close();
        }

        //Boh
        public static void CreateAffinity(int type)
        {
            FileStream stream = File.Open(@"..\..\..\File\FilePL\type\affinity\type" + type + ".af", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(0);
            stream.Close();
            bw.Close();
        }

        //Boh
        public static void CreateLastType(int type)
        {
            FileStream stream = File.Open(@"..\..\..\File\FilePL\type\lastType.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(type);
            stream.Close();
            bw.Close();
        }

        //Boh
        public static void CreateElementalsStats(string name, int speed, int attack, int defence, int life, int stamina)
        {
            Directory.CreateDirectory(@"..\..\..\File\FileEL\Elementals\" + name);

            FileStream stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + name + "\\" + name + ".el", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(speed);
            bw.Write(attack);
            bw.Write(defence);
            bw.Write(life);
            bw.Write(stamina);
            

            stream.Close();
            bw.Close();
        }

        //Boh
        public static void CreateElementalsStats(string name, int speed, int attack, int defence, int life, int stamina, int type)
        {
            Directory.CreateDirectory(@"..\..\..\File\FileEL\Elementals\" + name);

            FileStream stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + name + "\\" + name + ".el", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(speed);
            bw.Write(attack);
            bw.Write(defence);
            bw.Write(life);
            bw.Write(stamina);

            bw.Write(type);

            stream.Close();
            bw.Close();
        }

        public static void CreateElementalsMoves(string name, int type, int affinity, Dictionary<string, int> PM)
        {
            FileStream stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + name + "\\" + name + type + ".pm", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(affinity);

            bw.Write(PM.Count);
            foreach(var item in PM)
            {
                bw.Write(item.Key);
                bw.Write(item.Value);
            }

            stream.Close();
            bw.Close();
        }

        public static void CreateMoves(string name, int type, int damage, int precision, int stamina, int recoil, int priority)
        {
            Directory.CreateDirectory(@"..\..\..\File\FileEL\Moves\" + name);

            FileStream stream = File.Open(@"..\..\..\File\FileEL\Moves\" + name + "\\" + name + ".mv", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(type);
            bw.Write(damage);

            bw.Write(precision);
            bw.Write(stamina);
            bw.Write(recoil);
            bw.Write(priority);

            stream.Close();
            bw.Close();
        }
    }
}
