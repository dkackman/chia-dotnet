﻿using System.Collections.Generic;

namespace chia.dotnet
{
    public record MempoolItem
    {
        public SpendBundle SpendBundle { get; init; } = new();
        public ulong Fee { get; init; }
        public NPCResult NPCResult { get; init; } = new();
        public ulong Cost { get; init; }
        public string SpendBudndleName { get; init; } = string.Empty;
        public IEnumerable<Coin> Additions { get; init; } = new List<Coin>();
        public IEnumerable<Coin> Removals { get; init; } = new List<Coin>();
        public string Program { get; init; } = string.Empty;
    }
}
