using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language;
using System.Reactive.Linq;
using Integra.Vision.Event;
using Microsoft.Reactive.Testing;
using System.Reactive;
using Integra.Vision.Language.Runtime;
using System.Collections.Generic;
using System.Dynamic;
using Integra.Vision.Language.Exceptions;

namespace Integra.Space.LanguageUnitTests.Queries
{
	[TestClass]
	public class QueryTests : ReactiveTest
	{
		private PlanNode Parse(string statement)
		{
			return new EQLPublicParser(statement).Parse().First();
		}

		private Func<IQbservable<EventObject>, IObservable<object>> Build(PlanNode node)
		{
			ObservableConstructor te = new ObservableConstructor();
			return te.Compile<IQbservable<EventObject>, IObservable<object>>(node);
		}


		[TestMethod]
		public void Query0()
		{
			//var node = Parse(string.Format(@"from a apply window of '00:00:10' select @event.Message.#0.#0 as campo1"));
			var node = Parse(string.Format(@"from a apply window of '00:00:05' group by @event.Message.#0.#0 as grupo1 select key.grupo1 as campo1")); 

			var result = Build(node);

			System.Reactive.Subjects.ReplaySubject<EventObject> s = new System.Reactive.Subjects.ReplaySubject<EventObject>();

			var o = result(s.AsQbservable());

			o.Subscribe(r =>
			{

				var x = r;
			});

			s.OnNext(TestObjects.EventObjectTest);
			s.OnNext(TestObjects.EventObjectTest);
			s.OnNext(TestObjects.EventObjectTest);

			o.Wait();
        }


		[TestMethod]
		public void Query1()
		{
			var node = Parse(
					string.Format(@"from a where a.a == 1 select {0} as CampoNulo",
																"@event.Message.#0.[\"Campo que no existe\"]"));

			var result = Build(node);

			TestScheduler scheduler = new TestScheduler();

			ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
				OnNext(100, TestObjects.EventObjectTest),
				OnCompleted<EventObject>(200)
			);

			ITestableObserver<object> results = scheduler.Start(
				() =>
					result(input.AsQbservable())
					.Select(x =>
						(object)(new
						{
							CampoNulo = x.GetType().GetProperty("CampoNulo").GetValue(x)
						})
				),
				created: 10,
				subscribed: 50,
				disposed: 400);

			ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
				OnNext(100, (object)new { CampoNulo = default(object) }),
				OnCompleted<object>(200)
			});

			ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
					new Subscription(50, 200)
				});
		}
	}
}
