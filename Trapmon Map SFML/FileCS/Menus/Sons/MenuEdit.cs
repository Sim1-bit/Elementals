using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Create;

namespace GameOfYear.Gamemode
{
    class MenuEdit : Menu
    {
        private static MenuEdit instance;
        private static readonly object instanceLock = new object();

        private Player player;

        private List<Text> texts = new List<Text>();

        private List<Text> typeTexts = new List<Text>();

        private Sprite sprite;

        public RenderWindow subWindow;
        private bool auxSubWindow;

        public static MenuEdit Instance(int proportion, RenderWindow window, Player player)
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new MenuEdit(window, proportion, player);
                }
            }
            return instance;
        }

        public static MenuEdit Instance()
        {
            return instance;
        }

        private MenuEdit(RenderWindow window, int proportion, Player player) : base(Color.Transparent, window, proportion)
        {
            this.player = player;

            sprite = new Sprite(player.sprite)
            {
                Scale = new Vector2f(15, 15),
                Position = new Vector2f(window.Size.X/ 2, window.Size.Y / 2 - 20)
            };

            auxSubWindow = true;

            Text aux = new Text("Types", new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(Proportion / 2 - 15, window.Size.Y - 12.75f * Proportion - 15),
                CharacterSize = 48,
                FillColor = Color.White
            };
            texts.Add(aux);

            Button Exit = new Button(new Vector2f((window.Size.X - 4 * Proportion - 15) / 2, window.Size.Y - 3 * Proportion - 15), "Exit", Proportion, Color.White, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"));
            Exit.Click += ExitClick;
            List<Button> list = new List<Button>();
            list.Add(Exit);
            buttons.Add(list);

            AddTypeButtons();
            AddTypeTexts();
            AddStatsText();
            AddMoveButtons();
        }

        public void ExitClick(object sender, EventArgs e)
        {
            if(Program.Scene == Program.Scenes.Game)
            {
                if(Gamemode.SubScene == Gamemode.SubScenes.EditingPlayer)
                {
                    Gamemode.SubScene = Gamemode.SubScenes.Movement;
                }
                else if(Gamemode.SubScene == Gamemode.SubScenes.Fight && Fight.subScript == Fight.SubScripts.typeSelection)
                {
                    Fight.subScript = Fight.SubScripts.moveSelection;
                }
            }
            
        }

        public void AddTypeTexts()
        {
            int proportion = 24;
            typeTexts.Clear();
            Vector2f pos = new Vector2f(Proportion / 2 + 8 * proportion, window.Size.Y - 12 * Proportion - 15);
            for (int i = 0; i <= (int)Type.Types.Electric; i++)
            {
                if (File.Exists(@"..\..\..\File\FileEL\Elementals\" + player.name + "\\" + player.name + i + ".pm"))
                {
                    FileStream stream = File.Open(@"..\..\..\File\FileEL\Elementals\" + player.name + "\\" + player.name + i + ".pm", FileMode.Open);
                    BinaryReader br = new BinaryReader(stream);

                    pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
                    Text auxT = new Text(br.ReadInt32().ToString(), new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
                    {
                        Position = new Vector2f(Proportion / 2 + 8 * proportion, pos.Y + 10),
                        CharacterSize = (uint)proportion,
                        FillColor = Color.White
                    };
                    typeTexts.Add(auxT);

                    stream.Close();
                    br.Close();
                }
                else
                {
                    return;
                }
            }
        }
        public void AddTypeButtons()
        {
            int proportion = 24;
            try
            {
                buttons[1].Clear();
            }
            catch (ArgumentOutOfRangeException)
            {
                buttons.Add(new List<Button>());
            }
            Vector2f pos = new Vector2f(Proportion / 2 - 15, window.Size.Y - 12 * Proportion - 15);

            List<Button> typeButtons = new List<Button>();

            for (int i = 0; i <= (int)Type.Types.Electric; i++)
            {
                if (File.Exists(@"..\..\..\File\FileEL\Elementals\" + player.name + "\\" + player.name + i + ".pm"))
                {
                    pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
                    string aux = ((Type.Types)i).ToString();
                    Color color = player.type.type.ToString() == aux ? Color.Red : Color.White;
                    while (aux.Length < 8) 
                    {
                        aux += " ";
                    }

                    Button type = new Button(pos, aux, proportion, color, Color.Black, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf")); 
                    type.Click += TypeClick;
                    typeButtons.Add(type);
                }
                else
                {
                    buttons[1] = typeButtons;
                    return;
                }
            }
        }

        public void AddStatsText()
        {
            int proportion = 24;
            Vector2f pos = new Vector2f(Proportion / 2, window.Size.Y - 7 * Proportion - 15);
            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            Text auxT = new Text("Level: " + player.Level, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            texts.Add(auxT);
            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT = new Text("Attack: " + player.Attack, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            texts.Add(auxT);
            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT = new Text("Defence: " + player.Defence, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            texts.Add(auxT);
            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT = new Text("Speed: " + player.Speed, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            texts.Add(auxT);
            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT = new Text("Life: " + player.Life, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            texts.Add(auxT);
            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT = new Text("Stamina: " + player.Stamina, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            texts.Add(auxT);
        }

        public void AddMoveButtons()
        {
            try
            {
                buttons[2].Clear();
            }
            catch (ArgumentOutOfRangeException)
            {
                buttons.Add(new List<Button>());
            }
            int proportion = 20;
            Vector2f pos = new Vector2f(window.Size.X - 20 * proportion - 15, window.Size.Y - 12 * Proportion - 15);
            Text text = new Text("", new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"), 20);
            text.FillColor = Color.Black;
            List<Button> moveButtons = new List<Button>();

            foreach(var item in player.AllMoves)
            {
                if (File.Exists(@"..\..\..\File\FileEL\Moves\" + item.Key + "\\" + item.Key + ".mv"))
                {
                    pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
                    string aux = item.Key;
                    Color color = player.movesContains(item.Key) ? Color.Red : Color.White;
                    while (aux.Length < 20)
                    {
                        aux += " ";
                    }

                    text.DisplayedString = aux;

                    Button move = new Button(pos, color, text);
                    move.MouseOn += MouseOn;
                    move.MouseLeft += MouseLeft;
                    move.Click += MovesClick;
                    moveButtons.Add(move);
                }
                else
                {
                    buttons[2] = moveButtons;
                    return;
                }
            }
            buttons[2] = moveButtons;
        }


        public void TypeClick(object sender, EventArgs e)
        {
            if (Program.Scene != Program.Scenes.Game && Gamemode.SubScene != Gamemode.SubScenes.EditingPlayer)
                return;
            Button button = sender as Button;
            Enum.TryParse(button.text.DisplayedString.Replace(" ", ""), out Type.Types value);

            if (button.shape.FillColor == Color.Red)
                return;

            if (Gamemode.SubScene == Gamemode.SubScenes.Fight && Fight.Instance().permitChoice)
                Fight.Instance().permitChoice = false;
            else if (!Fight.Instance().permitChoice && Gamemode.SubScene == Gamemode.SubScenes.Fight)
                return;

            for (int i = 0; i < buttons[1].Count; i++)
            {
                if (buttons[1][i].text.DisplayedString.Replace(" ", "") == player.type.type.ToString())
                {
                    buttons[1][i].shape.FillColor = Color.White;
                    break;
                }
            }

            button.shape.FillColor = Color.Red;

            player.ChangeType((int)value);
            player.Print();

            sprite.TextureRect = new IntRect(0, 4 * (int)player.type.type * 32, 16, 32);
            Fight.Instance().ReloadMoveButtons();
            AddMoveButtons();
        }

        public void MovesClick(object sender, EventArgs e)
        {
            if (Gamemode.SubScene != Gamemode.SubScenes.EditingPlayer)
                return;

            string aux = (sender as Button).text.DisplayedString;
            int i = aux.Length - 1;
            while (aux[i] == ' ')
            {
                aux = aux.Remove(i);
                i--;
            }

            if ((sender as Button).shape.FillColor == Color.Red)
            {
                if (player.EditMove(aux))
                {
                    (sender as Button).shape.FillColor = Color.White;
                    Fight.Instance().ReloadMoveButtons();
                }
            }
            else
            {
                if (player.EditMove(new Move(aux)))
                {
                    (sender as Button).shape.FillColor = Color.Red;
                    Fight.Instance().ReloadMoveButtons();
                }
            }
        }

        public void MouseOn(object sender, EventArgs e)
        {
            if (!auxSubWindow)
                return;

            if (Program.Scene != Program.Scenes.Game && Gamemode.SubScene != Gamemode.SubScenes.EditingPlayer)
                return;

            int proportion = 24;

            string aux = (sender as Button).text.DisplayedString;

            for (int i = aux.Length - 1; aux[i] == ' '; i--)
            {
                aux = aux.Remove(i);
            }

            RenderWindow subWindow = new RenderWindow(new VideoMode(15 * (uint)proportion + 15, 15 * (uint)proportion), aux);
            subWindow.SetVerticalSyncEnabled(true);
            subWindow.Closed += (sender, args) => subWindow.Close();
            subWindow.Clear(Color.Black);

            subWindow.DispatchEvents();
            this.subWindow = subWindow;

            Vector2f pos = new Vector2f(proportion, 15);
            Move move = new Move(aux);

            Text auxT = new Text("Type: " + move.type.type, new Font(@"..\\..\\..\\File\FileTTF\PressStart2P-Regular.ttf"))
            {
                Position = new Vector2f(pos.X, pos.Y + 10),
                CharacterSize = (uint)proportion,
                FillColor = Color.White
            };
            subWindow.Draw(auxT);

            pos = new Vector2f(pos.X, pos.Y + 3 * proportion);
            auxT.DisplayedString = "Affinity: " + player.AllMoves[aux];
            auxT.Position = new Vector2f(pos.X, pos.Y + 10);

            subWindow.Draw(auxT);

            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT.DisplayedString = "Damage: " + move.Damage;
            auxT.Position = new Vector2f(pos.X, pos.Y + 10);

            subWindow.Draw(auxT);

            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT.DisplayedString = "Precision: " + move.Precision;
            auxT.Position = new Vector2f(pos.X, pos.Y + 10);

            subWindow.Draw(auxT);

            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT.DisplayedString = "Stamina: " + move.Stamina;
            auxT.Position = new Vector2f(pos.X, pos.Y + 10);

            subWindow.Draw(auxT);

            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT.DisplayedString = "Priority: " + move.Priority;
            auxT.Position = new Vector2f(pos.X, pos.Y + 10);

            subWindow.Draw(auxT);

            pos = new Vector2f(pos.X, pos.Y + 15 + proportion);
            auxT.DisplayedString = "Recoil: " + move.Recoil;
            auxT.Position = new Vector2f(pos.X, pos.Y + 10);

            subWindow.Draw(auxT);
            subWindow.Display();
            auxSubWindow = !auxSubWindow;
        }

        public void MouseLeft(object sender, EventArgs e)
        {
            if (auxSubWindow)
                return;
            subWindow.Dispose();
            GC.Collect();
            auxSubWindow = !auxSubWindow;
        }

        public override void Draw()
        {
            base.Draw();
            //Console.WriteLine("buttons = {1}", buttons[2].Count);
            for (int i = 0; i < typeTexts.Count; i++)
            {
                window.Draw(typeTexts[i]);
            }
            for (int i = 0; i < texts.Count; i++)
            {
                window.Draw(texts[i]);
            }
            window.Draw(sprite);
        }
    }
}
