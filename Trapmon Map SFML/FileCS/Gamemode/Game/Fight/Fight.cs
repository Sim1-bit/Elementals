using System;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using System.Collections.Generic;
using System.Numerics;
using static SFML.Window.Mouse;
using System.Runtime.CompilerServices;

namespace GameOfYear.Gamemode
{
    class Fight : Menu
    {
        private bool permitChoosing;
        Sprite sprite;
        Player player;
        NPC npc;

        private int indexMovePlayer;

        public bool permitChoice;
        public bool exitFight;

        public enum SubScripts
        {
            moveSelection,
            typeSelection
        }

        public static SubScripts subScript;

        private static Fight instance;
        private static readonly object instanceLock = new object();
        public static Fight Instance(RenderWindow window, int proportion, Player player)
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new Fight(proportion, window, player);
                }
            }
            return instance;
        }

        public static Fight Instance()
        {
            return instance;
        }

        private Fight(int proportion, RenderWindow window, Player player/*, NPC bot*/) : base(Color.Green, window, proportion)
        {
            sprite = new Sprite();
            this.player = player;
            //this.npc = bot;
            buttons.Add(AddMoveButtons());
            buttons.Add(AddAuxButtons());
            subScript = SubScripts.moveSelection;
        }

        public void ReloadMoveButtons()
        {
            buttons[0] = AddMoveButtons();
        }

        private List<Button> AddMoveButtons()
        {
            int proportion = 20;
            Vector2f pos;
            Text text = new Text("", new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"), (uint)proportion);
            text.FillColor = Color.Black;
            List<Button> moveButtons = new List<Button>();

            for (int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    if (player[i * 2 + j] != null)
                    {
                        pos = new Vector2f(i * 20 * proportion + 15 + 15 * i, window.Size.Y - 2 * proportion - 15 - (15 + proportion) * j);
                        string aux = player[i * 2 + j].name;
                        while (aux.Length < 20)
                        {
                            aux += " ";
                        }

                        text.DisplayedString = aux;

                        Button move = new Button(pos, Color.White, text);
                        //move.MouseOn += MouseOn;
                        //move.MouseLeft += MouseLeft;
                        move.Releas += MovesReleased;
                        moveButtons.Add(move);
                    }
                }
            }
            return moveButtons;
        }

        public void StartFight(NPC npc)
        {
            this.npc = npc;
            permitChoice = true;
            exitFight = false;

            this.npc.Print();
        }

        public void MovesReleased(object sender, EventArgs e)
        {
            if(!permitChoice)
                return;
            string aux = (sender as Button).text.DisplayedString;
            int i = aux.Length - 1;
            while (aux[i] == ' ')
            {
                aux = aux.Remove(i);
                i--;
            }

            indexMovePlayer = player.IndexMoves(aux);

            
            if (player.DontUseMove(indexMovePlayer) != null)
            {
                permitChoice = false;
                KeepFighting();
            }
        }

        private void KeepFighting()
        {
            if (permitChoice || npc.LifeRemaing == 0 || player.LifeRemaing == 0)
                return;
            int indexMoveNPC;
            do
            {
                indexMoveNPC = new Random().Next(0, 4);
            }
            while (npc.DontUseMove(indexMoveNPC) == null);
            bool aux;

            if (npc[indexMoveNPC].Priority > player[indexMovePlayer].Priority)
                aux = false;
            else if (npc[indexMoveNPC].Priority < player[indexMovePlayer].Priority)
                aux = true;
            else
            {
                if (npc.Speed > player.Speed)
                    aux = false;
                else if (npc.Speed < player.Speed)
                    aux = true;
                else
                    aux = Convert.ToBoolean(new Random().Next(0, 2));
            }

            for (int i = 0; i < 2; i++)
            {
                if (npc.LifeRemaing == 0 || player.LifeRemaing == 0)
                    continue;

                if (aux)
                    npc.DamageApplication(player, player.UseMove(indexMovePlayer));
                else
                    player.DamageApplication(npc, npc.UseMove(indexMoveNPC));

                aux = !aux;
            }

            Console.WriteLine("PLAYER");
            Console.WriteLine("   Life = {0}", player.LifeRemaing);
            Console.WriteLine("   Stamina = {0}\n", player.StaminaRemaing);

            Console.WriteLine("NPC");
            Console.WriteLine("   Life = {0}", npc.LifeRemaing);
            Console.WriteLine("   Stamina = {0}\n",npc.StaminaRemaing);

            permitChoice = true;
        }

        private List<Button> AddAuxButtons()
        {
            int proportion = 20;
            
            Text text = new Text("", new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"), (uint)proportion);
            text.FillColor = Color.Black;
            List<Button> AuxList = new List<Button>();

            Vector2f pos = new Vector2f(window.Size.X - 7 * proportion - 15, window.Size.Y - 2 * proportion - 15 - (15 + proportion));

            text.DisplayedString = " Types ";

            Button aux = new Button(pos, Color.White, text);
            aux.Click += TypesClick;
            AuxList.Add(aux);

            pos = new Vector2f(window.Size.X - 7 * proportion - 15, window.Size.Y - 2 * proportion - proportion - 15 + proportion);

            text.DisplayedString = "  Run  ";

            Button aux1 = new Button(pos, Color.White, text);
            aux1.Click += RunClick;
            AuxList.Add(aux1);

            return AuxList;
        }

        public void TypesClick(object sender, EventArgs e)
        {
            subScript = SubScripts.typeSelection;
        }

        public void RunClick(object sender, EventArgs e)
        {
            exitFight = true;
            Gamemode.SubScene = Gamemode.SubScenes.Movement;
        }

        public override void Draw()
        {
            switch (subScript)
            {
                case SubScripts.moveSelection:
                    base.Draw();
                    sprite = new Sprite(player.sprite)
                    {
                        Scale = new Vector2f(15, 15),
                        Position = new Vector2f(Proportion * 3, window.Size.Y / 2 - 20),
                        TextureRect = new IntRect(0, 4 * (int)player.type.type * 32, 16, 32)
                    };
                    window.Draw(sprite);
                    sprite.Position = new Vector2f(window.Size.X - Proportion * 3, window.Size.Y / 2 - 20);
                    sprite.TextureRect = new IntRect(0, 4 * (int)npc.type.type * 32, 16, 32);
                    window.Draw(sprite);
                    break;
                case SubScripts.typeSelection:
                    MenuEdit.Instance().Draw();
                    break;
            }
        }
    }
}
