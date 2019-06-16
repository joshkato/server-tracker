using Autofac;
using Autofac.Extras.NSubstitute;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Models;
using ServerTracker.Data.Validation;
using Xunit;
using Xunit.Abstractions;

namespace ServerTracker.Data.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServerValidatorUnitTests
    {
        private ITestOutputHelper Output { get; }

        private ILoggerFactory LoggerFactory { get; }

        public ServerValidatorUnitTests(ITestOutputHelper output)
        {
            Output = output;
            LoggerFactory = new LoggerFactory(new[]
            {
                new XunitLoggerProvider(Output),
            });
        }

        [Fact]
        public void Validate_ReturnsNotValid_WhenServerNull()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<ServerValidator>();

            var result = sut.Validate(testServer);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.ValidationErrors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_ReturnsNotValid_WhenServerNameInvalid(string serverName)
        {
            var testServer = new Server
            {
                Name = serverName,
            };

            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<ServerValidator>();

            var result = sut.Validate(testServer);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Contains(result.ValidationErrors, m => m.Contains("Name is null, empty, or whitespace"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_ReturnsNotValid_WhenDomainNameInvalid(string domainName)
        {
            var testServer = new Server
            {
                Name = "Test Server",
                DomainName = domainName,
            };

            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<ServerValidator>();

            var result = sut.Validate(testServer);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Contains(result.ValidationErrors, m => m.Contains("DomainName is null, empty, or whitespace"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_ReturnsNotValid_WhenIpAddressInvalid(string ipAddress)
        {
            var testServer = new Server
            {
                Name = "Test Server",
                DomainName = "server.test.tld",
                IpAddress = ipAddress,
            };

            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<ServerValidator>();

            var result = sut.Validate(testServer);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Contains(result.ValidationErrors, m => m.Contains("IpAddress is null, empty, or whitespace"));
        }

        [Theory]
        [InlineData("not-an-ip-address")]
        public void Validate_ReturnsNoValid_WhenIpAddressValueIsNotIpAddress(string ipAddress)
        {
            var testServer = new Server
            {
                Name = "Test Server",
                DomainName = "server.test.tld",
                IpAddress = ipAddress,
            };

            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<ServerValidator>();

            var result = sut.Validate(testServer);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Contains(result.ValidationErrors, m => m.Contains("is not a valid IP address"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_ReturnsNotValid_WhenOperatingSystemInvalid(string os)
        {
            var testServer = new Server
            {
                Name = "Test Server",
                DomainName = "server.test.tld",
                IpAddress = "127.0.0.1",
                OperatingSystem = os,
            };

            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<ServerValidator>();

            var result = sut.Validate(testServer);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Contains(result.ValidationErrors, m => m.Contains("OperatingSystem is null, empty, or whitespace"));
        }

        private AutoSubstitute GetAutoSub()
        {
            ILogger<T> GetLogger<T>(IComponentContext ctx)
            {
                return ctx.Resolve<ILoggerFactory>().CreateLogger<T>();
            }

            var builder = new ContainerBuilder();
            builder.RegisterInstance(LoggerFactory);
            builder.Register(GetLogger<ServerValidatorUnitTests>)
                .As<ILogger<ServerValidatorUnitTests>>();

            var autoSub = new AutoSubstitute(builder);

            return autoSub;
        }
    }
}
