using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

//
[RequiresPlayMode(false)]
public class NewTestScript {
    const string PATH = "Assets/TestDrivenDevelopment/";

    string spriteName;
    string colorHex;
    bool expectedResult;
    Vector2 expectedPosition;
    Texture2D sprite;
    Color color;

    TextureUtility sut;

    public NewTestScript() {
    }

    [TestCase("Sprite-0001.png", "#ff0000", true, 0f, 0f)]
    [TestCase("Sprite-0001.png", "#000000", false, 0f, 0f)]
    [TestCase("Sprite-0005.png", "#7dff00", true, -0.375f, 0.40625f, TestOf = typeof(NewTestScript))]
    public void T01_TryGetAveragePositionOfResult(string spriteName, string colorHex, bool expectedResult, float expectedX, float expectedY) {
        this.spriteName = spriteName;
        this.colorHex = colorHex;
        this.expectedResult = expectedResult;
        expectedPosition = new(expectedX, expectedY);

        sprite = AssetDatabase.LoadAssetAtPath<Texture2D>($"{PATH}{spriteName}");
        Assert.IsTrue(sprite);
        Assert.IsTrue(ColorUtility.TryParseHtmlString(colorHex, out color));

        sut = new(sprite);

        bool actualResult = sut.TryGetAveragePositionOf(color, out _);

        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestCase("Sprite-0001.png", "#ff0000", true, 0f, 0f)]
    [TestCase("Sprite-0001.png", "#000000", false, 0f, 0f)]
    [TestCase("Sprite-0005.png", "#7dff00", true, -0.375f, 0.40625f, TestOf = typeof(NewTestScript))]
    public void T02_TryGetAveragePositionOfPosition(string spriteName, string colorHex, bool expectedResult, float expectedX, float expectedY) {
        this.spriteName = spriteName;
        this.colorHex = colorHex;
        this.expectedResult = expectedResult;
        expectedPosition = new(expectedX, expectedY);

        sprite = AssetDatabase.LoadAssetAtPath<Texture2D>($"{PATH}{spriteName}");
        Assert.IsTrue(sprite);
        Assert.IsTrue(ColorUtility.TryParseHtmlString(colorHex, out color));

        sut = new(sprite);

        _ = sut.TryGetAveragePositionOf(color, out var actualPosition);

        Assert.AreEqual(expectedPosition, actualPosition);
    }
}
