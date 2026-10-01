/*
   Copyright 2026 Alexander Stärk

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

namespace Basilisque.Core.Auth;

/// <summary>
/// Represents a proxy for the <see cref="WritableUserContext{TKey}"/>.
/// </summary>
/// <remarks>
/// Multiple interfaces need to resolve to the same instance of <see cref="WritableUserContext{TKey}"/>.
/// This is not possible with open generic types in Microsoft.Extensions.DependencyInjection, because
/// it does not support open generic types with factories. Therefore, this proxy class is used to wrap
/// the <see cref="WritableUserContext{TKey}"/> and provide a single internal instance for multiple interfaces.
/// </remarks>
/// <typeparam name="TInterface">The type of the target interface.</typeparam>
/// <typeparam name="TKey">The type of the user key.</typeparam>
public abstract class BaseUserContextProxy<TInterface, TKey> : IWritableUserContext<TKey>
    where TInterface : IUserContext<TKey>
    where TKey : notnull
{
    /// <summary>
    /// The writable inner user context.
    /// </summary>
    protected readonly WritableUserContext<TKey> _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseUserContextProxy{TInterface, TKey}"/> class.
    /// </summary>
    /// <param name="userContext">The writable user context.</param>
    public BaseUserContextProxy(
        WritableUserContext<TKey> userContext
        )
    {
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    /// <inheritdoc />
    public TKey UserId
    {
        get => _userContext.UserId;
        set => _userContext.UserId = value;
    }

    /// <inheritdoc />
    public string? UserName
    {
        get => _userContext.UserName;
        set => _userContext.UserName = value;
    }

    /// <inheritdoc />
    public string? UserDisplayName
    {
        get => _userContext.UserDisplayName;
        set => _userContext.UserDisplayName = value;
    }

    /// <inheritdoc />
    public bool TryGetCurrentUserId(out TKey userId)
    {
        return _userContext.TryGetCurrentUserId(out userId);
    }
}