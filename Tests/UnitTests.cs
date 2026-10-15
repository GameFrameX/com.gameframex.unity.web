using System;
using GameFrameX.Runtime;
using GameFrameX.Web.Runtime;
using NUnit.Framework;

namespace GameFrameX.Web.Tests
{
    internal class UnitTests
    {
        private DateTime dateTime, dateTime1;

        [SetUp]
        public void Setup()
        {
            dateTime = DateTime.Now;
            dateTime1 = DateTime.Now.AddHours(1);
        }

        [Test]
        public void Test1()
        {
            Assert.That(dateTime1.Year, Is.EqualTo(dateTime.Year));
            Assert.That(dateTime1.Month, Is.EqualTo(dateTime.Month));
            Assert.That(dateTime1.Day, Is.EqualTo(dateTime.Day));
        }
    }

    internal class WebOptionsTests
    {
        [TearDown]
        public void TearDown()
        {
            GameFrameXOptions.Reset();
        }

        [Test]
        public void Ctor_ConsumesWebOptions_FromRegistry()
        {
            GameFrameXOptions.Reset();
            GameFrameXOptions.Configure(new WebOptions { Timeout = 42f });

            var manager = new WebManager();

            Assert.That(manager.Timeout, Is.EqualTo(42f), "模块首次实例化应消费 WebOptions.Timeout");
        }

        [Test]
        public void Ctor_UsesDefaultTimeout_WhenOptionsNotConfigured()
        {
            GameFrameXOptions.Reset();

            var manager = new WebManager();

            Assert.That(manager.Timeout, Is.EqualTo(5f), "未注入 Options 时应回落到与原序列化默认值同源的 5 秒");
        }
    }
}