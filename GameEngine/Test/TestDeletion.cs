﻿using GameEngine.Game;
using GameEngine.Game.Input;
using GameEngine.Game.Objects.Rendering;
using GameEngine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
// ReSharper disable NotAccessedField.Local

namespace GameEngine.Test
{
    public class TestDeletion : IGameTester
    {

        private Camera3D _cam;
        private ExampleTriangleObject _exampleObj;
        private ExampleTriangleObject _exampleObj2;

        private GamePlus _game;

        public void Initialize(GamePlus game)
        {
            _game = game;
            Debug.LogDebug("TestGame Initialize()");

            _cam = new Camera3D(game);
            _cam.Rotation = Math.FromEuler(0, 0, 0);
            _exampleObj = new ExampleTriangleObject(game, new Vector3(0, 0, -90), Quaternion.Identity);
            _exampleObj2 = new ExampleTriangleObject(game, new Vector3(10, 10, -100), Quaternion.Identity);
            //_testObj.AddChild();
        }

        public void Update(float deltaTime)
        {
            // Test basic camera rotation & FOV stuff

            if (RawInput.KeyPressing(Keys.R))
            {
                Vector3 e = Math.ToEuler(_cam.Rotation);
                e.Y += 90f * deltaTime;
                _cam.Rotation = Math.FromEuler(e);
            }
            if (RawInput.KeyPressing(Keys.W))
            {
                _cam.Fov += 10f * deltaTime;
            } else if (RawInput.KeyPressing(Keys.S))
            {
                _cam.Fov -= 10f * deltaTime;
            }

            // Test deletion

            if (RawInput.KeyPressed(Keys.G))
            {
                Debug.Log("POOF 1");
                if (_exampleObj != null) _exampleObj.Destroy();
            }
            if (RawInput.KeyPressed(Keys.H))
            {
                Debug.Log("POOF 2");
                if (_exampleObj2 != null) _exampleObj2.Destroy();
            }

            if (RawInput.KeyPressed(Keys.J))
            {
                Debug.Log("KABOOM (does nothing rn)");
                //_game.SceneManager.UnloadSceneAtEndOfFrame();
            }
        }

        public void Draw()
        {

        }
    }

}
