using UnityEngine;

public class PlayableBoundsController: BaseMonoBehaviour {

    public Transform player;
    public Wind wind;

    private bool offBounds;
    private Vector3 correctingWind;

    void Start() {
        offBounds = false;
    }

    void Update () {
        Vector3 playerPos = player.position;
        Vector3 localPlayerPos = this.transform.InverseTransformPoint(playerPos);
        Vector3 excedingDirection;

        if (excedingBounds(localPlayerPos, out excedingDirection)) {
            correctingWind = excedingDirection;
            offBounds = true;
        } else {
            offBounds = false;
        }
    }

    void FixedUpdate() {
        if (offBounds) {
            wind.ApplyWind(10f * correctingWind * wind.MaxWindStrenght);
        }
    }

    bool excedingBounds(Vector3 position, out Vector3 direction) {
        direction = Vector3.zero;

        if (Mathf.Abs(position.x) > 0.5f) {
            direction = -Vector3.right * Mathf.Sign(position.x);
        } else if (Mathf.Abs(position.y) > 0.5f) {
            direction = -Vector3.up * Mathf.Sign(position.y);
        } else if (Mathf.Abs(position.z) > 0.5f) {
            direction = -Vector3.forward * Mathf.Sign(position.z);
        }

        return direction != Vector3.zero;
    }
}
