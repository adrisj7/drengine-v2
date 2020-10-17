﻿namespace DREngine.Game
{
    public interface IGameStarter
    {
        public void Initialize(GamePlus game);
        public void Update(float deltaTime);
        public void Draw();
    }
}
