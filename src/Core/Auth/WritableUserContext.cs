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
/// Represents the writable context of the current user.
/// </summary>
/// <typeparam name="TKey">The type of the user key.</typeparam>
[RegisterServiceScoped(ImplementsITypeName = false)]
public class WritableUserContext<TKey> : IWritableUserContext<TKey>
    where TKey : notnull
{
    private bool _hasUserId = false;
    private TKey _userId = default!;

    /// <inheritdoc />
    public TKey UserId
    {
        get
        {
            if (!TryGetCurrentUserId(out var userId))
                throw new InvalidOperationException("UserId has not been set.");

            return userId;
        }
        set
        {
            _userId = value;
            _hasUserId = !IsEmpty(value);
        }
    }

    /// <inheritdoc />
    public string? UserName { get; set; }

    /// <inheritdoc />
    public string? UserDisplayName { get; set; }

    /// <inheritdoc />
    public bool TryGetCurrentUserId(out TKey userId)
    {
        userId = _userId;

        if (_hasUserId)
            return true;

        return false;
    }

    /// <summary>
    /// Determines whether the specified value is considered empty for the type TKey.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is considered empty; otherwise, false.</returns>
    protected virtual bool IsEmpty(TKey value)
    {
        return value switch
        {
            null => true,
            Guid g => g == Guid.Empty,
            string s => string.IsNullOrWhiteSpace(s),
            _ => false
        };
    }
}