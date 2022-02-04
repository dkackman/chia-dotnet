namespace chia.dotnet
{
    public record KeyringStatus
    {
        public bool IsKeyringLocked { get; init; }
        public bool PassphreaseSupportEnabled { get; init; }
        public bool CanSavePassphrase { get; init; }
        public bool UserPassphraseIsSet { get; init; }
        public bool NeedsMigration { get; init; }
        public bool CanRemoveLegacyKeys { get; init; }
        public bool CanSetPassphraseHint { get; init; }
        public string PassphraseHint { get; init; } = string.Empty;
        public PassphraseRequirements PassphraseRequirements { get; init; } = new PassphraseRequirements();
    }
}
