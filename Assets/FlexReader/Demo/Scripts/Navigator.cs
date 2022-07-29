using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlexFramework.Demo
{
    /// <summary>
    /// Scene navigator
    /// </summary>
    public class Navigator : MonoBehaviour
    {
        /// <summary>
        /// Unload current scene and load target scene by index
        /// </summary>
        /// <param name="index">Scene index</param>
        public void GoToScene(int index)
        {
            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }
    }
}
