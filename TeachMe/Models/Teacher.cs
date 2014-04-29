using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        string street;
        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        int homeNum;
        public int HomeNum
        {
            get { return homeNum; }
            set { homeNum = value; }
        }

        string category;
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        int price;
        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        string _latitude;
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        string _lognitude;
        public string Lognitude
        {
            get { return _lognitude; }
            set { _lognitude = value; }
        }

    }
}