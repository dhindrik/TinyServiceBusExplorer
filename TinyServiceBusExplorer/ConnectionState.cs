﻿using System;
namespace TinyServiceBusExplorer
{
    public static class ConnectionState
    {
        public static string? ConnectionString { get; set; }
        public static string? CurrentQueue { get; set; }
    }
}
