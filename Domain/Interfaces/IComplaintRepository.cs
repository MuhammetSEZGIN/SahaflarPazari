using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IComplaintRepository : IRepository<Complaint>
    {
        Task<IEnumerable<Complaint>> GetComplaintsByUserIdAsync(string id);
    }
}
