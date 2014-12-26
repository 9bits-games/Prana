using UnityEngine;
using System.Collections;

public class WindBlowController : BaseMonoBehaviour {
    public Transform WindMark;

    public float Radius = 1f;
    public int MarkCount = 5;
    public float LifeTime = 2f;

    private float timeLiving;
    private Transform[] windMarks;
    private float baseOpacity; 

    void Awake() {
        windMarks = new Transform[MarkCount];

        for (int i = 0; i < MarkCount; i++) {
            windMarks[i] = placeWindMark();
        }
    }

    void Start() {
        timeLiving = 0f;
        baseOpacity = windMarks[0].renderer.material.GetColor("_TintColor").a;
    }

    void Update() {
        timeLiving += Time.deltaTime;

        float fadeTime = 0.5f * LifeTime;

        if (timeLiving < fadeTime) {
            setOpacity(Mathf.InverseLerp(0f, fadeTime, timeLiving));
        } else if (timeLiving > LifeTime - fadeTime) {
            setOpacity(1f - Mathf.InverseLerp(LifeTime - fadeTime, LifeTime, timeLiving));
        } 

        if (timeLiving > LifeTime) {
            Destroy(gameObject);
        }
    }

    void setOpacity(float opacity) {
        foreach (Transform windMark in windMarks) {
            Material material = windMark.renderer.material;
          
            Color color = material.GetColor("_TintColor");
            color.a = opacity * baseOpacity;
            material.SetColor("_TintColor", color);
        }

    }

    Transform placeWindMark() {
        Vector3 position = this.transform.position + new Vector3(
            Random.Range(-Radius, Radius),
            Random.Range(-Radius, Radius),
            Random.Range(-Radius, Radius)
        );

        Transform newMark = Instantiate(WindMark, position, Quaternion.identity) as Transform;
        newMark.parent = this.transform;

        return newMark;
    }
}
