﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    public class TestSuiteAttribute : Attribute
    {
        public TestSuiteAttribute(Type suiteClass)
        {
            //if (suiteType is ISuite)
            //{
            //    var instance = (ISuite)Activator.CreateInstance(suiteType);

            //} else
            //{
            //    throw new ArgumentException($"The type {suiteType.FullName} is an invalid parameter for {nameof(TestSuiteAttribute)}. " +
            //        $"It must implement the {nameof(ISuite)} interface.");
            //}
        }
    }
}
