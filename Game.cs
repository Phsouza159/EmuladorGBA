using EmuladorGBA.Business;
using EmuladorGBA.Business.Config;
using EmuladorGBA.Business.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA
{
    public class Game
    {
        public Game()
        {
            this.Cpu = new Cpu();
            this.StartMemomy();
        }

        private void StartMemomy()
        {
            MemoryMap memoryMap = new MemoryMap
            {
                Length = MemoryConfig.MEMORY_LENGTH
            };

            this.Memory = new RamMemory(memoryMap);
        }

        private RamMemory Memory { get; set; }

        private Cpu Cpu { get; set; }

        public Card Card { get; set; }

        public string Title { get; set; }

        public void LoadHead()
        {
            this.Card.LoadHead();
        }

        public void ShowHeadValues()
        {
            this.Card.ShowHeadValues();
        }

        internal void Start()
        {
            this.Cpu.Init();
        }
    }
}
