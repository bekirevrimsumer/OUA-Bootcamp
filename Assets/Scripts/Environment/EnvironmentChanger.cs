using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChanger : MonoBehaviour, IEventListener<LightReflectionEvent>
{
    public Material originalSkyboxMaterial;
    public Material newSkyboxMaterial;
    public Material skyboxBlendMaterial;
    public float transitionDuration = 5.0f;
    private bool isTransitioning = false;

    void Start()
    {
        RenderSettings.skybox = originalSkyboxMaterial;
    }

    private IEnumerator TransitionSkybox(Material fromSkybox, Material toSkybox)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float blendFactor = Mathf.Clamp01(elapsedTime / transitionDuration);

            skyboxBlendMaterial.SetColor("_SunDiscColor", Color.Lerp(fromSkybox.GetColor("_SunDiscColor"), toSkybox.GetColor("_SunDiscColor"), blendFactor));
            skyboxBlendMaterial.SetFloat("_SunDiscMultiplier", Mathf.Lerp(fromSkybox.GetFloat("_SunDiscMultiplier"), toSkybox.GetFloat("_SunDiscMultiplier"), blendFactor));
            skyboxBlendMaterial.SetFloat("_SunDiscExponent", Mathf.Lerp(fromSkybox.GetFloat("_SunDiscExponent"), toSkybox.GetFloat("_SunDiscExponent"), blendFactor));

            skyboxBlendMaterial.SetColor("_SunHaloColor", Color.Lerp(fromSkybox.GetColor("_SunHaloColor"), toSkybox.GetColor("_SunHaloColor"), blendFactor));
            skyboxBlendMaterial.SetFloat("_SunHaloExponent", Mathf.Lerp(fromSkybox.GetFloat("_SunHaloExponent"), toSkybox.GetFloat("_SunHaloExponent"), blendFactor));
            skyboxBlendMaterial.SetFloat("_SunHaloContribution", Mathf.Lerp(fromSkybox.GetFloat("_SunHaloContribution"), toSkybox.GetFloat("_SunHaloContribution"), blendFactor));

            skyboxBlendMaterial.SetColor("_HorizonLineColor", Color.Lerp(fromSkybox.GetColor("_HorizonLineColor"), toSkybox.GetColor("_HorizonLineColor"), blendFactor));
            skyboxBlendMaterial.SetFloat("_HorizonLineExponent", Mathf.Lerp(fromSkybox.GetFloat("_HorizonLineExponent"), toSkybox.GetFloat("_HorizonLineExponent"), blendFactor));
            skyboxBlendMaterial.SetFloat("_HorizonLineContribution", Mathf.Lerp(fromSkybox.GetFloat("_HorizonLineContribution"), toSkybox.GetFloat("_HorizonLineContribution"), blendFactor));

            skyboxBlendMaterial.SetColor("_SkyGradientTop", Color.Lerp(fromSkybox.GetColor("_SkyGradientTop"), toSkybox.GetColor("_SkyGradientTop"), blendFactor));
            skyboxBlendMaterial.SetColor("_SkyGradientBottom", Color.Lerp(fromSkybox.GetColor("_SkyGradientBottom"), toSkybox.GetColor("_SkyGradientBottom"), blendFactor));
            skyboxBlendMaterial.SetFloat("_SkyGradientExponent", Mathf.Lerp(fromSkybox.GetFloat("_SkyGradientExponent"), toSkybox.GetFloat("_SkyGradientExponent"), blendFactor));

            RenderSettings.skybox = skyboxBlendMaterial;

            yield return null;
        }

        RenderSettings.skybox = toSkybox;
        isTransitioning = false;
    }

    public void ChangeEnvironment()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionSkybox(originalSkyboxMaterial, newSkyboxMaterial));
        }
    }

    public void OnEvent(LightReflectionEvent eventType)
    {
        switch(eventType.LightReflectionEventType)
        {
            case LightReflectionEventType.HitTarget:
                ChangeEnvironment();
                break;
        }
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<LightReflectionEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<LightReflectionEvent>();
    }
}