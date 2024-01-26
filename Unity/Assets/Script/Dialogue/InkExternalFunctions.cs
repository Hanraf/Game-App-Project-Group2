using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;

public class InkExternalFunctions
{
    private bool isPaused = false;
    DialogueManager dialogueManager;

    public void Bind(Story story, Animator emoteAnimator)
    {
        story.BindExternalFunction("playEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
        story.BindExternalFunction("Pause", () => Pause());
        story.BindExternalFunction("Continue", () => Continue());
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("playEmote");
        story.UnbindExternalFunction("Pause");
        story.UnbindExternalFunction("Continue");
    }

    public void PlayEmote(string emoteName, Animator emoteAnimator)
    {
        if (emoteAnimator != null)
        {
            emoteAnimator.Play(emoteName);
            Debug.Log(emoteName);
        }
        else
        {
            Debug.LogWarning("Tried to play emote, but emote animator was "
                + "not initialized when entering dialogue mode.");
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}

