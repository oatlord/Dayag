using UnityEngine;

public class FloorArrow : MonoBehaviour
{
    private Renderer rend;
    private Material mat;
    private Animator anim;


    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (anim != null)
        anim.SetBool("IsActive", true);
    }

    public void Hide()
    {
        if (anim != null)
        anim.SetBool("IsActive", false);
        gameObject.SetActive(false);
    }
}