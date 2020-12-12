using System;

namespace Prova
{
    public class MyClass
    {

        public String MyString { get; set; }
        public int MyInt { get; set; }

        public MyClass() { }

        public MyClass()
        {
            this.MyString = "mystring";
            this.MyInt = 2;
        }
    }
}
