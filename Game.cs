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
            this.Card = new Card();
            this.Bus = new Bus(this.Card);
            this.Cpu = new Cpu(this.Bus);

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

        private Card Card { get; set; }

        private IBus Bus { get; set; }

        public bool Running { get; private set; }
        public bool Paused { get; private set; }

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
            this.Running = true;
            this.Paused = false;

            while (this.Running)
            {
                if(this.Paused)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if (!this.Cpu.Step())
                {
                    this.Exit(TypeExit.STOPED);
                }

                this.Cpu.Tickets += 1;

                //TODO...
                Thread.Sleep(1000);
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
