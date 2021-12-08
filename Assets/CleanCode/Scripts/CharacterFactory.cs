using UnityEngine;

public class CharacterFactory {
    public static ICharacter CreateCharacter(string type, Transform transform) => type switch {
        "avatar" => new Avatar(transform),
        "enemy" => throw new System.NotImplementedException(),
        _ => throw new System.NotImplementedException(),
    };
}
