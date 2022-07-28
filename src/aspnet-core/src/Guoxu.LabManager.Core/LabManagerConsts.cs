using Guoxu.LabManager.Debugging;

namespace Guoxu.LabManager
{
    public class LabManagerConsts
    {
        public const string LocalizationSourceName = "LabManager";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "78062e62debb46a0945283a071d5d15b";

        public const string MustHaveWarehouseTypeFilterName = "MustHaveWarehouseType";
    }
}
