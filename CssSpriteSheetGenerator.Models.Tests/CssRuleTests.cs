using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class CssRuleTests
    {
        private CssRule cssRule;

        [TestInitialize]
        public void Initialize()
        {
            cssRule = new CssRule();
        }

        [TestMethod]
        public void ToString_ProducesProperlyFormattedString()
        {
            cssRule.Selectors.Add("Close");
            cssRule.Declarations.Add("background-image", "Close.png");

            var expected = ".Close\r\n" +
                           "{\r\n" +
                           "    background-image: Close.png\r\n" +
                           "}\r\n";
            Assert.AreEqual(expected, cssRule.ToString());
        }

        [TestMethod]
        public void ToString_WithMultipleSelectors_ProducesProperlyFormattedString()
        {
            cssRule.Selectors.Add("Close");
            cssRule.Selectors.Add("Open");
            cssRule.Declarations.Add("background-image", "Close.png");

            var expected = ".Close, .Open\r\n" +
                           "{\r\n" +
                           "    background-image: Close.png\r\n" +
                           "}\r\n";
            Assert.AreEqual(expected, cssRule.ToString());
        }
    }
}
