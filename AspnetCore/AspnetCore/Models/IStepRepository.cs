using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspnetCore.Models
{
    interface IStepRepository
    {
        IEnumerable<StepQueue> GetAll();
        StepQueue Get(int id);
        StepQueue Add(StepQueue item);
        void Remove(int id);
        bool Update(StepQueue item);
    }
}
