using System;

namespace Test.Bernd.Models.Domain
{
    public class ParkingLot
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public int Capacity { get; private set; }

        public ParkingLot(string id, string name, int capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }

        private ParkingLot()
        {
        }
    }
}