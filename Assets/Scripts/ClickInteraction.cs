using UnityEngine;

public class ClickInteractor : MonoBehaviour
{
    public Camera cam;
    public MixStation mixStation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Ingredient ingredient = hit.collider.GetComponent<Ingredient>();

                if (ingredient != null)
                {
                    mixStation.AddIngredient(ingredient);
                }
            }
        }
    }
}
