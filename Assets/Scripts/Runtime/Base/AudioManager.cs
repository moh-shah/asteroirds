using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Moshah.Asteroids.Base
{
    public enum SfxType
    {
        Shoot,
        Hit,
        GameOver
    }

    public interface IAudioManager
    {
        void PlaySfx(SfxType sfxType);
    }
    public class AudioManager : MonoBehaviour,IAudioManager
    {
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private AssetReference shootSfx;
        [SerializeField] private AssetReference gameOverSfx;
        [SerializeField] private AssetReference hitSfx;

        private Dictionary<SfxType,AudioClip> _loadedAudioClips = new Dictionary<SfxType, AudioClip>();

        private void Start()
        {
            PreLoadAssets();
        }

        private void PreLoadAssets()
        {
            shootSfx.LoadAssetAsync<AudioClip>().Completed += handle => { _loadedAudioClips.Add(SfxType.Shoot, handle.Result); };
            gameOverSfx.LoadAssetAsync<AudioClip>().Completed += handle => { _loadedAudioClips.Add(SfxType.GameOver, handle.Result); };
            hitSfx.LoadAssetAsync<AudioClip>().Completed += handle => { _loadedAudioClips.Add(SfxType.Hit, handle.Result); };
        }

        public void PlaySfx(SfxType sfxType)
        {
            if (_loadedAudioClips.ContainsKey(sfxType))
                audioSource.PlayOneShot(_loadedAudioClips[sfxType]);
        }
    }
}