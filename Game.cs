using EmuladorGBA.Business;
using EmuladorGBA.Business.Config;
using EmuladorGBA.Business.Enum;
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

        public bool Running { get; private set; }
        public bool Paused { get; private set; }
        public int Tickets { get; private set; }

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
            this.Running = true;
            this.Paused = false;
            this.Tickets = 0;

            while (this.Running)
            {
                if(this.Paused)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if(!this.Cpu.Step())
                {
                    this.Exit(TypeExit.STOPED);
                }

                this.Tickets += 1;

                //TODO...
                Thread.Sleep(1000);
                Console.WriteLine($"Chuck: {this.Tickets}");
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
