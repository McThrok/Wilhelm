using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend;
using Wilhelm.DataAccess;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Wilhelm.UnitTests
{
    class BackendTests
    {
        //[Test]
        //public void Add_AddPositive_Returns_5()
        //{
        //    Class1 c = new Class1();
        //    int expectedResult = 5;
        //    int result = c.Add(2, 3);
        //    Assert.AreEqual(expectedResult, result);
        //}

        //[TestCase(5,1,4)]
        //[TestCase(7,3,4)]
        //public void Add_AddPositive_ReturnsNumber(int expected, int a, int b)
        //{
        //    Class1 c = new Class1();
        //    int result = c.Add(a,b);
        //    Assert.AreEqual(expected, result);
        //}


        [Test]
        public void DataAccessIntegrationTest()
        {
            var DAI = new DataAccessIntegration(new FakeWContextFactory());
            var task = new WTask();
            DAI.SaveTask(task);
            Assert.AreNotEqual(task.Id, 0);
        }


        public class FakeWContextFactory : IWContextFactory
        {
            public IWContext Create()
            {
                return new FakeWContext();
            }
        }

        public class FakeWContext : IWContext
        {
            public FakeWContext()
            {
                WTasks = new FakeDbSet<WTask>();
            }

            public IDbSet<WTask> WTasks { get; set; }
            public IDbSet<WGroup> WGroups { get; set; }
            public DbSet<WActivity> WActivities { get; set; }
            public void Dispose()
            {
            }

            public int SaveChanges()
            {
                foreach (var task in WTasks)
                {
                    if(task.Id==0)
                        task.Id =  123456789;
                }

                return 0;
            }
        }

        public class FakeDbSet<TEntity> : IDbSet<TEntity> where TEntity : class
        {
            private ObservableCollection<TEntity> _local;
            public FakeDbSet()
            {
                _local = new ObservableCollection<TEntity>();
            }
            public ObservableCollection<TEntity> Local
            {
                get
                {
                    return _local;
                }
            }

            public Expression Expression => throw new NotImplementedException();

            public Type ElementType => throw new NotImplementedException();

            public IQueryProvider Provider => throw new NotImplementedException();

            public TEntity Add(TEntity entity)
            {
                Local.Add(entity);
                return entity;
            }

            public TEntity Attach(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public TEntity Create()
            {
                throw new NotImplementedException();
            }

            public TEntity Find(params object[] keyValues)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TEntity> GetEnumerator()
            {
                foreach (var item in _local)
                {
                    yield return item;
                }
            }

            public TEntity Remove(TEntity entity)
            {
                throw new NotImplementedException();
            }

            TDerivedEntity IDbSet<TEntity>.Create<TDerivedEntity>()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
