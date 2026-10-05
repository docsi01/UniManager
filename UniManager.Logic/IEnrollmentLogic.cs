using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Models;

namespace UniManager.Logic
{
    public interface IEnrollmentLogic
    {
        IEnumerable<Enrollment> ReadAll();
        void Create(Enrollment enrollment);
    }
}
