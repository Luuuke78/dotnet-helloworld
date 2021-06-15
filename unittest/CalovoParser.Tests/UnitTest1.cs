using System;
using Xunit;
using Parser.Services;

namespace CalovoParser.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Parser.Services.CalovoParser p = new Parser.Services.CalovoParser();
            Assert.Equal("", p.GetNextEvent().summary);
        }
        [Fact]
        public void Test2()
        {
            Parser.Services.CalovoParser p = new Parser.Services.CalovoParser();
            Assert.True("" == p.GetNextEvent().summary);
        }
    }
}
