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
/// Represents the read-only context of the current user.
/// </summary>
/// <typeparam name="TKey">The type of the user key.</typeparam>
public interface IUserContext<TKey>
    where TKey : notnull
{
    /// <summary>
    /// Gets the key of the current user.
    /// </summary>
    TKey UserId { get; }

    /// <summary>
    /// Gets the username of the current user, if available.
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Gets the display name of the current user, if available.
    /// </summary>
    string? UserDisplayName { get; }

    /// <summary>
    /// Attempts to get the key of the current user.
    /// </summary>
    /// <param name="userId">The current user key when this method returns <see langword="true"/>.</param>
    /// <returns><see langword="true"/> when a current user key is available; otherwise, <see langword="false"/>.</returns>
    bool TryGetCurrentUserId(out TKey userId);
}