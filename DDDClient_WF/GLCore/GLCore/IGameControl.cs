using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    /// <summary>
    /// Interface for GameFramework.  Pick a class in your Game for which
    /// you wish to implement this interface, pass its instance to
    /// GameFramework.Run.
    /// GameFramework will call this interface during its game loop.
    /// </summary>
    public interface IGameControl
    {
        /// <summary>
        /// Initialize the Game canvas.  The Canvas contains the DirectX
        /// device that you are rendering to.
        /// </summary>
        /// <param name="options">A canvas Options class.</param>
        void InitializeCanvasOptions(CanvasOptions options);
        /// <summary>
        /// An array of Scenes that the game framework will run through and
        /// render.  Typically a Scene will be a game level.
        /// </summary>
        /// <param name="g">Game Framework instance</param>
        /// <returns>Scene array</returns>
        Scene[] InitializeScenes(GameFramework g);
        /// <summary>
        /// Define what to do in when the game ends, here.
        /// </summary>
        /// <param name="g">Game Framework instance.</param>
        void GameOver(GameFramework g);
        void SceneChanged(int scene_number);
        /// <summary>
        /// A GDI+ control that the GameFramework will render its canvas
        /// on.
        /// </summary>
        /// <returns></returns>
        Control GetTargetControl();
    }
}
