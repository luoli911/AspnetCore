using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class StepRepository : IStepRepository
    {
        private List<StepQueue> steps = new List<StepQueue>();
        private int _nextId = 1;
        public StepQueue Add(StepQueue item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("step");
            }
            item.QueueId = _nextId++;
            steps.Add(item);
            return item;
        }

        public StepQueue Get(int id)
        {
            return steps.Find(p => p.StepFile.Id == id);
        }

        public IEnumerable<StepQueue> GetAll()
        {
            return steps;
        }

        public void Remove(int id)
        {
            steps.RemoveAll(p => p.QueueId == id);
        }

        public bool Update(StepQueue item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = steps.FindIndex(p => p.QueueId == item.QueueId);
            if(index == -1)
            {
                return false;
            }
            steps.RemoveAt(index);
            steps.Add(item);
            return true;
        }
    }
}