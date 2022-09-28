using ArgsCleanCode.Main.ArgsClass;

namespace ArgsCleanCode.TEST
{
    [TestClass]
    public class ArgsTest
    {

        [TestMethod]
        public void CreateArgsWithValidSchemaAndArgsGetAllParametersSuccessfuly()
        {
            const int expectedPort = 8080;
            const string expectedDir = "C:\\Windows";
            const string expectedComplexNum = "[1.4,1.0]";

            const string schema = "l\np#\nd*\nx[##,##]\n";

            var rawArgs = new string[] { "-l", $"-p {expectedPort}", $"-d {expectedDir}", $"-x {expectedComplexNum}" };
            var argsObj = new Args(schema, rawArgs);

            var logging = argsObj.GetBool('l');
            var port = argsObj.GetInt('p');
            var directory = argsObj.GetString('d');
            var complexNum = argsObj.GetComplex('x');

            Assert.IsTrue(logging);
            Assert.AreEqual(expectedPort, port);
            Assert.AreEqual(expectedDir, directory);
            Assert.AreEqual(new System.Numerics.Complex(1.4, 1.0), complexNum);
        }
    }
}