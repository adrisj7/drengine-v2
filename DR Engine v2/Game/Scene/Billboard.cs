﻿using DREngine.Editor.SubWindows.FieldWidgets;
using GameEngine.Game.Objects;
using GameEngine.Game.Objects.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using static DREngine.Game.IDependentOnResourceData;

namespace DREngine.Game.Scene
{
    //[Serializable]
    //[JsonConverter(typeof(SceneObjectJsonConverter))]
    public class Billboard : GameObjectRender3D, ISceneObject, IDependentOnResourceData
    {
        public Billboard(DRGame game, Vector3 position, Quaternion rotation) : base(game, position, rotation)
        {
            Game = game;
        }

        // Required empty constructor for deserialization.
        public Billboard() : this(CurrentGame, Vector3.Zero, Quaternion.Identity)
        {
        }

        [JsonIgnore] public new DRGame Game { get; set; }

        [FieldIgnore]
        public string Type { get; } = "Billboard";
        public string Name { get; set; }
        [FieldIgnore]
        public Vector3 FocusCenter => Transform.Position;


        public override void Draw(Camera3D cam, GraphicsDevice g, Transform3D transform)
        {
            //Debug.Log("TODO: Render Billboard");
        }
    }
}