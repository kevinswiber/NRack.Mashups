using NUnit.Framework;

namespace Mixup.Tests
{
    [TestFixture]
    public class JavaScriptEvaluator_Tests
    {
        [Test]
        public void ReturnsObject()
        {
            var evaluator = new JavaScriptEvaluator();

            Assert.IsNotNull(evaluator.Evaluate());
        }
    }
}
