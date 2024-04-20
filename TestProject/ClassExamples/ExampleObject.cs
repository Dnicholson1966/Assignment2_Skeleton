using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ClassExamples
{
    internal class ExampleObject
    {
        private string _fullName;
        private int _age;

        public string Name { get { return _fullName; } }
        public int Age { get { return _age; } }

        public ExampleObject(string name, int age)
        {
            _fullName = name;
            _age = age;
        }

        public string toString()
        {
            return $"{Name},{Age}";
        }
    }
}
