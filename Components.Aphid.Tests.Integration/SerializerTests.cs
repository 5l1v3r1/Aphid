﻿using Components.Aphid.Parser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    [TestFixture(Category = "Serialization")]
    public class SerializerTests : AphidTests
    {
        protected override bool LoadStd
        {
            get { return true;  }
        }

        private void AssertSerialization(object expected, string objectDecl, string code)
        {
            var script = string.Format(
                "o = ({0}) |> serialize |> deserialize;\r\n{1}",
                objectDecl,
                code);

            AssertEquals(expected, script);
        }

        private void AssertFooDeserialization(string objectDecl, string code)
        {
            AssertSerialization("foo", objectDecl, code);
        }

        private void Assert9Deserialization(string objectDecl, string code)
        {
            AssertSerialization(9, objectDecl, code);
        }

        private void AssertTrueDeserialization(string objectDecl, string code)
        {
            AssertSerialization(true, objectDecl, code);
        }

        private void AssertFalseDeserialization(string objectDecl, string code)
        {
            AssertSerialization(false, objectDecl, code);
        }

        [Test]
        public void TestStringSerialization()
        {
            AssertFooDeserialization("'foo'", "ret o");
        }

        [Test]
        public void TestStringSerialization2()
        {
            AssertFooDeserialization("{ x: 'foo' }", "ret o.x");
        }

        [Test]
        public void TestStringSerialization3()
        {
            AssertFooDeserialization("{ vec: { x: 'foo' } }", "ret o.vec.x");
        }

        /// <summary>
        /// Todo: fix AphidObjectLexer.
        /// </summary>
        [Test]
        public void TestNumberSerialization()
        {
            Assert9Deserialization(
                "@{ using Components.Aphid.Interpreter; ret ValueHelper.Wrap(9) }()",
                "ret o");
        }

        [Test]
        public void TestNumberSerialization2()
        {
            Assert9Deserialization("{ x: 9 }", "ret o.x");
        }

        [Test]
        public void TestNumberSerialization3()
        {
            Assert9Deserialization("{ vec: { x: 9 } }", "ret o.vec.x");
        }

        [Test]
        public void TestListSerialization()
        {
            AssertFooDeserialization("[ 'foo' ]", "ret o[0]");
        }

        [Test]
        public void TestListSerialization2()
        {
            AssertFooDeserialization("[ null, 'foo' ]", "ret o[1]");
        }

        [Test]
        public void TestListSerialization3()
        {
            Assert9Deserialization("[ 9 ]", "ret o[0]");
        }

        [Test]
        public void TestListSerialization4()
        {
            Assert9Deserialization("[ null, 9 ]", "ret o[1]");
        }

        [Test]
        public void TestListSerialization5()
        {
            Assert9Deserialization("0..9", "ret o.Count");
        }

        [Test]
        public void TestListSerialization6()
        {
            AssertTrueDeserialization("0..0", "ret o.Count == 0");
        }

        [Test]
        public void TestListSerialization7()
        {
            AssertTrueDeserialization("0..0x1000", "ret o.Count == 0x1000");
        }

        [Test, Ignore]
        public void TestListSerialization8()
        {
            Assert9Deserialization(
                "(0..0x100)->@* 2",
                "ret o.Count == 0x100 && o[0x2] == 0x4");
        }

        [Test]
        public void TestBoolSerialization()
        {
            AssertTrueDeserialization("{ x: true }", "ret o.x");
        }

        [Test]
        public void TestBoolSerialization2()
        {
            AssertTrueDeserialization("{ x: false }", "ret !o.x");
        }

        [Test]
        public void TestBoolSerialization3()
        {
            AssertTrueDeserialization("{ foo: { x: true } }", "ret o.foo.x");
        }

        [Test]
        public void TestBoolSerialization4()
        {
            AssertTrueDeserialization("{ foo: { x: false } }", "ret !o.foo.x");
        }

        [Test]
        public void TestCircularSerialization()
        {
            AssertTrueDeserialization(
                "@{ a = { isActive: false }; a.self = a; ret a }()",
                "ret !o.self.isActive");
        }

        [Test]
        public void TestCircularSerialization2()
        {
            AssertTrueDeserialization(
                "@{ a = { isActive: false }; a.self = a; ret a }()",
                "ret !o.self.self.self.self.self.self.self.isActive");
        }

        [Test]
        public void TestCircularSerialization3()
        {
            Assert9Deserialization(
                @"@{
                    a = { position: { x: 10, y: 9 }, context: { objectModel: { } } };
                    a.context.objectModel.selfPosition = a.position;
                    ret a;
                }()",
                "ret o.context.objectModel.selfPosition.y");
        }

        [Test]
        public void TestCircularSerialization4()
        {
            Assert9Deserialization(
                @"@{
                    a = { x: 10, y: 20 };
                    a.window = { title: 'Test Window', resizable: false };
                    a.context = { datasource: [ 1, 2, 3, 4, 9 ], metadata = { ref: a, bar: {} } };
                    a.context.list = [ 5, 6, a ];
                    a.context.metadata.foo = a.context;
                    a.context.metadata.bar.w = a.context.datasource;
                    ret a;
                }()",
                "ret o.context.metadata.bar.w[4]");
        }

        [Test, Ignore]
        public void TestCircularSerialization5()
        {
            Assert9Deserialization(
                @"@{
                    a = { x: 10, y: 20 };
                    a.window = { title: 'Test Window', resizable: false };
                    a.context = { datasource: [ 1, 2, 3, 4, 9 ], metadata = { ref: a, bar: {} } };
                    a.context.list = [ 5, 6, a ];
                    a.context.metadata.foo = a.context;
                    a.context.metadata.bar.w = a.context.datasource;
                    ret a;
                }()",
                "ret o.context.list[2].context.metadata.bar.w[4]");
        }

        [Test]
        public void TestDynamicMemberCircularSerialization()
        {
            AssertTrueDeserialization(
                "@{ a = { isActive: false }; a.{'$self'} = a; ret a }()",
                "ret !o.{'$self'}.isActive");
        }

        [Test]
        public void TestDynamicMemberCircularSerialization2()
        {
            AssertFalseDeserialization(
                "@{ a = { isActive: true }; a.{'$self'} = a; ret a }()",
                "ret !o.{'$self'}.{'$self'}.{'$self'}.{'$self'}.{'$self'}.{'$self'}.isActive");
        }

        [Test]
        public void TestDynamicMemberCircularSerialization3()
        {
            AssertTrueDeserialization(
                @"@{
                    a = { isActive: true, '$internal': { } };
                    a.{'$internal'}.{'$self'} = a;
                    ret a;
                }()",
                "ret o.{'$internal'}.{'$self'}.isActive");
        }

        [Test]
        public void TestDynamicMemberCircularSerialization4()
        {
            AssertTrueDeserialization(
                @"@{
                    a = { isActive: true, '~!@': { } };
                    a.{'~!@'}.{'$self'} = a;
                    ret a;
                }()",
                "ret o.{'~!@'}.{'$self'}.isActive");
        }
    }
}
