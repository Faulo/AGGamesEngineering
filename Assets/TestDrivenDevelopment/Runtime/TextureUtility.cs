using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class TextureUtility {
    private Texture2D sprite;

    public TextureUtility(Texture2D sprite) => this.sprite = sprite;

    public bool TryGetAveragePositionOf(Color color, out Vector2 position) {
        var positions = new List<Vector2>();

        for (int x = 0; x < sprite.width; x++) {
            for (int y = 0; y < sprite.height; y++) {
                var pixel = sprite.GetPixel(x, y);
                if (pixel == color) {
                    positions.Add(new(x, y));
                }
            }
        }

        position = positions.Count == 0
            ? default
            : (positions.Aggregate((pos, sum) => pos + sum) / positions.Count) - (new Vector2(sprite.width - 1, sprite.height - 1) / 2);

        return positions.Count != 0;
    }
}
