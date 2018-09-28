using System;
using System.Collections.Generic;
using System.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using Katarai.Controls;
using Katarai.Wpf.Monitor;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    [Ignore("Because nunit 3.x needs this populated")]
    public class TestTaskbarIconAdapter
    {
        readonly Stack<IDisposable> _itemsToDispose = new Stack<IDisposable>();

        [TearDown]
        public void TesrDown()
        {
            while (_itemsToDispose.Count > 0)
            {
                var disposable = _itemsToDispose.Pop();
                disposable.Dispose();
            }
        }
        
        [Apartment(ApartmentState.STA)]
        [Test]
        public void Dispose_ShouldDisposeTrayIcon()
        {
            //---------------Set up test pack-------------------
            var taskbarIcon = new TaskbarIcon();
            var toast = new Toast(taskbarIcon);
            var adapter = CreateAdapter(toast);
            //---------------Assert Precondition----------------
            Assert.IsFalse(toast.IsDisposed);
            //---------------Execute Test ----------------------
            adapter.Dispose();
            //---------------Test Result -----------------------
            Assert.IsTrue(toast.IsDisposed);
        }

        [Test]
        public void Constructor_WhenTaskBarIconIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new ToastDisplayer(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("toast",exception.ParamName);
        }


        private ToastDisplayer CreateAdapter(IToast toast = null)
        {
            var taskbarIconAdapter = new ToastDisplayer(toast ?? new Toast(new TaskbarIcon()));
            _itemsToDispose.Push(taskbarIconAdapter);
            return taskbarIconAdapter;
        }
    }
}