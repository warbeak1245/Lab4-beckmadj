using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;
using System.Reflection;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}


        [Test()]
        public void TestThatCarGetsMileageFromDatabase()
        {
            int targetMiles = 250000;
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            mockDatabase.Miles = targetMiles;
            var target = new Car(10);
            target.Database = mockDatabase;
            int actualMiles = target.Mileage;
            Assert.AreEqual(actualMiles, targetMiles);
        }

        [Test()]
        public void TestThatLocationGetsFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();

            String carLocation = "Earth";

            using (mocks.Record())
            {
                mockDatabase.getCarLocation(23);
                LastCall.Return(carLocation);
            }

            var target = ObjectMother.BMW();
            target.Database = mockDatabase;
            String result;
            result = target.getCarLocation(23);
            Assert.AreEqual(result, carLocation);
        }
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}
	}
}
