using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise;

namespace Promise.Tests;

[TestClass()]
public class BindingCommandTests
{
    BindingCommand bindingCommand = default!;

    [TestInitialize]
    public void Init()
    {
        bindingCommand = new BindingCommand(BindingAction);
    }

    [TestMethod()]
    public void CanExecuteTest()
    {
        Assert.IsTrue(bindingCommand.CanExecute());
    }

    [TestMethod()]
    public void ExecuteTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void SetGlobalCommandExceptionCallbackTest()
    {
        bindingCommand = new BindingCommand(() => throw new Exception());

        Assert.ThrowsException<Exception>(() => bindingCommand.Execute());
    }

    private void BindingAction() { }
}
