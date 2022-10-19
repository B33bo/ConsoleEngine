using ConsoleEngine.Sound;
namespace ConsoleEngineTesting;

[TestClass]
public class SoundTests
{
    [TestMethod]
    public void NoteTest()
    {
        Assert.IsTrue(Note.TryParse("F", 4, out _));
        Assert.IsTrue(Note.TryParse("F#", 4, out _));
        Assert.IsTrue(Note.TryParse("Ab", 4, out _));
        Assert.IsTrue(Note.TryParse("Db", 4, out _));
        Assert.IsTrue(Note.TryParse("Db3", 4, out _));
        Assert.IsTrue(Note.TryParse("D#", 4, out _));
        Assert.IsTrue(Note.TryParse("D#3", 4, out _));
        Assert.IsTrue(Note.TryParse("D3", 4, out _));
        Assert.IsTrue(Note.TryParse("", 4, out _));
        Assert.IsTrue(Note.TryParse(" ", 4, out _));

        Assert.IsFalse(Note.TryParse("F", -1324, out _));

        Assert.IsFalse(Note.TryParse("P#", 4, out _));
        Assert.IsFalse(Note.TryParse("Pb", 4, out _));
        Assert.IsFalse(Note.TryParse("Pb4", 4, out _));
        Assert.IsFalse(Note.TryParse("P#4", 4, out _));
        Assert.IsFalse(Note.TryParse("P", 4, out _));
    }
}
