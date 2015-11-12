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
    public class UserQueryNodeTests
    {
        [TestMethod]
        public void ConsultaProyeccionCampoNuloConWhere()
        {
            EQLPublicParser parser = new EQLPublicParser(
                    string.Format("from {0} where {1} select {2} as CampoNulo",
                                                                "SpaceObservable1",
                                                                "@event.Message.#0.MessageType == \"0100\"",
                                                                "@event.Message.#0.[\"Campo que no existe\"]")
                                                                );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        CampoNulo = x.First().GetType().GetProperty("CampoNulo").GetValue(x.First())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(100, Notification.CreateOnNext((object)(new { CampoNulo = default(object) }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaProyeccionCampoNuloSinWhere()
        {
            EQLPublicParser parser = new EQLPublicParser(
                    string.Format("from {0} select {1} as CampoNulo",
                                                                "SpaceObservable1",
                                                                "@event.Message.#0.[\"Campo que no existe\"]")
                                                                );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        CampoNulo = x.First().GetType().GetProperty("CampoNulo").GetValue(x.First())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(100, Notification.CreateOnNext((object)(new { CampoNulo = default(object) }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYCount()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave, {5} as Contador",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1",
                                                                                            "key.grupo1",
                                                                                            "count()")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave = x.First().GetType().GetProperty("Llave").GetValue(x.First()).ToString(),
                        Contador = int.Parse(x.First().GetType().GetProperty("Contador").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave = "0100", Contador = 2 }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }        

        [TestMethod]
        public void ConsultaSoloApplyWindowDosEventos1()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} apply window of {2} select {3} as Resultado",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:01'", // hay un comportamiento inesperado cuando el segundo parametro es 2 y se envian dos EventObject                                                                                        
                                                                                            "count()")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Resultado = int.Parse(x.First().GetType().GetProperty("Resultado").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Resultado = 2 }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaSoloApplyWindowDosEventos2()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} where {1} apply window of {2} select {3} as Resultado",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'", // hay un comportamiento inesperado cuando el segundo parametro es 2 y se envian dos EventObject                                                                                        
                                                                                            "count()")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Resultado = int.Parse(x.First().GetType().GetProperty("Resultado").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Resultado = 2 }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYSum()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave, {5} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1",
                                                                                            "key.grupo1",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave = x.First().GetType().GetProperty("Llave").GetValue(x.First()).ToString(),
                        Sumatoria = decimal.Parse(x.First().GetType().GetProperty("Sumatoria").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave = "0100", Sumatoria = 2m }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByDosLlavesYCount()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave1, {5} as Llave2, {6} as Contador",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1, @event.Message.#1.PrimaryAccountNumber as grupo2",
                                                                                            "key.grupo1",
                                                                                            "key.grupo2",
                                                                                            "count()")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave1 = x.First().GetType().GetProperty("Llave1").GetValue(x.First()).ToString(),
                        Llave2 = x.First().GetType().GetProperty("Llave2").GetValue(x.First()).ToString(),
                        Contador = int.Parse(x.First().GetType().GetProperty("Contador").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave1 = "0100", Llave2 = "9999941616073663", Contador = 2 }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByDosLlavesYSum()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave1, {5} as Llave2, {6} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1, @event.Message.#1.PrimaryAccountNumber as grupo2",
                                                                                            "key.grupo1",
                                                                                            "key.grupo2",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave1 = x.First().GetType().GetProperty("Llave1").GetValue(x.First()).ToString(),
                        Llave2 = x.First().GetType().GetProperty("Llave2").GetValue(x.First()).ToString(),
                        Sumatoria = decimal.Parse(x.First().GetType().GetProperty("Sumatoria").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave1 = "0100", Llave2 = "9999941616073663", Sumatoria = 2m }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYCountSinWhere()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} apply window of {1} group by {2} select {3} as Llave, {4} as Contador",
                                                                                            "SpaceObservable1",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1",
                                                                                            "key.grupo1",
                                                                                            "count()")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave = x.First().GetType().GetProperty("Llave").GetValue(x.First()).ToString(),
                        Contador = int.Parse(x.First().GetType().GetProperty("Contador").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave = "0100", Contador = 2 }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYSumSinWhere()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} apply window of {1} group by {2} select {3} as Llave, {4} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1",
                                                                                            "key.grupo1",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave = x.ElementAt(0).GetType().GetProperty("Llave").GetValue(x.ElementAt(0)).ToString(),
                        Sumatoria = decimal.Parse(x.ElementAt(0).GetType().GetProperty("Sumatoria").GetValue(x.ElementAt(0)).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave = "0100", Sumatoria = 2m }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYSumSinWhere2()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} apply window of {1} group by {2} select {3} as Llave",
                                                                                            "SpaceObservable1",
                                                                                            "'00:00:10'",
                                                                                            "@event.Message.#0.#0 as grupo1",
                                                                                            "key.grupo1",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave = x.ElementAt(0).GetType().GetProperty("Llave").GetValue(x.ElementAt(0)).ToString()
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave = "0100" }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByDosLlavesYCountSinWhere()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} apply window of {1} group by {2} select {3} as Llave1, {4} as Llave2, {5} as Contador",
                                                                                            "SpaceObservable1",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1, @event.Message.#1.PrimaryAccountNumber as grupo2",
                                                                                            "key.grupo1",
                                                                                            "key.grupo2",
                                                                                            "count()")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave1 = x.First().GetType().GetProperty("Llave1").GetValue(x.First()).ToString(),
                        Llave2 = x.First().GetType().GetProperty("Llave2").GetValue(x.First()).ToString(),
                        Contador = int.Parse(x.First().GetType().GetProperty("Contador").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave1 = "0100", Llave2 = "9999941616073663", Contador = 2 }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByDosLlavesYSumSinWhere()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} apply window of {1} group by {2} select {3} as Llave1, {4} as Llave2, {5} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1, @event.Message.#1.PrimaryAccountNumber as grupo2",
                                                                                            "key.grupo1",
                                                                                            "key.grupo2",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func < IQbservable<EventObject>, IObservable <IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave1 = x.First().GetType().GetProperty("Llave1").GetValue(x.First()).ToString(),
                        Llave2 = x.First().GetType().GetProperty("Llave2").GetValue(x.First()).ToString(),
                        Sumatoria = decimal.Parse(x.First().GetType().GetProperty("Sumatoria").GetValue(x.First()).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<object>>[] {
                    new Recorded<Notification<object>>(200, Notification.CreateOnNext((object)(new { Llave1 = "0100", Llave2 = "9999941616073663", Sumatoria = 2m }))),
                    new Recorded<Notification<object>>(200, Notification.CreateOnCompleted<object>())
                });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaLlaveSinGroupBySinWhere()
        {
            try
            {
                // no es posible utilizar key sin un group by
                EQLPublicParser parser = new EQLPublicParser(
                    string.Format("from {0} select {1} as Llave",
                                                                "SpaceObservable1",
                                                                "key")
                                                                );
                List<PlanNode> plan = parser.Parse();

                ObservableConstructor te = new ObservableConstructor();
                Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

                Assert.Inconclusive();
            }
            catch (CompilationException e)
            {
                // prueba exitosa porque es un error poner key sin group by
            }
            catch (Exception e)
            {
                if (!(e.InnerException is CompilationException))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void ConsultaLlaveSinGroupByConWhere()
        {
            try
            {
                // no es posible utilizar key sin un group by
                EQLPublicParser parser = new EQLPublicParser(
                    string.Format("from {0} where {1} select {2} as Llave",
                                                                "SpaceObservable1",
                                                                "@event.Message.#0.MessageType == \"0100\"",
                                                                "key")
                                                                );
                List<PlanNode> plan = parser.Parse();

                ObservableConstructor te = new ObservableConstructor();
                Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

                Assert.Inconclusive();
            }
            catch (CompilationException e)
            {
                // prueba exitosa porque es un error poner key sin group by
            }
            catch (Exception e)
            {
                if (!(e.InnerException is CompilationException))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void ConsultaCountSinGroupBySinWhere()
        {
            try
            {
                // no es posible utilizar key sin un group by
                EQLPublicParser parser = new EQLPublicParser(
                    string.Format("from {0} select {1} as Llave",
                                                                "SpaceObservable1",
                                                                "count()")
                                                                );
                List<PlanNode> plan = parser.Parse();

                ObservableConstructor te = new ObservableConstructor();
                Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

                Assert.Inconclusive();
            }
            catch (CompilationException e)
            {
                // prueba exitosa porque es un error poner key sin group by
            }
            catch (Exception e)
            {
                if (!(e.InnerException is CompilationException))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYSumErrorDeCasteoCampoNoNumerico()
        {
            EQLPublicParser parser = new EQLPublicParser(
                string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave, {5} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1",
                                                                                            "key.grupo1",
                                                                                            "sum((decimal)@event.Message.#1.CardAcceptorNameLocation)")
                                                                                            );
            List<PlanNode> plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<IQbservable<EventObject>, IObservable<object>> result = te.Compile<IQbservable<EventObject>, IObservable<object>>(plan.First());

            TestScheduler scheduler = new TestScheduler();

            ITestableObservable<EventObject> input = scheduler.CreateHotObservable(
                new Recorded<Notification<EventObject>>(100, Notification.CreateOnNext(TestObjects.EventObjectTest)),
                new Recorded<Notification<EventObject>>(200, Notification.CreateOnCompleted<EventObject>())
                );

            ITestableObserver<object> results = scheduler.Start(
                () => result(input.AsQbservable())
                .Select(x =>
                    (object)(new
                    {
                        Llave = x.GetType().GetProperty("Llave").GetValue(x).ToString(),
                        Sumatoria = decimal.Parse(x.GetType().GetProperty("Sumatoria").GetValue(x).ToString())
                    })
                ),
                created: 10,
                subscribed: 50,
                disposed: 400);

            ReactiveAssert.AssertEqual<string>(new string[] { (results.Messages.First().Value.Exception).InnerException.InnerException.Message }, "Specified cast is not valid.");

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                    new Subscription(50, 200)
                });
        }

        [TestMethod]
        public void ConsultaGroupByUnaLlaveYSumCampoNoNumerico()
        {
            try
            {
                EQLPublicParser parser = new EQLPublicParser(
                    string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave, {5} as Sumatoria",
                                                                                                "SpaceObservable1",
                                                                                                "@event.Message.#0.MessageType == \"0100\"",
                                                                                                "'00:00:00:01'",
                                                                                                "@event.Message.#0.MessageType as grupo1",
                                                                                                "key.grupo1",
                                                                                                "sum(@event.Message.#1.CardAcceptorNameLocation)")
                                                                                                );
                List<PlanNode> plan = parser.Parse();

                ObservableConstructor te = new ObservableConstructor();
                Func<IQbservable<EventObject>, IObservable<IEnumerable<object>>> result = te.Compile<IQbservable<EventObject>, IObservable<IEnumerable<object>>>(plan.First());

                Assert.Inconclusive();
            }
            catch (CompilationException e)
            {
                // prueba exitosa porque es un error poner key sin group by
            }
            catch (Exception e)
            {
                if (!(e.InnerException is CompilationException))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void CreacionCadenaDeConsultaDesdePlanDeEjecucion1()
        {
            string command = string.Format("from {0} where {1} apply window of {2} group by {3} select {4} as Llave1, {5} as Llave2, {6} as Contador, {7} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\" and @event.Message.#0.MessageType == \"0100\"",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1, @event.Message.#1.PrimaryAccountNumber as grupo2",
                                                                                            "key.grupo1",
                                                                                            "key.grupo2",
                                                                                            "count()",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)");
            EQLPublicParser parser = new EQLPublicParser(command);
            List<PlanNode> plan = parser.Parse();

            Assert.AreEqual<string>(command, plan.First().NodeText);
        }

        [TestMethod]
        public void CreacionCadenaDeConsultaDesdePlanDeEjecucion2()
        {
            string command = string.Format("from {0} apply window of {1} group by {2} select {3} as Llave1, {4} as Llave2, {5} as Contador, {6} as Sumatoria",
                                                                                            "SpaceObservable1",
                                                                                            "'00:00:00:01'",
                                                                                            "@event.Message.#0.MessageType as grupo1, @event.Message.#1.PrimaryAccountNumber as grupo2",
                                                                                            "key.grupo1",
                                                                                            "key.grupo2",
                                                                                            "count()",
                                                                                            "sum((decimal)@event.Message.#1.TransactionAmount)");
            EQLPublicParser parser = new EQLPublicParser(command);
            List<PlanNode> plan = parser.Parse();

            Assert.AreEqual<string>(command, plan.First().NodeText);
        }

        [TestMethod]
        public void CreacionCadenaDeConsultaDesdePlanDeEjecucion3()
        {
            string command = string.Format("from {0} where {1} select {2} as PrimaryAccountNumber, {3} as CardAcceptorNameLocation, {4} as TransactionAmount, {5} as TransactionCurrencyCode",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.MessageType == \"0100\" and @event.Message.#0.MessageType == \"0100\"",
                                                                                            "@event.Message.#0.PrimaryAccountNumber",
                                                                                            "@event.Message.#0.CardAcceptorNameLocation",
                                                                                            "@event.Message.#0.TransactionAmount",
                                                                                            "@event.Message.#0.TransactionCurrencyCode");
            EQLPublicParser parser = new EQLPublicParser(command);
            List<PlanNode> plan = parser.Parse();

            Assert.AreEqual<string>(command, plan.First().NodeText);
        }

        [TestMethod]
        public void CreacionCadenaDeConsultaDesdePlanDeEjecucion4()
        {
            string command = string.Format("from {0} select {1} as PrimaryAccountNumber, {2} as CardAcceptorNameLocation, {3} as TransactionAmount, {4} as TransactionCurrencyCode",
                                                                                            "SpaceObservable1",
                                                                                            "@event.Message.#0.PrimaryAccountNumber",
                                                                                            "@event.Message.#0.CardAcceptorNameLocation",
                                                                                            "@event.Message.#0.TransactionAmount",
                                                                                            "@event.Message.#0.TransactionCurrencyCode");
            EQLPublicParser parser = new EQLPublicParser(command);
            List<PlanNode> plan = parser.Parse();

            Assert.AreEqual<string>(command, plan.First().NodeText);
        }
    }
}
