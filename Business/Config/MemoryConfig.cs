using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Config
{
    internal static class MemoryConfig
    {
        public static short MEMORY_WRAM_LENGTH = 0x2000;

        public static short MEMORY_HRAM_LENGTH = 0x80;

        #region ENTRY POINT

        public static decimal MEMORY_ENTRY_POINT_INIT = 0x100;

        public static decimal MEMORY_ENTRY_POINT_END = 0x103;

        #endregion

        #region NITENDO LOGO

        public static decimal MEMORY_NITENDO_LOGO_INIT = 0x104;

        public static decimal MEMORY_NITENDO_LOGO_END = 0x133;

        #endregion

        #region TITLE

        public static decimal MEMORY_NAME_ROM_INIT = 0x134;

        public static decimal MEMORY_NAME_ROM_END = 0x143;

        #endregion

        #region MANUFACTURER CODE

        public static decimal MEMORY_MANUFACTURER_CODE_INIT = 0x13F;

        public static decimal MEMORY_MANUFACTURER_CODE_END = 0x142;

        #endregion

        #region CGB FLAG

        public static decimal MEMORY_CGB_FLAG_INIT = 0x143;

        public static decimal MEMORY_CGB_FLAG_END = 0x144;

        #endregion

        #region LICENSEE CODE

        public static decimal MEMORY_LICENSEE_CODE_INIT = 0x144;

        public static decimal MEMORY_LICENSEE_CODE_END = 0x146;

        #endregion

        #region CGB FLAG

        public static decimal MEMORY_SGB_FLAG_INIT = 0x146;

        public static decimal MEMORY_SGB_FLAG_END = 0x147;

        #endregion

        #region CARTRIDGE TYPE

        public static decimal MEMORY_CARTRIDGE_TYPE_INIT = 0x147;

        #endregion

        #region ROM SIZE

        public static decimal MEMORY_ROM_SIZE_INIT = 0x148;

        #endregion

        #region RAM SIZE

        public static decimal MEMORY_RAM_SIZE_INIT = 0x149;

        #endregion

        #region DESTINATION CODE

        public static decimal MEMORY_DESTINAITON_INIT = 0x14A;

        #endregion

        #region ROM VERSION

        public static decimal MEMORY_ROM_VERSION_INIT = 0x14C;

        #endregion

        #region CHECK VALUE

        public static int MEMORY_CHECK_VALUE = 0x14D;

        public static int MEMORY_CHECK_HEAD_INIT = 0x134;

        public static int MEMORY_CHECK_HEAD_END = 0x14C;

        #endregion
    }
}
