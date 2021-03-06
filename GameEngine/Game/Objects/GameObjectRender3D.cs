﻿using System;
using GameEngine.Game.Objects.Rendering;
using GameEngine.Game.Tween;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace GameEngine.Game.Objects
{
    [Serializable]
    public abstract class GameObjectRender3D : GameObjectRender
    {
        public GameObjectRender3D(GamePlus game, Vector3 position, Quaternion rotation) : base(game)
        {
            Transform.Position = position;
            Transform.Rotation = rotation;
        }

        public Transform3D Transform { get; set; } = new Transform3D();

        [JsonIgnore] public new TweenerGameObject3D Tweener => (TweenerGameObject3D) base.Tweener;

        public override void PreDraw(GraphicsDevice g)
        {
            Game.SceneManager.Cameras.LoopThroughAll(cam => { PreDraw(cam, g, Transform); });
        }

        public override void Draw(GraphicsDevice g)
        {
            Game.SceneManager.Cameras.LoopThroughAll(cam => { Draw(cam, g, Transform); });
        }

        public override void PostDraw(GraphicsDevice g)
        {
            Game.SceneManager.Cameras.LoopThroughAll(cam => { PostDraw(cam, g, Transform); });
        }

        protected override Tweener NewTweener(GamePlus game)
        {
            return new TweenerGameObject3D(game, this);
        }


        public abstract void Draw(Camera3D cam, GraphicsDevice g, Transform3D transform);

        public virtual void PreDraw(Camera3D cam, GraphicsDevice g, Transform3D transform)
        {
        }

        public virtual void PostDraw(Camera3D cam, GraphicsDevice g, Transform3D transform)
        {
        }
    }
}