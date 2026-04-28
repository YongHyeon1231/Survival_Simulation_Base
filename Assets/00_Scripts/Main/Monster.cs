using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    [SerializeField] private float Range;
    [SerializeField] private GameObject Board;
    [SerializeField] private Image Slider01Fill;
    [SerializeField] private Image Slider02Fill;

    Coroutine coroutine;
    Coroutine hit_Coroutine;
    Renderer renderer;

    private void Start()
    {
        HP = MaxHP;
        Slider01Fill.fillAmount = 1;
        Slider02Fill.fillAmount = 2;
        renderer = transform.GetComponentInChildren<Renderer>();
    }

    public void GetDamage(int dmg)
    {
        var playerPos = P_Movement.instance.transform.position;
        if(Vector3.Distance(transform.position, playerPos) <= Range)
        {
            Board.SetActive(true);
            Canvas_Holder.instance.GetText(dmg.ToString(), Color.yellow, transform.position);
            HP -= dmg;
            P_Movement.instance.GetComponent<Character>().GetHitParticle();

            if(coroutine != null) StopCoroutine(coroutine);

            coroutine = StartCoroutine(SliderCoroutine(HP));

            if(hit_Coroutine != null) StopCoroutine(hit_Coroutine);

            hit_Coroutine = StartCoroutine(GetHitCoroutine());
        }
    }

    IEnumerator GetHitCoroutine()
    {
        float current = 0.0f;
        float percent = 0.0f;
        Color startColor = Color.white;
        Color endColor = Color.black;

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;

            Color LerpColor = Color.Lerp(endColor, startColor, percent);
            renderer.material.SetColor("_EmissionColor", LerpColor);
            yield return null;
        }

        current = 0.0f;
        percent = 0.0f;

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;

            Color LerpColor = Color.Lerp(startColor, endColor, percent);
            renderer.material.SetColor("_EmissionColor", LerpColor);
            yield return null;
        }
    }

    IEnumerator SliderCoroutine(int hp)
    {
        float value = (float)hp/(float)MaxHP;
        Slider02Fill.fillAmount = value;
        float timer = 0.0f;
        while(timer <= 1.0f)
        {
            timer += Time.deltaTime;
            Slider01Fill.fillAmount = Mathf.Lerp(Slider01Fill.fillAmount, Slider02Fill.fillAmount, timer);
            yield return null;
        }
    }

}
