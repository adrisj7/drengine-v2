﻿using System;
using GameEngine.Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Game.Objects.Rendering
{
    public class SpriteRenderer : SimpleMeshAlphaTestRenderer<VertexPositionColorTexture>
    {
        private Color _blend = Color.White;
        private Sprite _sprite;

        public SpriteRenderer(GamePlus game, Sprite sprite, Vector3 position = default, Quaternion rotation = default) :
            base(game, position, rotation)
        {
            _sprite = sprite;
            PrimitiveType = PrimitiveType.TriangleList;
        }

        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                UpdateVertices();
            }
        }

        public Color Blend
        {
            get => _blend;
            set
            {
                _blend = value;
                // TODO: Just update the colors, not the whole thing.
                UpdateVertices();
            }
        }

        public override void Start()
        {
            base.Start();

            UpdateVertices();
        }

        private void UpdateVertices()
        {
            Texture = _sprite.Texture ??
                      throw new InvalidOperationException("Sprite Renderer's Sprite was not initialized yet!");

            var up = Vector3.Up * _sprite.Height * _sprite.Scale;
            var right = Vector3.Right * _sprite.Width * _sprite.Scale;
            var pivot = _sprite.Pivot.X * right + _sprite.Pivot.Y * up;

            Vector3 topLeft = up - pivot,
                topRight = topLeft + right,
                bottomLeft = -pivot,
                bottomRight = bottomLeft + right;

            // We rotate the order so the default rotation faces US, ( so technically it's backwards )
            Vertices = new[]
            {
                new VertexPositionColorTexture(topLeft, _blend, new Vector2(0, 0)),
                new VertexPositionColorTexture(bottomRight, _blend, new Vector2(1, 1)),
                new VertexPositionColorTexture(bottomLeft, _blend, new Vector2(0, 1)),

                new VertexPositionColorTexture(bottomRight, _blend, new Vector2(1, 1)),
                new VertexPositionColorTexture(topLeft, _blend, new Vector2(0, 0)),
                new VertexPositionColorTexture(topRight, _blend, new Vector2(1, 0))
            };
        }

        public override void Draw(Camera3D cam, GraphicsDevice g, Transform3D transform)
        {
            g.BlendState = BlendState.AlphaBlend;
            Texture = _sprite.Texture;
            base.Draw(cam, g, transform);
        }
    }
}