using System;
using GameFrameX.LitJSON.Runtime;
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

    internal class HttpJsonResultHelperTests
    {
        private class SampleData
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        [Test]
        public void ToHttpJsonResultData_SuccessBody_ParsesData()
        {
            var result = "{\"code\":0,\"message\":\"ok\",\"data\":\"{\\\"name\\\":\\\"alpha\\\"}\"}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.True, "code 为 0 的约定响应应解析为成功");
            Assert.That(result.Data.Name, Is.EqualTo("alpha"));
        }

        [Test]
        public void ToHttpJsonResultData_BusinessErrorCode_ReturnsFailure()
        {
            var result = "{\"code\":1001,\"message\":\"参数错误\"}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Code, Is.EqualTo(1001));
            Assert.That(result.Message, Is.EqualTo("参数错误"));
        }

        [Test]
        public void ToHttpJsonResultData_ProblemDetailsStatus500_ReturnsFailure()
        {
            var result = "{\"type\":\"about:blank\",\"title\":\"Internal Server Error\",\"status\":500}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.False, "缺少 code 字段的 500 错误体不得被视为成功");
            Assert.That(result.Code, Is.EqualTo(500));
            Assert.That(result.Message, Is.EqualTo("Internal Server Error"));
        }

        [Test]
        public void ToHttpJsonResultData_LegacyStatusCode_ReturnsFailure()
        {
            var result = "{\"statusCode\":503,\"message\":\"Service Unavailable\"}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Code, Is.EqualTo(503));
            Assert.That(result.Message, Is.EqualTo("Service Unavailable"));
        }

        [Test]
        public void ToHttpJsonResultData_SpringBootErrorBody_ReturnsFailure()
        {
            var result = "{\"timestamp\":\"2026-10-15T00:00:00Z\",\"status\":500,\"error\":\"Internal Server Error\",\"path\":\"/api\"}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Code, Is.EqualTo(500));
            Assert.That(result.Message, Is.EqualTo("Internal Server Error"));
        }

        [Test]
        public void ToHttpJsonResultData_HtmlErrorPage_ReturnsFailureWith500()
        {
            var result = "<html><body>500 Internal Server Error</body></html>".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.False, "非 JSON 错误页不得被视为成功");
            Assert.That(result.Code, Is.EqualTo(500));
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        public void ToHttpJsonResultData_EmptyOrNullBody_ReturnsFailureWith500()
        {
            var empty = "".ToHttpJsonResultData<SampleData>();
            var nullBody = ((string)null).ToHttpJsonResultData<SampleData>();

            Assert.That(empty.IsSuccess, Is.False);
            Assert.That(empty.Code, Is.EqualTo(500));
            Assert.That(nullBody.IsSuccess, Is.False);
            Assert.That(nullBody.Code, Is.EqualTo(500));
        }

        [Test]
        public void ToHttpJsonResultData_JsonNullLiteral_ReturnsFailureWith500()
        {
            var result = "null".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Code, Is.EqualTo(500));
        }

        [Test]
        public void ToHttpJsonResultData_StringStatusField_DoesNotBreakSuccess()
        {
            var result = "{\"status\":\"ok\",\"data\":\"{\\\"name\\\":\\\"beta\\\"}\"}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.True, "非数值 status 字段不应误判为服务器错误");
            Assert.That(result.Data.Name, Is.EqualTo("beta"));
        }

        [Test]
        public void ToHttpJsonResultData_ZeroStatusFields_StillSuccess()
        {
            var result = "{\"code\":0,\"status\":0,\"data\":\"{\\\"name\\\":\\\"gamma\\\"}\"}".ToHttpJsonResultData<SampleData>();

            Assert.That(result.IsSuccess, Is.True, "status 为 0 时不得误判为服务器错误");
            Assert.That(result.Data.Name, Is.EqualTo("gamma"));
        }
    }
}