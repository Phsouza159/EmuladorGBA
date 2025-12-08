using EmuladorGBA.Business;
using EmuladorGBA.Business.Config;
using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Memory;
using EmuladorGBA.Business.Process;
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
            MemoryMap memoryMap = new MemoryMap
            {
                WRAM_Length = MemoryConfig.MEMORY_WRAM_LENGTH,
                HRAM_Length = MemoryConfig.MEMORY_HRAM_LENGTH
            };

            this.Cpu = Cpu.Create();
            this.Card = new Card();
            this.Cpu.Bus = new Bus(this.Card, new RamMemory(memoryMap), this.Cpu);
        }

        private Cpu Cpu { get; set; }
        private Card Card { get; set; }

        public bool Running { get; private set; }
        public bool Paused { get; private set; }

        public decimal? MaxTicketBreak { get; set; }

        internal void LoadRomFromPath(string pathRom)
        {
            this.Card.SetPathRom(pathRom);
            this.Card.SetTypeRom(TypeRom.PATH);
            this.Card.LoadRom();

            this.LoadHead();
            this.ShowHeadValues();
        }

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
            int sleepTime = 10;
            this.Running = true;
            this.Paused = false;
            this.MaxTicketBreak = 0x0050;

            Console.WriteLine("--------------- RUNNING....");

            while (this.Running)
            {
                if(this.Paused)
                {
                    Thread.Sleep(sleepTime);
                    continue;
                }

                if (!this.Cpu.Step())
                {
                    this.Exit(TypeExit.STOPED);
                }

                this.Cpu.Tickets += 1;

                //TODO...
                Thread.Sleep(sleepTime);

                if (this.MaxTicketBreak.HasValue && this.Cpu.Tickets > this.MaxTicketBreak)
                    break;

                // TEST LIMIT CPU TICKETS
                //if (this.Cpu.Tickets == 50) break;
            }
        }

        private void Exit(TypeExit type)
        {
            //TODO...
            Console.WriteLine($"EXIT CODE {(short)type} - {type}");
            throw new ArgumentException("EXIT");
        }

    
    }
}
