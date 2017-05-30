/*
 *  Copyright 2013 Feifan Tang. All rights reserved.
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Owin.Security.QQ
{
    public class QQAuthenticationProvider : IQQAuthenticationProvider
    {
        public QQAuthenticationProvider()
        {
            onAuthenticated = (c) => Task.FromResult<QQAuthenticatedContext>(null);
            onReturnEndpoint = (c) => Task.FromResult<QQReturnEndpointContext>(null);
        }

        public Func<QQAuthenticatedContext, Task> onAuthenticated { get; set; }
        public Func<QQReturnEndpointContext, Task> onReturnEndpoint { get; set; }

        public Task Authenticated(QQAuthenticatedContext context)
        {
            return onAuthenticated(context);
        }

        public Task ReturnEndpoint(QQReturnEndpointContext context)
        {
            return onReturnEndpoint(context);
        }
    }
}
