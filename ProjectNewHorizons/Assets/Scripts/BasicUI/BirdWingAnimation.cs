using UnityEngine;

public class UIPlayAnimation : MonoBehaviour
{
    [SerializeField] private Animator[] animator;
    public void Play(string triggerName)
    {
        switch (triggerName)
        {
            case "BuyUpgrade":
                    if (animator[0] == null)
                    {
                        Debug.LogError("Animator not assigned!");
                        return;
                    }
                animator[0].SetTrigger(triggerName);
                break;
            case "BuyTower":
                    if (animator[1] == null)
                    {
                        Debug.LogError("Animator not assigned!");
                        return;
                    }
                animator[1].SetTrigger(triggerName);
                break;
            default:
                Debug.LogError("Invalid trigger name: " + triggerName);
                break;
        }
    }
}