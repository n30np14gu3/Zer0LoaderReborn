using System;
using Zer0LoaderReborn.SDK.Api.Structs;

namespace Zer0LoaderReborn
{
    internal class ClientData
    {
        public const int GameId = 1;
        public static readonly DateTime LastUpdate = new DateTime(2029, 7, 23, 3, 0, 0);
        public static string AppDomain = " http://localhost:8000";
        public static bool Logged = false;
        public static UserData Data = null;
    }
}