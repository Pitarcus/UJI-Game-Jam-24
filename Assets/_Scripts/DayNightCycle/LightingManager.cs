using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    public Light directionalLight;
    public LightingConditions preset;

    [Header("Parameters")]
    [SerializeField] public float dayPeriod = 24;
    [SerializeField] public bool rotateAllDirections = false;
    [SerializeField] [Range(-360, 360)]public float yRotation = -100f;
    [SerializeField] [Range(0, 1)] public float timeOfDay;

    // Private memebers
    private bool _cycleRunning = true;
    public Transform _originalLightTransform;
    private Quaternion _originalLightRotation;
    private Color _originalFogColor;
    private float _realTimeOfDay;


    private void Start()
    {
        _originalLightRotation = _originalLightTransform.rotation;
        _originalFogColor = RenderSettings.fogColor;

        _realTimeOfDay = timeOfDay * dayPeriod;
    }

    

    private void Update()
    {
        if (preset == null)
            return;

        if (!_cycleRunning)
            return;

        if(Application.isPlaying)
        {
            _realTimeOfDay += Time.deltaTime;
            _realTimeOfDay %= dayPeriod;
            UpdateLighting(_realTimeOfDay / dayPeriod);
        }
        else
        {
            UpdateLighting(_realTimeOfDay / dayPeriod);
        }
    }

    public void StartCycle()
    {
        _cycleRunning = true;
    }

    public void StopCycle()
    {
        _cycleRunning = false;
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if(directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            if(!rotateAllDirections)
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, yRotation, 0));
            else
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) + 55f, (timePercent * 2 * 360f) - 45f, 0));
        }
    }

    private void OnValidate()
    {

        _realTimeOfDay = timeOfDay * dayPeriod;

        if (directionalLight != null)
        {
            return;
        }
        if(RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

}
