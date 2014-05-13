using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using TeachMe;
using TeachMe.Models;
using System.Linq;
using TeachMe.Controllers;

namespace TeachMe
{
    [TestClass]
    public class RadiusUnitTest
    {
        private SearchController Controller = new SearchController();

        private MapSearchViewModel model = new MapSearchViewModel();

        public MapSearchViewModel Model()
        {
            model.MapCenter = new GeoLocation();
            model.MapCenter.Latitude = "32.134360";
            model.MapCenter.Longitude = "34.796590";
            model.MapZoom = 12;
            return model;
        }

        private List<Teacher> teachers = new List<Teacher>();

        public List<Teacher> Teachers()
        {
            string teachersTemp = "מיכל דורון 34 תל-אביב 32.127932 34.800555 מדעי-המחשב 100 050-1111111,יוחאי אלון 30 תל-אביב 32.120897 34.798193 מדעי-המחשב 90 050-2222222,הגר אברמסון 27 תל-אביב 32.105589 34.794193 מדעי-המחשב 80 050-3333333,אופיר קורקוס 28 תל-אביב 32.095749 34.796127 מתמטיקה 110 050-4444444,ארנסטו זילברשטיין 50 תל-אביב 32.090869 34.784465 מתמטיקה 100 050-5555555,נדב זורנו 27 תל-אביב 32.081761 34.783920 מדעי-המחשב 100 050-6666666,יאיר לביא 39 תל-אביב 32.072597 34.784089 מדעי-המחשב 110 050-7777777,אייל מור 31 ירושלים 31.773368 35.219993 מוזיקה 90 050-1231231";

            var list = teachersTemp.Replace("  ", " ").Split(',').ToList();
            int id = 0;
            foreach (var t in list)
            {
                var temp = t.Split(' ');
                var teacher = new Teacher();
                teacher.User = new ApplicationUser();
                teacher.User.FirstName = temp[0];
                teacher.User.LastName = temp[1];
                teacher.Age = int.Parse(temp[2]);
                teacher.City = temp[3];
                teacher.GeoLocation = new GeoLocation();
                teacher.GeoLocation.Latitude = temp[4];
                teacher.GeoLocation.Longitude = temp[5];
                teacher.Category = temp[6];
                teacher.LessonPrice = int.Parse(temp[7]);
                teacher.Phone = temp[8];
                teacher.Id = id++;
                teachers.Add(teacher);
            }

            return teachers;
        }

        private List<Teacher> radius(List<Teacher> teachers, MapSearchViewModel model, string distance)
        {
            return Controller.SearchInRadius(teachers, model, distance);
        }

        [TestMethod]
        public void RadiusOneReturnsOneResults()
        {
            string distance = "1";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusTwoReturnsTwoResults()
        {
            string distance = "2";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusThreeReturnsThreeResults()
        {
            string distance = "3";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusFourReturnsFourResults()
        {
            string distance = "4";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusFiveReturnsFiveResults()
        {
            string distance = "5";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusSixReturnsSixResults()
        {
            string distance = "6";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusSevenReturnsSevenResults()
        {
            string distance = "7";
            Assert.AreEqual(System.Convert.ToSingle(distance), radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusEightReturnsSevenResults()
        {
            string distance = "8";
            Assert.AreEqual(System.Convert.ToSingle(distance)-1, radius(Teachers(), Model(), distance).Count);
        }

        [TestMethod]
        public void RadiusSixtyReturnsEightResults()
        {
            string distance = "60";
            Assert.AreEqual(System.Convert.ToSingle(distance)-52, radius(Teachers(), Model(), distance).Count);
        }
    }
}
