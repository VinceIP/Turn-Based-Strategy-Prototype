using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Busted;
using System;

public class AudioController : MonoBehaviour
{
    private EditorController _editorController;
    private AudioClip _audioClip = null;
    private AudioSource _audioSource;
    private AudioCache _audioCache;

    void Awake()
    {
        _audioCache = GRID.AudioCache;
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        EditorController.onPainted += PlayImpactAudio;

    }

    void PlayImpactAudio()
    {
        _audioClip = null;
        int randInt;
        System.Random rand = new System.Random(Time.frameCount);
        if (_editorController.paintType == EditorController.PaintType.Terrain) //If terrain was just painted
        {
            if(_editorController.selectedTileType == Tile.TileType.WATER)
            {
                randInt =
                rand.Next(0, 3); //Random int between 0 and 2
                _audioClip = _audioCache.TerrainImpact[randInt];

            }
        }
        else if (_editorController.paintType == EditorController.PaintType.Building)
        {
            randInt =
            rand.Next(0, 2); //Random int between 0 and 2
            _audioClip = _audioCache.BuildingImpact[randInt];
        }


        if (_audioClip != null)
        {
            if (_audioSource.isPlaying == true)
            {
                if (_audioSource.time >= (0.3f * _audioSource.clip.length)) //If 50% or more of the clip has played
                {
                    _audioSource.clip = _audioClip;
                    if (_audioClip != null) _audioSource.Play();
                }
            }
            else
            {
                _audioSource.clip = _audioClip;
                if (_audioClip != null) _audioSource.Play();
            }
        }


    }

    public EditorController EditorController
    {
        get{ return _editorController; }
        set{ _editorController = value; }
    }

}

