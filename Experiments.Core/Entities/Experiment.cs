using System;
using System.Collections.Generic;
using Domain.Common;

namespace Experiments.Core.Entities
{
    public class Experiment : AggregateRoot
    {
        public string Creator { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public List<long> MethodIds { get; set; }

        public Experiment(string creator, string name)
        {
            Creator = creator;
            Name = name;
            CreationDate = DateTime.Now;
            MethodIds = new List<long>();
        }

        public void AddMethod(long id)
        {
            if (MethodIds == null || id < 1)
                return;

            MethodIds.Add(id);
        }

        public void RemoveMethod(long id)
        {
            if (MethodIds == null || id < 1)
                return;

            MethodIds.Remove(id);
        }

    }
}
