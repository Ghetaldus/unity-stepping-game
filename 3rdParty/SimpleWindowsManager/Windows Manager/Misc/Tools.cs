﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Tools
{
    public static string GetMethodName(Func<IEnumerator> method)
    {
        return method.Method.Name;
    }
}