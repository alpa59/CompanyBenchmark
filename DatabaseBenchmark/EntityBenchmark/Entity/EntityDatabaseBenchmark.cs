using Benchmarking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EntityBenchmark.Entity {
    public class EntityDatabaseBenchmark : IDatabaseBenchmark {
        private readonly EntityContext _context;

        public EntityDatabaseBenchmark(EntityContext context) {
            _context = context;
        }

        // Insert Parent with Children
        public void InsertParentWithChildren(Parent parent) {
            _context.Parents.Add(parent);
            _context.SaveChanges();
        }

        // Get all Parents with their Children
        public List<Parent> GetParentsWithChildren() {
            return _context.Parents.Include(p => p.Children).ToList();
        }

        public Parent GetLatestParent() {
            return _context.Parents.Include(p => p.Children).OrderByDescending(p => p.Id).FirstOrDefault();
        }


        // Update Parent and all its Children
        public void UpdateParentWithChildren(Parent parent) {
            var existingParent = _context.Parents.Include(p => p.Children).FirstOrDefault(p => p.Id == parent.Id);
            if (existingParent != null) {
                _context.Entry(existingParent).CurrentValues.SetValues(parent);

                // Update children
                foreach (var child in parent.Children) {
                    var existingChild = existingParent.Children.FirstOrDefault(c => c.Id == child.Id);
                    if (existingChild != null) {
                        _context.Entry(existingChild).CurrentValues.SetValues(child);
                    } else {
                        existingParent.Children.Add(child);
                    }
                }

                _context.SaveChanges();
            }
        }

        // Delete Parent and all its Children
        public void DeleteParentWithChildren(int parentId) {
            var parent = _context.Parents.Include(p => p.Children).FirstOrDefault(p => p.Id == parentId);
            if (parent != null) {
                _context.Parents.Remove(parent);
                _context.SaveChanges();
            }
        }
    }
}
