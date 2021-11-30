using UnityEngine;

public class AvatarScript : MonoBehaviour
{
    [SerializeField]
    private string type = "avatar";
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private ICharacter behaviour;
    [SerializeField]
    private IMover mover;

    // Start is called before the first frame update
    private void Awake()
    {
        behaviour = CharacterFactory.CreateCharacter(type, transform);
        behaviour.speed = speed;
    }

    private void Start()
    {
        behaviour.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        behaviour.Update();
    }
}
