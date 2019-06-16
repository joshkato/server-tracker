using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.NSubstitute;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ServerTracker.Data.Models;
using ServerTracker.Data.Repositories;
using ServerTracker.Data.Services;
using ServerTracker.Data.Validation;
using Xunit;
using Xunit.Abstractions;

namespace ServerTracker.Data.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServersServiceUnitTests
    {
        private ITestOutputHelper Output { get; }

        private ILoggerFactory LoggerFactory { get; }

        public ServersServiceUnitTests(ITestOutputHelper output)
        {
            Output = output;
            LoggerFactory = new LoggerFactory(new[]
            {
                new XunitLoggerProvider(Output),
            });
        }

        [Fact]
        public async Task AddNewServer_ReturnsError_WhenValidationFails()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var validator = autoSub.Resolve<IServerValidator>();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            validator.Validate(Arg.Any<Server>())
                .Returns(ci => new ValidationResult {IsValid = false});

            var error = await sut.AddNewServer(testServer).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Contains("did not pass validation", error.Message);

            await serversRepo.DidNotReceive().AddNewServer(Arg.Any<Server>());
        }

        [Fact]
        public async Task AddNewServer_ReturnsError_WhenRepositoryThrows()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var validator = autoSub.Resolve<IServerValidator>();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            validator.Validate(Arg.Any<Server>())
                .Returns(ci => new ValidationResult {IsValid = true});

            serversRepo.AddNewServer(Arg.Any<Server>())
                .Throws(ci => new Exception("Simulating repository exception."));

            var error = await sut.AddNewServer(testServer).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.NotNull(error.Exception);
            Assert.Contains("Failed to add new server", error.Message);
        }

        [Fact]
        public async Task AddNewServer_ReturnsNull_WhenServerAddedSuccessfully()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var validator = autoSub.Resolve<IServerValidator>();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            validator.Validate(Arg.Any<Server>())
                .Returns(ci => new ValidationResult { IsValid = true });

            var error = await sut.AddNewServer(testServer).ConfigureAwait(false);

            Assert.Null(error);

            await serversRepo.Received().AddNewServer(Arg.Any<Server>());
        }

        [Fact]
        public async Task DeleteServer_ReturnsError_WhenRepositoryThrows()
        {
            var autoSub = GetAutoSub();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            serversRepo.DeleteServer(Arg.Any<long>())
                .Throws(ci => new Exception("Simulating repository exception."));

            var error = await sut.DeleteServer(1L).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.NotNull(error.Exception);
            Assert.Contains("Failed to delete server", error.Message);
        }

        [Fact]
        public async Task DeleteServer_ReturnsNull_WhenServerDeleted()
        {
            var autoSub = GetAutoSub();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            var error = await sut.DeleteServer(1L).ConfigureAwait(false);

            Assert.Null(error);

            await serversRepo.DeleteServer(Arg.Any<long>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task GetAllServers_ReturnsError_WhenRepositoryThrows()
        {
            var autoSub = GetAutoSub();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            serversRepo.GetAllServers()
                .Throws(ci => new Exception("Simulating repository exception."));

            var (result, error) = await sut.GetAllServers().ConfigureAwait(false);

            Assert.Null(result);
            Assert.NotNull(error);
            Assert.NotNull(error.Exception);
            Assert.Contains("Failed to retrieve all servers", error.Message);
        }

        [Fact]
        public async Task GetAllServers_ReturnsServersWithNoError_WhenGetAllServersSuccessful()
        {
            var testServers = new List<Server>
            {
                new Server(),
                new Server(),
                new Server(),
            };

            var autoSub = GetAutoSub();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            serversRepo.GetAllServers()
                .Returns(ci => testServers);

            var (result, error) = await sut.GetAllServers().ConfigureAwait(false);

            Assert.Null(error);
            Assert.NotNull(result);
            Assert.Same(testServers, result);
        }

        [Fact]
        public async Task UpdateServer_ReturnsError_WhenValidationFails()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var validator = autoSub.Resolve<IServerValidator>();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            validator.Validate(Arg.Any<Server>())
                .Returns(ci => new ValidationResult { IsValid = false });

            var error = await sut.UpdateServer(testServer).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Contains("did not pass validation", error.Message);

            await serversRepo.DidNotReceive().UpdateServer(Arg.Any<Server>());
        }

        [Fact]
        public async Task UpdateServer_ReturnsError_WhenRepositoryThrows()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var validator = autoSub.Resolve<IServerValidator>();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            validator.Validate(Arg.Any<Server>())
                .Returns(ci => new ValidationResult { IsValid = true });

            serversRepo.UpdateServer(Arg.Any<Server>())
                .Throws(ci => new Exception("Simulating repository error."));

            var error = await sut.UpdateServer(testServer).ConfigureAwait(false);

            Assert.NotNull(error);
            Assert.Contains("Failed to update server", error.Message);
        }

        [Fact]
        public async Task UpdateServer_ReturnsNull_WhenUpdateSuccessful()
        {
            var testServer = new Server();

            var autoSub = GetAutoSub();
            var validator = autoSub.Resolve<IServerValidator>();
            var serversRepo = autoSub.Resolve<IServersRepository>();
            var sut = autoSub.Resolve<ServersService>();

            validator.Validate(Arg.Any<Server>())
                .Returns(ci => new ValidationResult { IsValid = true });

            var error = await sut.UpdateServer(testServer).ConfigureAwait(false);

            Assert.Null(error);

            await serversRepo.Received().UpdateServer(Arg.Is(testServer));
        }

        private AutoSubstitute GetAutoSub()
        {
            ILogger<T> GetLogger<T>(IComponentContext ctx)
            {
                return ctx.Resolve<ILoggerFactory>().CreateLogger<T>();
            }

            var builder = new ContainerBuilder();
            builder.RegisterInstance(LoggerFactory);
            builder.Register(GetLogger<ServersService>)
                .As<ILogger<ServersService>>();

            var autoSub = new AutoSubstitute(builder);

            return autoSub;
        }
    }
}
