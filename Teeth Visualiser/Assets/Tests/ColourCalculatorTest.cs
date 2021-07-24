using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ColourCalculatorTest
    {
        List<Color> colours;
        float from;
        float to;

        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void Dispose()
        {
            colours = null;
            from = -1;
            to = -1;
        }

        public void setUpGoodData()
        {
            colours = new List<Color>();
            colours.Add(Color.red);
            colours.Add(Color.blue);
            colours.Add(Color.green);

            from = 1;
            to = 13;

        }

        public void noColoursData() {
            colours = new List<Color>();
            from = 1;
            to = 13;
        }
        public void coloursNoData() {
            colours = new List<Color>();
            colours.Add(Color.red);
            colours.Add(Color.blue);
            colours.Add(Color.green);
        }

        public void noData() {
            colours = null;
            from = -1;
            to = -1;
        }

        [Test]
        public void colourCalculatorThrowsExceptionWhenNodata()
        {
            noData();
            var ex = Assert.Throws<Exception>(() => ColourCalculator.CalculateColourScale(from,to,colours));
            Assert.That(ex.Message, Is.EqualTo("No radiation values selected"));
        }

        [Test]
        public void colourCalculatorThrowsExceptionWhencoloursNoData()
        {
            noData();
            coloursNoData();
            var ex = Assert.Throws<Exception>(() => ColourCalculator.CalculateColourScale(from, to, colours));
            Assert.That(ex.Message, Is.EqualTo("No radiation values selected"));
        }

        [Test]
        public void colourCalculatorThrowsExceptionWhenDataNoColours()
        {
            noData();
            noColoursData();
            var ex = Assert.Throws<Exception>(() => ColourCalculator.CalculateColourScale(from, to, colours));
            Assert.That(ex.Message, Is.EqualTo("No colour's selected"));
        }

        [Test]
        public void colourCalculatorProducesListAsExpected()
        {
            noData();
            setUpGoodData();
            Assert.AreEqual(13, ColourCalculator.CalculateColourScale(from, to, colours).Count);
        }


    }
}
