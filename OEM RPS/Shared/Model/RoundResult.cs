using System;
using OEM_RPS.Shared.Enums;

namespace OEM_RPS.Shared.Model
{
    public class RoundResult: BaseEntity
    {
        public Position Player1Choice { get; set; }
        public Position Player2Choice { get; set; }
        public int Winner { get; set; }
    }
}

