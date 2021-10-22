using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSound : MonoBehaviour
{
    public AudioSource slow;
    public AudioSource danger;
    public AudioSource breath;
    Animator _anim;
    int _level;

    private void Awake()
    {

        SceneManager.sceneLoaded += this.OnLoadCallback;

        if (Global.backgroundsound == null)
        {
            Global.backgroundsound = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        _anim = GetComponent<Animator>();
        _anim.SetBool("Danger", false);
    }

    private void Start()
    {
        if (!slow.isPlaying) slow.Play();
        if (!danger.isPlaying) danger.Play();
    }

    private void Update()
    {
        bool _danger = _anim.GetBool("Danger");
        if (Global.danger_objects.Count > 0)
        {
            if (!_danger)
            {
                danger.Play();
                _anim.SetBool("Danger", true);
            }
        }
        else
        {
            if (_danger)
            {
                slow.Play();
                _anim.SetBool("Danger", false);
            }
        }

        if (Global.player == null)
        {
            breath.volume = 0f;
            breath.pitch = 0.9f;
        }
        else
        {
            SetBreath();
        }
    }

    void SetBreath()
    {
        if (Global.player_script.health == Settings.player_max_health)
        {
            breath.volume = 0f;
        }
        else
        {
            breath.volume = 1f;
        }
        //breath.volume = 1f - (float)Global.player_script.health / (float)Settings.player_max_health;
        breath.pitch = 1.2f - 0.3f * ((float)Global.player_script.health / (float)Settings.player_max_health);
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {

        if (gameObject != Global.backgroundsound) return;

        int level = scene.buildIndex;

        _level = level;

        if (level == 0 || level == 1)
        {
            if (!slow.isPlaying) slow.Play();
            if (!danger.isPlaying) danger.Play();
            _anim.SetBool("Danger", false);
        }
        if (level == 2 || level == 3)
        {
            if (slow.isPlaying) slow.Stop();
            if (danger.isPlaying) danger.Stop();
            _anim.SetBool("Danger", false);
        }
    }
}
