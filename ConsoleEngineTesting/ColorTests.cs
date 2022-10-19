namespace ConsoleEngineTesting;

[TestClass]
public class ColorTests
{
    [TestMethod]
    public void HSVtoRGB()
    {
        Assert.AreEqual(new Color(255, 0, 0), (Color)new ColorHSV(0, 1, 1));
        Assert.AreEqual(new Color(233, 255, 0), (Color)new ColorHSV(65, 1, 1));
        Assert.AreEqual(new Color(0, 255, 21), (Color)new ColorHSV(125, 1, 1));
        Assert.AreEqual(new Color(0, 233, 255), (Color)new ColorHSV(185, 1, 1));
        Assert.AreEqual(new Color(21, 0, 255), (Color)new ColorHSV(245, 1, 1));
        Assert.AreEqual(new Color(255, 0, 233), (Color)new ColorHSV(305, 1, 1));
        Assert.AreEqual(new Color(255, 0, 233), (Color)new ColorHSV(305, 1, 1));

        Assert.AreEqual(new Color(127, 117, 114), (Color)new ColorHSV(15, .1f, .5f));
    }

    [TestMethod]
    public void RGBtoHSV()
    {
        Assert.AreEqual(new ColorHSV(341, .6f, 1), (ColorHSV)new Color(255, 102, 152));

        ColorHSV result = (ColorHSV)new Color(255, 150, 100);
        ColorHSV expected = new ColorHSV(18.422228f, 0.60784316f, 1);
        Assert.IsTrue(
            (int)result.Hue == (int)expected.Hue && (int)result.Saturation == (int)expected.Saturation && (int)result.Value == (int)expected.Value);
    }
}