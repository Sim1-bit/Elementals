using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GameOfYear.Logger;
using GameOfYear.Create;


namespace GameOfYear
{
    class Button
    {
        protected int charSize;
        public int CharSize
        {
            get
            {
                return charSize;
            }
            set
            {
                if (value <= 0)
                {
                    charSize = 1;
                }
                else
                {
                    charSize = value;
                }
            }
        }

        public event EventHandler Click;
        public event EventHandler Releas;
        public event EventHandler MouseOn;
        public event EventHandler MouseLeft;

        public RectangleShape shape;
        public readonly Text text;
        private bool isPressed;
        private bool isMouseOver;

        public Button(Vector2f position, string buttonText, int charSize, Color colorButton, Color colorText, Font font)
        {
            isPressed = false;
            isMouseOver = false;
            this.CharSize = charSize;
            shape = new RectangleShape(new Vector2f(buttonText.Length * CharSize + 10, CharSize + 10))
            {
                Position = position,
                FillColor = colorButton
            };

            text = new Text(buttonText, font)
            {
                Position = new Vector2f(position.X + 10, position.Y + 10),
                FillColor = colorText,
                CharacterSize = (uint)CharSize
            };
        }

        public Button(Vector2f position, Color colorButton, Text text)
        {
            isPressed = false;
            isMouseOver = false;
            this.CharSize = (int)text.CharacterSize;
            shape = new RectangleShape(new Vector2f(text.DisplayedString.Length * CharSize + 10, CharSize + 10))
            {
                Position = position,
                FillColor = colorButton
            };
            this.text = new Text(text);
            this.text.Position = new Vector2f(position.X + 10, position.Y + 10);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(shape);
            window.Draw(text);
        }

        public void Update(RenderWindow window)
        {
            Vector2i mousePosition = Mouse.GetPosition(window);
            bool previouslyOver = isMouseOver;
            isMouseOver = shape.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y);

            if(isMouseOver && ! previouslyOver)
            {
                OnMouseEntered();
            }
            else if(!isMouseOver && previouslyOver)
            {
                OnMouseLeft();
            }

            if (isMouseOver && Mouse.IsButtonPressed(Mouse.Button.Left) && !isPressed)
            {
                OnClicked();
                isPressed = true;
            }
            else if (isPressed && window.HasFocus() && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                OnReleased();
                isPressed = false;
            }
        }

        protected virtual void OnClicked()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnReleased()
        {
            Releas?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseEntered()
        {
            MouseOn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseLeft()
        {
            MouseLeft?.Invoke(this, EventArgs.Empty);
        }
    }
}
