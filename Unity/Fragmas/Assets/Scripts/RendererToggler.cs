using UnityEngine;

public class RendererToggler : MonoBehaviour
{
    [SerializeField]
    float turnOnDelay = .1f;
    [SerializeField]
    float turnOffDelay = 3.5f;
    [SerializeField]
    bool enabledOnLoad = false;

    Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);

        if (enabledOnLoad)
            EnableRenderers();
        else
            DisableRenderers();
    }

    //Method used by our Unity events to show and hide the player
    public void ToggleRenderersDelayed(bool isOn)
    {
        Debug.Log("entered RendererToggler.ToggleRenderersDelayed" + isOn.ToString());
        if (isOn)
        {
            Invoke("EnableRenderers", turnOnDelay);
        }
        else
        {
            Invoke("DisableRenderers", turnOffDelay);
        }
        Debug.Log("leaving RendererToggler.ToggleRenderersDelayed");

    }

    public void EnableRenderers()
    {
        Debug.Log("entered RendererToggler.EnableRenderers");
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = true;
        }
        Debug.Log("leaving RendererToggler.EnableRenderers");
    }

    public void DisableRenderers()
    {
        Debug.Log("entered RendererToggler.DisableRenderers");
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
        Debug.Log("leaving RendererToggler.DisableRenderers");
    }

    //Will be used to change the color of the players for different options
    public void ChangeColor(Color newColor)
    {
        Debug.Log("entered RendererToggler.ChangeColor");
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = newColor;
        }
        Debug.Log("leaving RendererToggler.ChangeColor");
    }
}