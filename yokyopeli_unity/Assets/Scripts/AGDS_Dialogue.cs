using UnityEngine;
using Q3Movement;
public class AGDS_Dialogue : MonoBehaviour
{
    public Q3Movement.Q3PlayerController player;
    private void Awake()
    {
        player = MainGameObject.Instance.playerController;
    }
}
