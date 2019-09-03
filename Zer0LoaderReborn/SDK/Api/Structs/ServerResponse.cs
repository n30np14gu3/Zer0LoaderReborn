using System;

namespace Zer0LoaderReborn.SDK.Api.Structs
{
    [Serializable]
    internal class ServerResponse<T>
    {
        public ServerCodes code;
        public T data;
    }
}