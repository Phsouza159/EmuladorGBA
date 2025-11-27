using EmuladorGBA.Business.Config;
using EmuladorGBA.Business.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business
{
    public class Card
    {
        public TypeRom TypeRom { get; private set; }

        public string PathRom { get; private set; }

        public byte[] RomData { get; private set; }

        #region GET / SET

        internal void SetPathRom(string pathRom)
        {
            this.PathRom = pathRom;
        }

        internal void SetTypeRom(TypeRom typeRom)
        {
            this.TypeRom = TypeRom;
        }

        #endregion


        #region LOAD ROM

        public bool LoadRom()
        {
            if(this.TypeRom  == TypeRom.PATH)
            {
                return this.LoadRomFromPath();
            }

            return false;
        }

        private bool LoadRomFromPath()
        {
            if (string.IsNullOrEmpty(this.PathRom))
                throw new ArgumentException("Informe um PATH para a ROM");

            if (!File.Exists(this.PathRom))
                throw new FileNotFoundException($"Arquivo de ROM não localizado em: '{this.PathRom}'");

            this.RomData = File.ReadAllBytes(this.PathRom);
            return this.RomData != null && this.RomData.Length > 0;
        }

        #endregion

        #region MEMORY HEAD

        private byte[] EntryPoint { get; set; }

        private byte[] NitendoLog { get; set; }

        private byte[] Title { get; set; }

        private byte[] ManufacturerCode { get; set; }

        private byte[] CGB_Flag { get; set; }

        private byte[] LicenseeCode { get; set; }

        private byte[] SGB_Flag { get; set; }

        private byte[] CartridgeType { get; set; }

        private byte RomSize { get; set; }

        private byte RamSize { get; set; }

        private byte DestinationCode { get; set; }

        private bool IsHeadLoad { get; set; }

        #endregion

        #region LOAD HEAD

        public void LoadHead()
        {
            this.LoadTitle();
            this.LoadEntyPoint();
            this.LoadNitendoLogo();
            this.LoadManufacturerCode();
            this.LoadCGB_Flag();
            this.LoadLicenseeCode();
            this.LoadSGB_Flag();
            this.LoadCartridgeType();
            this.LoadRomSize();
            this.LoadRamSize();
            this.LoadDestinationCode();

            byte checksum = 0;
            byte checkValue = this.RomData[MemoryConfig.MEMORY_CHECK_VALUE];
            for (int address = MemoryConfig.MEMORY_CHECK_HEAD_INIT; address <= MemoryConfig.MEMORY_CHECK_HEAD_END; address++)
            {
                checksum = (byte)(checksum - this.RomData[address] - 1);
            }

            this.IsHeadLoad = checksum == checkValue;
        }

        public void LoadTitle()
        {
            this.Title = this.LoadData(MemoryConfig.MEMORY_NAME_ROM_INIT, MemoryConfig.MEMORY_NAME_ROM_END);
        }

        public void LoadEntyPoint()
        {
            this.EntryPoint = this.LoadData(MemoryConfig.MEMORY_ENTRY_POINT_INIT, MemoryConfig.MEMORY_ENTRY_POINT_END);
        }

        public void LoadNitendoLogo()
        {
            this.EntryPoint = this.LoadData(MemoryConfig.MEMORY_NITENDO_LOGO_INIT, MemoryConfig.MEMORY_NITENDO_LOGO_END);
        }

        public void LoadManufacturerCode()
        {
            this.ManufacturerCode = this.LoadData(MemoryConfig.MEMORY_MANUFACTURER_CODE_INIT, MemoryConfig.MEMORY_MANUFACTURER_CODE_END);
        }

        public void LoadCGB_Flag()
        {
            this.CGB_Flag = this.LoadData(MemoryConfig.MEMORY_CGB_FLAG_INIT, MemoryConfig.MEMORY_CGB_FLAG_END);
        }

        public void LoadLicenseeCode()
        {
            this.LicenseeCode = this.LoadData(MemoryConfig.MEMORY_LICENSEE_CODE_INIT, MemoryConfig.MEMORY_LICENSEE_CODE_END);
        }

        public void LoadSGB_Flag()
        {
            this.SGB_Flag = this.LoadData(MemoryConfig.MEMORY_SGB_FLAG_INIT, MemoryConfig.MEMORY_SGB_FLAG_END);
        }

        public void LoadCartridgeType()
        {
            this.CartridgeType = this.LoadData(MemoryConfig.MEMORY_CARTRIDGE_TYPE_INIT, MemoryConfig.MEMORY_CARTRIDGE_TYPE_END);
        }

        public void LoadRomSize()
        {
            this.RomSize = this.LoadData(MemoryConfig.MEMORY_ROM_SIZE_INIT);
        }

        public void LoadRamSize()
        {
            this.RamSize = this.LoadData(MemoryConfig.MEMORY_RAM_SIZE_INIT);
        }

        public void LoadDestinationCode()
        {
            this.DestinationCode = this.LoadData(MemoryConfig.MEMORY_DESTINAITON_INIT);
        }

        private byte LoadData(decimal init)
        {
            if (init < 1)
                throw new ArgumentException("Valor inválido para tamanho de memorio.");

           return this.RomData[(int)init];
        }

        private byte[] LoadData(decimal init, decimal end)
        {
            decimal length = end - init;

            if (length < 1)
                throw new ArgumentException("Valor inválido para tamanho de memorio.");

            byte[] data = new byte[(int)length];
            Array.Copy(this.RomData, (int)init, data, 0, (int)length);
            return data;
        }

        #endregion
    }
}
