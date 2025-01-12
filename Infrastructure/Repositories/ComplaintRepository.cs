using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(SahaflarPazari context) : base(context)
        {
        }

        public async Task<IEnumerable<Complaint>> GetComplaintsByUserIdAsync(string id)
        {
            return await _context.Complaints.Where(c => c.UserId == id).ToListAsync();
        }
    }
}
