﻿using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSample
{
    class Program
    {
        static void Main(string[] args)
        {
            new AphidInterpreter().Interpret(@"
                #'std';
                list = [ 10, 20, 'foo' ];

                printf(
                    'Count: {0}, Contains foo: {1}',
                    list.count(),
                    list.contains('foo'));
                
                for (x in list)
                    print(x);
            ");
        }
    }
}
