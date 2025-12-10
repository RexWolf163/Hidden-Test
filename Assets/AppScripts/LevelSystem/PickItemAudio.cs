using AppScripts.LevelSystem;
using UnityEngine;
using Vortex.Unity.AudioSystem.Handlers;

public class PickItemAudio : MonoBehaviour
{
    [SerializeField] private AudioHandler audio;

    private void OnEnable()
    {
        LevelController.OnItemPicked += Play;
    }

    private void OnDisable()
    {
        LevelController.OnItemPicked -= Play;
    }

    private void Play() => audio.Play();
}