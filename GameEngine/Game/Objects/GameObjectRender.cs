﻿using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Game.Objects
{
    public abstract class GameObjectRender : GameObject
    {
        private ObjectContainerNode<GameObjectRender> _renderAddedNode;

        protected GameObjectRender(GamePlus game) : base(game)
        {
            _renderAddedNode = game.SceneManager.GameRenderObjects.Add(this);
        }

        #region Internal Control

        internal void DoDraw(GraphicsDevice g)
        {
            EnsureStarted();
            Draw(g);
        }

        internal void DoPreDraw(GraphicsDevice g)
        {
            EnsureStarted();
            PreDraw(g);
        }

        internal void DoPostDraw(GraphicsDevice g)
        {
            EnsureStarted();
            PostDraw(g);
        }

        #endregion

        #region Util

        /// <summary>
        ///     When we are deleted, also delete ourselves off of the
        ///     render thing.
        /// </summary>
        internal override void RunOnDestroy()
        {
            _renderAddedNode = Game.SceneManager.GameRenderObjects.RemoveImmediate(_renderAddedNode);
            base.RunOnDestroy();
        }

        internal override void RunOnDisable(ObjectContainerNode<GameObject> newNode)
        {
            _renderAddedNode = Game.SceneManager.GameRenderObjects.DisableImmediate(_renderAddedNode);
            base.RunOnDisable(newNode);
        }

        internal override void RunOnEnable(ObjectContainerNode<GameObject> newNode)
        {
            _renderAddedNode = Game.SceneManager.GameRenderObjects.EnableImmediate(_renderAddedNode);
            base.RunOnEnable(newNode);
        }

        #endregion

        #region Object Functions

        /// <summary>
        ///     Draws. Abstract cause we don't want to
        ///     mistakenly make tons of game-object-renderers without
        ///     ensuring that its rendering is actually taken
        ///     advantage of.
        /// </summary>
        /// <param name="g"></param>
        public abstract void Draw(GraphicsDevice g);

        public virtual void PreDraw(GraphicsDevice g)
        {
        }

        public virtual void PostDraw(GraphicsDevice g)
        {
        }

        #endregion
    }
}