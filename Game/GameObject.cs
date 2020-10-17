using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DREngine.Game
{
    public abstract class GameObject : GameComponent, IGameObject
    {
        protected GamePlus _game;
        private bool _gottaStart = true;
        public GameObject(GamePlus game) : base(game)
        {
            _game = game;
            game.Components.Add(this);
            _gottaStart = true;
            Awake();
        }

        ~GameObject()
        {
            // TODO: Destroy immediate?
        }


        #region GameComponent Hookups
        public override void Update(GameTime gameTime)
        {
            // Start before first tick
            if (_gottaStart)
            {
                Start();
                _gottaStart = false;
            }
            Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        #endregion

        #region Interface
        /// <summary>
        ///
        /// </summary>
        public virtual void Awake()
        {

        }

        public  virtual void Start()
        {

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"> delta time in seconds </param>
        public  virtual void Update(float dt)
        {

        }

        public virtual void OnDestroy()
        {

        }
        #endregion
    }
}
