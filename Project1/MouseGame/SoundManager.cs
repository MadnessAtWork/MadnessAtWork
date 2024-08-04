using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// Manages for playing sounds for the game
    /// </summary>
    class SoundManager
    {
        //Utilize singleton pattern to make sure there is only one sound manager that can be used globally
        private static SoundManager _instance;

        /// <summary>
        /// Access-point for the soundmanager class
        /// </summary>
        internal static SoundManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SoundManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Stores the references to the audio with a string key
        /// </summary>
        private Dictionary<string, SoundEffect> soundEffects;

        /// <summary>
        /// Stores the song for the game
        /// </summary>
        private Song song;

        internal SoundManager()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            MediaPlayer.IsRepeating = true;
            SQLDatabase.instance.audioEnabledEvent += audioChange;
        }

        /// <summary>
        /// Method call to make sure instance starts
        /// </summary>
        internal void Start() {
            
        }

        internal void LoadContent(ContentManager contentManager)
        {
            soundEffects.Add("hit", contentManager.Load<SoundEffect>("hitSound"));
            song = contentManager.Load<Song>("Song");
            //load audio here
            audioChange(SQLDatabase.instance.audioEnabled);
        }

        /// <summary>
        /// Listens for if the audio enabled changes
        /// </summary>
        /// <param name="enabled"></param>
        private void audioChange(bool enabled)
        {
            Log.log("Audio changed");
            if (enabled)
            {
                MediaPlayer.Play(song);
                SoundEffect.MasterVolume = 1.0f;
            }
            else
            {
                MediaPlayer.Stop();
                SoundEffect.MasterVolume = 0.0f;
            }
        }

        /// <summary>
        /// Plays the audio associated with the key value
        /// </summary>
        /// <param name="name"></param>
        internal void playAudio(string name)
        {
            if (soundEffects.ContainsKey(name))
            {
                soundEffects[name].Play();
            }

        }
    }
}
