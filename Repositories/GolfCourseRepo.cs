using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolfCourseRepo
    {
        private readonly List<GolfCourse> _courseDbContext = new List<GolfCourse>();
        private readonly List<Hole> _holeDbContext = new List<Hole>();
        private int _count;

        public bool AddCourseToDatabase(GolfCourse course)
        {
            if (course == null)
            {
                return false;
            }
            else
            {
                _count++;
                _courseDbContext.Add(course);
                return true;
            }
        }

        public List<GolfCourse> GetCourses()
        {
            return _courseDbContext;
        }

        public GolfCourse GetCourseByName(string courseName)
        {
            foreach (var course in _courseDbContext)
            {
                if (course.CourseName == courseName)
                {
                    return course;
                }
            }
            return null;
        }

        public GolfCourse GetCourseById(int id)
        {
            foreach (var course in _courseDbContext)
            {
                if (course.Id == id)
                {
                    return course;
                }
            }
            return null;
        }
        public bool RemoveCourse(string courseName)
        {
            foreach (var course in _courseDbContext)
            {
                if (course.CourseName == courseName)
                {
                    _courseDbContext.Remove(course);
                    return true;
                }
            }
              return false;
        }

        public bool AddHoleToDatabase(Hole hole)
        {
            if (hole == null)
            {
                return false;
            }
            else
            {
                _count++;
                _holeDbContext.Add(hole);
                return true;
            }
        }

        public bool AssignHole(int id, Hole hole)
        {
            var course = GetCourseById(id);
            if (course != null)
            {
                course.HoleList.Add(hole);
                return true;
            }
            else return false;
        }
    }
}
