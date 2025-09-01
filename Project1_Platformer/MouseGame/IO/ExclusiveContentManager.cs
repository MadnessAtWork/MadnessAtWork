using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// This class is to build a content manager that can be used to load and unload specific parts of levels
    /// </summary>
    class ExclusiveContentManager : ContentManager
    {
        //stores the loaded assets in a dictionary
        private Dictionary<string, Texture2D> assets;
        internal ExclusiveContentManager(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        {
            assets = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Only loads texture2d for the game
        /// </summary>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        internal Texture2D LoadContentExclusive(string AssetName)
        {
            if (assets.ContainsKey(AssetName))
            {
                Texture2D returnObj;
                assets.TryGetValue(AssetName, out returnObj);
                return returnObj;
            }
            else
            {
                Texture2D texture = ReadAsset<Texture2D>(AssetName, null);
                assets.Add(AssetName, texture);
                return texture;
            }
        }

        /// <summary>
        /// dispose the texture and remove it from the dictionary
        /// </summary>
        /// <param name="disposable"></param>
        internal void Unload(IDisposable disposable, String AssetName)
        {
            //try and remove the texture from assets list
            if (assets.ContainsKey(AssetName))
            {
                Texture2D tmp;
                assets.TryGetValue(AssetName, out tmp);
                tmp.Dispose();
                assets.Remove(AssetName);
            }
            disposable.Dispose();
        }
    }
}
