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
[RegisterServiceScoped(As = typeof(IWritableUserContext<>), ImplementsITypeName = false)]
public sealed class WritableUserContextProxy<TKey> : BaseUserContextProxy<IWritableUserContext<TKey>, TKey>, IWritableUserContext<TKey>
    where TKey : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WritableUserContextProxy{TKey}"/> class.
    /// </summary>
    /// <param name="userContext">The writable user context.</param>
    public WritableUserContextProxy(
        WritableUserContext<TKey> userContext
        )
    : base(userContext)
    { }
}