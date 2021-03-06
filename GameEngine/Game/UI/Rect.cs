﻿using Microsoft.Xna.Framework;

namespace GameEngine.Game.UI
{
    public class Rect
    {
        public float Height;
        public float Width;
        public float X;
        public float Y;

        public Rect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect(Vector2 pos, Vector2 size) : this(pos.X, pos.Y, size.X, size.Y)
        {
        }

        public Rect(Rect toCopy)
        {
            X = toCopy.X;
            Y = toCopy.Y;
            Width = toCopy.Width;
            Height = toCopy.Height;
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public Vector2 Position
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public Vector2 Min
        {
            get => Position;
            set => Position = value;
        }

        public Vector2 Max
        {
            get => Position + Size;
            set => Size = Max - Min;
        }

        public bool Contains(Vector2 pos)
        {
            return
                Min.X < pos.X && pos.X < Max.X &&
                Min.Y < pos.Y && pos.Y < Max.Y;
        }

        public override string ToString()
        {
            return $"[Pos: ({X}, {Y}) Size: ({Width}, {Height})]";
        }

        public static implicit operator Rectangle(Rect rect)
        {
            return new Rectangle((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
        }
    }
}