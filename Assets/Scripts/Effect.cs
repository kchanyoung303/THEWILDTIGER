using UnityEngine;

public class Effect : MonoBehaviour {

    public void Init(Vector3 startPos)
    {
        transform.position = startPos;
        gameObject.SetActive(true);
        Invoke("Despawn", 1);
    }

    void Despawn()
    {
        gameObject.SetActive(false);
    }
}
