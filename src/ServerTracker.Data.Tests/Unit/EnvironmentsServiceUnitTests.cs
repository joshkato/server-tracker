using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.NSubstitute;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ServerTracker.Data.Repositories;
using ServerTracker.Data.Services;
using Xunit;
using Xunit.Abstractions;

namespace ServerTracker.Data.Tests.Unit
{
    using Environment = Models.Environment;

    [Trait("Category", "Unit")]
    public class EnvironmentsServiceUnitTests
    {
        private ITestOutputHelper Output { get; }

        private ILoggerFactory LoggerFactory { get; }

        public EnvironmentsServiceUnitTests(ITestOutputHelper output)
        {
            Output = output;
            LoggerFactory = new LoggerFactory(new []
            {
                new XunitLoggerProvider(Output), 
            });
        }

        [Fact]
        public async Task AddNewEnvironment_ReturnsError_WhenEnvironmentIsNull()
        {
            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<EnvironmentsService>();

            var error = await sut.AddNewEnvironment(null).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Matches("(?i)cannot add empty or invalid environment", error.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\r\n")]
        public async Task AddNewEnvironment_ReturnsError_WhenEnvironmentNameInvalid(string testName)
        {
            var autoSub = GetAutoSub();
            var sut = autoSub.Resolve<EnvironmentsService>();

            var error = await sut.AddNewEnvironment(new Environment {Name = testName}).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Contains("missing", error.Message);
            Assert.Contains("invalid", error.Message);
        }

        [Fact]
        public async Task AddNewEnvironment_ReturnsError_WhenEnvironmentsRepositoryThrows()
        {
            var testEnv = new Environment { Name = "Test" };

            var autoSub = GetAutoSub();
            var envsRepo = autoSub.Resolve<IEnvironmentsRepository>();
            var sut = autoSub.Resolve<EnvironmentsService>();

            envsRepo.AddNewEnvironment(Arg.Any<Environment>())
                .Throws(ci => new Exception("Simulating repository exception"));

            var error = await sut.AddNewEnvironment(testEnv).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Contains("Failed to add new environment", error.Message);
            Assert.NotNull(error.Exception);
        }

        [Fact]
        public async Task AddNewEnvironment_ReturnsNull_WhenEnvironmentCreatedSuccessfully()
        {
            var testEnv = new Environment {Name = "Test"};

            var autoSub = GetAutoSub();
            var envsRepo = autoSub.Resolve<IEnvironmentsRepository>();
            var sut = autoSub.Resolve<EnvironmentsService>();

            var error = await sut.AddNewEnvironment(testEnv).ConfigureAwait(false);

            Assert.Null(error);

            await envsRepo.Received().AddNewEnvironment(Arg.Is(testEnv));
        }

        [Fact]
        public async Task DeleteEnvironment_ReturnsError_WhenRepositoryThrows()
        {
            var autoSub = GetAutoSub();
            var envsRepo = autoSub.Resolve<IEnvironmentsRepository>();
            var sut = autoSub.Resolve<EnvironmentsService>();

            envsRepo.DeleteEnvironment(Arg.Any<long>())
                .Throws(ci => new Exception("Simulating repository exception"));

            var error = await sut.DeleteEnvironment(1).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Contains("delete", error.Message);
            Assert.NotNull(error.Exception);
        }

        [Fact]
        public async Task DeleteEnvironment_ReturnsNull_WhenEnvironmentIsSuccessfullyDeleted()
        {
            var autoSub = GetAutoSub();
            var envsRepo = autoSub.Resolve<IEnvironmentsRepository>();
            var sut = autoSub.Resolve<EnvironmentsService>();

            var error = await sut.DeleteEnvironment(1).ConfigureAwait(false);

            Assert.Null(error);

            await envsRepo.Received().DeleteEnvironment(Arg.Is(1L));
        }

        [Fact]
        public async Task GetAllEnvironments_ReturnsError_WhenRepositoryThrows()
        {
            var autoSub = GetAutoSub();
            var envsRepo = autoSub.Resolve<IEnvironmentsRepository>();
            var sut = autoSub.Resolve<EnvironmentsService>();

            envsRepo.GetAllEnvironments()
                .Throws(ci => new Exception("Simulating repository exception"));

            var (result, error) = await sut.GetAllEnvironments().ConfigureAwait(false);

            Assert.Null(result);
            Assert.NotNull(error);
            Assert.NotNull(error.Exception);
            Assert.Contains("Failed", error.Message);
        }

        private AutoSubstitute GetAutoSub()
        {
            ILogger<T> GetLogger<T>(IComponentContext ctx)
            {
                return ctx.Resolve<ILoggerFactory>().CreateLogger<T>();
            }

            var builder = new ContainerBuilder();
            builder.RegisterInstance(LoggerFactory);
            builder.Register(GetLogger<EnvironmentsService>)
                .As<ILogger<EnvironmentsService>>();

            var autoSub = new AutoSubstitute(builder);

            return autoSub;
        }
    }
}
