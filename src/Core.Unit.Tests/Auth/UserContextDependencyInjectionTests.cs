/*
    Copyright 2026

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Basilisque.Core.Auth;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Assertions;
using TUnit.Core;

namespace Basilisque.Core.Unit.Tests.Auth;

public class UserContextDependencyInjectionTests
{
    [Test]
    public async Task GenericUserContextInterfaces_ShareWritableUserContextStateWithinScope()
    {
        using var provider = createServiceProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var writableContext = services.GetRequiredService<IWritableUserContext<int>>();
        var readOnlyContext = services.GetRequiredService<IUserContext<int>>();
        var concreteContext = services.GetRequiredService<WritableUserContext<int>>();

        writableContext.UserId = 42;
        writableContext.UserName = "test-user";
        writableContext.UserDisplayName = "Test User";

        await Assert.That(readOnlyContext.UserId).IsEqualTo(42);
        await Assert.That(readOnlyContext.UserName).IsEqualTo("test-user");
        await Assert.That(readOnlyContext.UserDisplayName).IsEqualTo("Test User");
        await Assert.That(concreteContext.UserId).IsEqualTo(42);
        await Assert.That(concreteContext.UserName).IsEqualTo("test-user");
        await Assert.That(concreteContext.UserDisplayName).IsEqualTo("Test User");
    }

    [Test]
    public async Task GuidConvenienceInterfaces_ShareGenericGuidUserContextStateWithinScope()
    {
        using var provider = createServiceProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;
        var expectedUserId = Guid.NewGuid();

        var writableContext = services.GetRequiredService<IWritableUserContext>();
        var readOnlyContext = services.GetRequiredService<IUserContext>();
        var genericWritableContext = services.GetRequiredService<IWritableUserContext<Guid>>();
        var genericReadOnlyContext = services.GetRequiredService<IUserContext<Guid>>();
        var concreteContext = services.GetRequiredService<WritableUserContext<Guid>>();

        writableContext.UserId = expectedUserId;
        writableContext.UserName = "test-user";
        writableContext.UserDisplayName = "Test User";

        await Assert.That(readOnlyContext.UserId).IsEqualTo(expectedUserId);
        await Assert.That(genericWritableContext.UserId).IsEqualTo(expectedUserId);
        await Assert.That(genericReadOnlyContext.UserId).IsEqualTo(expectedUserId);
        await Assert.That(concreteContext.UserId).IsEqualTo(expectedUserId);
        await Assert.That(readOnlyContext.UserName).IsEqualTo("test-user");
        await Assert.That(genericWritableContext.UserName).IsEqualTo("test-user");
        await Assert.That(genericReadOnlyContext.UserName).IsEqualTo("test-user");
        await Assert.That(concreteContext.UserName).IsEqualTo("test-user");
        await Assert.That(readOnlyContext.UserDisplayName).IsEqualTo("Test User");
        await Assert.That(genericWritableContext.UserDisplayName).IsEqualTo("Test User");
        await Assert.That(genericReadOnlyContext.UserDisplayName).IsEqualTo("Test User");
        await Assert.That(concreteContext.UserDisplayName).IsEqualTo("Test User");
    }

    [Test]
    public async Task DifferentGenericUserContextTypes_DoNotShareStateWithinScope()
    {
        using var provider = createServiceProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var intContext = services.GetRequiredService<IWritableUserContext<int>>();
        var stringContext = services.GetRequiredService<IUserContext<string>>();

        intContext.UserId = 42;

        await Assert.That(stringContext.TryGetCurrentUserId(out _)).IsFalse();
    }

    [Test]
    public async Task UserContextState_IsScoped()
    {
        using var provider = createServiceProvider();
        var expectedUserId = Guid.NewGuid();

        using (var firstScope = provider.CreateScope())
        {
            firstScope.ServiceProvider.GetRequiredService<IWritableUserContext>().UserId = expectedUserId;
            await Assert.That(firstScope.ServiceProvider.GetRequiredService<IUserContext>().UserId).IsEqualTo(expectedUserId);
        }

        using var secondScope = provider.CreateScope();

        await Assert.That(secondScope.ServiceProvider.GetRequiredService<IUserContext>().TryGetCurrentUserId(out _)).IsFalse();
    }

    [Test]
    public async Task ResolvedProxyInstances_AreNotTheSharedStateInstance()
    {
        using var provider = createServiceProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var writableContext = services.GetRequiredService<IWritableUserContext<Guid>>();
        var readOnlyContext = services.GetRequiredService<IUserContext<Guid>>();
        var concreteContext = services.GetRequiredService<WritableUserContext<Guid>>();

        await Assert.That(writableContext).IsNotSameReferenceAs(concreteContext);
        await Assert.That(readOnlyContext).IsNotSameReferenceAs(concreteContext);
        await Assert.That(writableContext).IsNotSameReferenceAs(readOnlyContext);
    }

    private static ServiceProvider createServiceProvider()
    {
        var services = new ServiceCollection();
        services.RegisterServices();

        return services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        });
    }
}