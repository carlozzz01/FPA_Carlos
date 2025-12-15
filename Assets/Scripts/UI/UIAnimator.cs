using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UIAnimator
{
    private class Runner : MonoBehaviour { }
    private static Runner _runner;

    private static void EnsureRunnerExists()
    {
        if (_runner == null)
        {
            var go = new GameObject("UIAnimation_Runner");

            UnityEngine.Object.DontDestroyOnLoad(go);

            _runner = go.AddComponent<Runner>();
        }
    }

    // todo este código ha sido comprimido por ChatGPT
    // antes eran 8 static voids distintos, cada uno con diferentes parametros, y le pedi ayuda a la ia pa sintetizarlo
    // asi que voy a intentar explicar cómo funciona esa sintetización tal y cómo me la ha explicado la ia y yo la he entendido
    // - solty

    // esta todo sintetizado de manera que se pueda crear una funcion para cada cosa que se pueda fadear
    // public Fade(CanvasGroup);
    // public Fade(Image)
    // public Fade(SpriteRenderer);
    // etc...

    // primero se encuentran las llamadas a esas funciones
    // aquí, como programador, tienes que definir tú las funciones que se deban hacer o no al final del fade
    // por ejemplo, con el CanvasGroup debe cuidarse el .interactable y el .blocksRaycasts
    // como el Image no necesita nada después, el after no se define y se pasa como null 

    public static void Fade(CanvasGroup group, bool fadeIn, float duration, bool timeScaled = true, Action onComplete = null)
    {
        // este void necesita (bool, float, bool opcional, onComplete opcional, setter, getter, after opcional )
        StartGenericFade(
            fadeIn,                       // este es el bool
            duration,                     // este es el float
            timeScaled,                   // este es el bool opcional
            onComplete,                   // este es el onComplete opcional
            () => group.alpha,            // este es el getter
            value => group.alpha = value, // este es el setter
            () =>                         // esto es el after opcional
            {
                group.interactable = fadeIn;
                group.blocksRaycasts = fadeIn;
            }
        );
    }


    public static void Fade(Image image, bool fadeIn, float duration, bool timeScaled = true, Action onComplete = null)
    {
        // este void necesita (bool, float, bool opcional, onComplete opcional, setter, getter, after opcional )
        StartGenericFade(
            fadeIn,                  // este es el bool
            duration,                // este es el float
            timeScaled,              // este es el bool opcional
            onComplete,              // este es el onComplete opcional
            () => image.color.a,     // este es el getter
            value =>                 // este es el setter
            {
                Color color = image.color;
                color.a = value;
                image.color = color;
            }
            // aquí no hay after, porque una vez hemos pasado el alpha de la imagen al objetivo, no hay nada más que hacer
        );
    }

    // aquí está la chicha, el trabajo que hace el fade

    // Núcleo genérico
    private static void StartGenericFade(bool fadeIn, float duration, bool timeScaled, Action onComplete, Func<float> getter, Action<float> setter, Action after = null)
    {
        EnsureRunnerExists();
        
        _runner.StartCoroutine(FadeRoutine(fadeIn, duration, timeScaled, onComplete, getter, setter, after));
    }

    // este IEnumerator recibe 3 cosas que a primeras pueden parecer raras
    // Func<float> getter
    // Action<float> setter
    // Action after
    private static IEnumerator FadeRoutine(bool fadeIn, float duration, bool timeScaled, Action onComplete, Func<float> getter, Action<float> setter, Action after)
    {
        // llamar al getter devuelve el valor que se le pasa en StartGenericFade();
        float start = getter();

        float end = fadeIn ? 1f : 0f;
        
        float t = 0f;

        while (t < duration)
        {
            t += timeScaled ? Time.deltaTime : Time.unscaledDeltaTime;

            setter(Mathf.Lerp(start, end, t / duration));

            yield return null;
        }

        // llamar al setter pasandole el float, ejecuta la lógica pasa al StartGenerigFade();
        setter(end);

        // el after ejecuta la lógica, cómo el setter, pero al ser opcional, se define como after = null en el StartGenericFade 
        after?.Invoke();

        // igual que el after, en Fade(), el onComplete = null porque es opcional.
        onComplete?.Invoke();
    }

    public static void StopAllCoroutines()
    {
        _runner.StopAllCoroutines();
    }
}
