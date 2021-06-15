using Xunit;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        CalovoParser p = new CalovoParser();
        Assert.Equal("", p.GetNextEvent().summary);
    }
    [Fact]
    public void Test2()
    {
        CalovoParser p = new CalovoParser();
        Assert.True("" == p.GetNextEvent().summary);
    }
}
