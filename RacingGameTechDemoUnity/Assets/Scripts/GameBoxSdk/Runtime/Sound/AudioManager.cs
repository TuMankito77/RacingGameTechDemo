namespace GameBoxSdk.Runtime.Sound
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Pool;

    using GameBoxSdk.Runtime.Core;

    public class AudioManager : BaseSystem
    {
        private const string AUDIO_PLAYER_PREFAB_PATH = "Sound/AudioPlayer";

        private Dictionary<int, AudioPlayer> loopingAudioPlayers = new Dictionary<int, AudioPlayer>();
        private AudioPlayer audioPlayerPrefab = null;
        private AudioPlayer gameplayAudioPlayer = null;
        private AudioPlayer generalAudioPlayer = null;
        private AudioPlayer backgroundMusicAudioPlayer = null;
        private GameObject audioManagerGO = null;
        private IObjectPool<AudioPlayer> audioPlayersPool = null; 
        private List<AudioPlayer> loopingClipsPlaying = null;
        private ClipsDatabase clipsDatabase = null;

        public float CurrentMusicVolume { get; private set; } = 1;
        public float CurrentVfxVolume { get; private set; } = 1;

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);

            audioPlayersPool = new ObjectPool<AudioPlayer>(OnCreateAudioPlayerForPool);
            loopingClipsPlaying = new List<AudioPlayer>();
            audioManagerGO = new GameObject(GetType().Name);
            audioManagerGO.AddComponent<AudioListener>();
            GameObject.DontDestroyOnLoad(audioManagerGO);
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            audioPlayerPrefab = await contentLoader.LoadAsset<AudioPlayer>(AUDIO_PLAYER_PREFAB_PATH);
            
            if (audioPlayerPrefab == null)
            {
                return false;
            }

            gameplayAudioPlayer = audioPlayersPool.Get();
            gameplayAudioPlayer.name = "GameplayAudioPlayer";
            gameplayAudioPlayer.SetIsLooping(false);
            gameplayAudioPlayer.UpdateVolume(CurrentVfxVolume);

            generalAudioPlayer = audioPlayersPool.Get();
            generalAudioPlayer.name = "GeneralAudioPlayer";
            generalAudioPlayer.SetIsLooping(false);
            generalAudioPlayer.UpdateVolume(CurrentVfxVolume);

            backgroundMusicAudioPlayer = audioPlayersPool.Get();
            backgroundMusicAudioPlayer.name = "BackgroundMusicAudioPlayer";
            backgroundMusicAudioPlayer.SetIsLooping(true);
            backgroundMusicAudioPlayer.UpdateVolume(CurrentMusicVolume);

            clipsDatabase = await contentLoader.LoadAsset<ClipsDatabase>(ClipsDatabase.CLIPS_DATABASE_SCRIPTABLE_OBJECT_PATH);
            
            if(clipsDatabase == null)
            {
                return false;
            }

            clipsDatabase.Initialize();
            
            return true;
        }

        public void PlayGameplayClip(ClipIds clipId)
        {
            AudioClip audioClip = clipsDatabase.GetFile(clipId.ToString());
            gameplayAudioPlayer.PlayClipOneShot(audioClip);
        }

        public void PauseGameplayClips()
        {
            gameplayAudioPlayer.Pause();
        }

        public void UnPauseGameplayClips()
        {
            gameplayAudioPlayer.UnPause();
        }

        public void PlayGeneralClip(ClipIds clipId)
        {
            AudioClip audioClip = clipsDatabase.GetFile(clipId.ToString());
            generalAudioPlayer.PlayClipOneShot(audioClip);
        }

        public void PuaseGeneralClips()
        {
            generalAudioPlayer.Pause();
        }

        public void UnPauseGeneralClips()
        {
            generalAudioPlayer.UnPause();
        }

        public void PlayBackgroundMusic(ClipIds clipId)
        {
            AudioClip audioClip = clipsDatabase.GetFile(clipId.ToString());
            backgroundMusicAudioPlayer.UpdateDefaultClip(audioClip);
            backgroundMusicAudioPlayer.Play();
            loopingClipsPlaying.Add(backgroundMusicAudioPlayer);
        }

        public void PauseBackgroundMusic()
        {
            backgroundMusicAudioPlayer.UnPause();
        }

        public void UnPaseBackgroundMusic()
        {
            backgroundMusicAudioPlayer.Pause();
        }

        public void PlayLoopingClip(int idRetreiver, ClipIds clipId, Transform parent = null, bool isSpatial = false, bool isGameplaySound = true)
        {
            AudioClip audioClip = clipsDatabase.GetFile(clipId.ToString());
            AudioPlayer audioPlayer = null;

            //Checking if the object retrived from the dictionary was destroyed in the scene 
            if (!loopingAudioPlayers.TryGetValue(idRetreiver, out audioPlayer) || audioPlayer == null)
            {
                audioPlayer = audioPlayersPool.Get();
                loopingAudioPlayers.Add(idRetreiver, audioPlayer);
            }

            if(isGameplaySound)
            {
                loopingClipsPlaying.Add(audioPlayer);
            }

            audioPlayer.transform.parent = parent;
            audioPlayer.SetIsSpatial(isSpatial);
            audioPlayer.UpdateDefaultClip(audioClip);
            audioPlayer.SetIsLooping(true);
            audioPlayer.UpdateVolume(CurrentVfxVolume);
            audioPlayer.Play();
        }

        public void PauseLoopingClip(int idRetreiver)
        {
            if(loopingAudioPlayers.TryGetValue(idRetreiver, out AudioPlayer audioPlayer))
            {
                audioPlayer.Pause();
                loopingClipsPlaying.Remove(audioPlayer);
            }
        }

        public void UnPauseLoopingClip(int idRetreiver)
        {
            if(loopingAudioPlayers.TryGetValue(idRetreiver, out AudioPlayer audioPlayer))
            {
                audioPlayer.UnPause();
                loopingClipsPlaying.Add(audioPlayer);
            }
        }

        public void StopLoopingClip(int idRetreiver)
        {
            if(loopingAudioPlayers.TryGetValue(idRetreiver, out AudioPlayer audioPlayer))
            {
                audioPlayer.Pause();
                loopingAudioPlayers.Remove(idRetreiver);
                loopingClipsPlaying.Remove(audioPlayer);
                audioPlayer.transform.parent = audioManagerGO.transform;
                audioPlayersPool.Release(audioPlayer);
            }
        }

        public void StopAllLoopingClips()
        {
            foreach(KeyValuePair<int, AudioPlayer> idAudioPlayerPair in loopingAudioPlayers)
            {
                idAudioPlayerPair.Value.Stop();
                loopingClipsPlaying.Remove(idAudioPlayerPair.Value);
                idAudioPlayerPair.Value.transform.parent = audioManagerGO.transform;
                audioPlayersPool.Release(idAudioPlayerPair.Value);
            }

            loopingAudioPlayers.Clear();
            backgroundMusicAudioPlayer.Stop();
        }

        public void PauseAllLoopingClips()
        {
            foreach(AudioPlayer audioPlayer in loopingClipsPlaying)
            {
                audioPlayer.Pause();
            }
        }

        public void UnPauseAllLoopingClips()
        {
            foreach(AudioPlayer audioPlayer in loopingClipsPlaying)
            {
                audioPlayer.UnPause();
            }
        }

        public void UpdateMusicVolume(float volume)
        {
            CurrentMusicVolume = volume;
            backgroundMusicAudioPlayer.UpdateVolume(volume);
        }

        public void UpdateVFXMusicVolume(float volume)
        {
            CurrentVfxVolume = volume;
            gameplayAudioPlayer.UpdateVolume(volume);
            generalAudioPlayer.UpdateVolume(volume);

            foreach(AudioPlayer audioPlayer in loopingAudioPlayers.Values)
            {
                audioPlayer.UpdateVolume(volume);
            }    
        }

        private AudioPlayer OnCreateAudioPlayerForPool()
        {
            return GameObject.Instantiate(audioPlayerPrefab, audioManagerGO.transform);
        }
    }
}
