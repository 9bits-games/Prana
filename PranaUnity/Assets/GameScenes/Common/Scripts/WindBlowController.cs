using UnityEngine;
using System.Collections;

public class WindBlowController : BaseMonoBehaviour {
    public delegate void DestroyDelegate();
    public event DestroyDelegate OnDestroyEvent;

    public Transform WindMark;

    public float Radius = 1f;
    public int MarkCount = 5;
    public float LifeTime = 2f;
    public float MaxSpeed = 2f;
    public float SpeedMultiplier = 0.25f;

    private float TimeLiving;
    private Transform[] WindMarks;
    private float BaseOpacity;


    public void addVelocity(Vector3 velocity) {
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity + velocity * SpeedMultiplier, MaxSpeed);
    }

    void Awake() {
        WindMarks = new Transform[MarkCount];

        for (int i = 0; i < MarkCount; i++) {
            WindMarks[i] = PlaceWindMark();
        }
    }

    void Start() {
        TimeLiving = 0f;
        BaseOpacity = WindMarks[0].renderer.material.GetColor("_TintColor").a;
    }

    void Update() {
        TimeLiving += Time.deltaTime;

        float fadeTime = 0.5f * LifeTime;

        if (TimeLiving < fadeTime) {
            SetOpacity(Mathf.InverseLerp(0f, fadeTime, TimeLiving));
        } else if (TimeLiving > LifeTime - fadeTime) {
            SetOpacity(1f - Mathf.InverseLerp(LifeTime - fadeTime, LifeTime, TimeLiving));
        } 

        if (TimeLiving > LifeTime) {
            Destroy(gameObject);
        }
    }

    void SetOpacity(float opacity) {
        foreach (Transform windMark in WindMarks) {
            Material material = windMark.renderer.material;
          
            Color color = material.GetColor("_TintColor");
            color.a = opacity * BaseOpacity;
            material.SetColor("_TintColor", color);
        }

    }

    Transform PlaceWindMark() {
        Vector3 position = this.transform.position + new Vector3(
            Random.Range(-Radius, Radius),
            Random.Range(-Radius, Radius),
            Random.Range(-Radius, Radius)
        );

        Transform newMark = Instantiate(WindMark, position, Quaternion.identity) as Transform;
        newMark.parent = this.transform;

        return newMark;
    }

    protected void OnDestroy() {
        if (OnDestroyEvent != null)
            OnDestroyEvent();
    }
}
