using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    public Slider brightnessSlider;

    public PostProcessProfile brightness;
    public PostProcessLayer layer;

    AutoExposure exposure;

    private const string BrightnessKey = "BrightnessValue";

    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);

        // Cargar el valor guardado o usar un valor por defecto
        float savedBrightness = PlayerPrefs.GetFloat(BrightnessKey, 0.05f);
        exposure.keyValue.value = savedBrightness;

        // Actualizar el slider para que refleje el valor cargado
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        }
    }

    public void AdjustBrightness(float value)
    {
        if (value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = 0.05f;
        }

        // Guardar el valor actual en PlayerPrefs para autoguardado
        PlayerPrefs.SetFloat(BrightnessKey, exposure.keyValue.value);
        PlayerPrefs.Save();
    }
}

