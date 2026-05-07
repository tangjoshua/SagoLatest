using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;

    public void OnClick(MixStation mixStation)
    {
        mixStation.AddIngredient(this);
    }
}
