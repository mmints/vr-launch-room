using System.Collections.Generic;
using UnityEngine;

namespace Moodle
{ 
    public class MoodleUser : MonoBehaviour
    {
        public int id;
        public string username;
        public string firstName;
        public string lastName;
        public string fullName;
        public string email;
        public Texture2D profileImage;

        public List<Course> enrolledCourses = new List<Course>();
    }
    
    public class Course
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Summary { get; set; }

        public List<Activity> activities = new List<Activity>();
    }

    public class Activity
    {
        public int ID { get; set; }
        public int Instance { get; set; }
        public string ModName { get; set; }
        public string ModPlural { get; set; }
        public string Name { get; set; }
    }
}