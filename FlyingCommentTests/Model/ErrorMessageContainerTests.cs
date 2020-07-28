using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Model.Tests
{
    [TestClass()]
    public class ErrorMessageContainerTests
    {
        [TestMethod()]
        public void ErrorMessageContainerTest()
        {

            ErrorMessageContainer err = new ErrorMessageContainer();


            err = new ErrorMessageContainer("pro", "data");
            Assert.AreEqual("data", err["pro"]);

            err.Add("pro", "Data2");
            Assert.AreEqual("Data2", err["pro"]);

            err.Remove("pro");
            Assert.AreEqual(null, err["pro"]);


        }
    }
}